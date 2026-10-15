#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <fstream>
#include <string>
#include <thread>
#include <chrono>
#include <sstream>
#include <vector>
#include "Winsock2.h"

#pragma comment(lib, "WS2_32.lib")
using namespace std;

struct HDFS {
    fstream file;
    bool isValid = false;
    operator bool() const { return isValid; }
};

struct DFSClient {
    char ipaddr[16];
    char fileName[256];
    int clientNumber;
    SOCKET sock;
    SOCKADDR_IN serverAddr;
} client;

void InitClient(const char* ip, const char* file, int number) {
    strncpy(client.ipaddr, ip, sizeof(client.ipaddr));
    strncpy(client.fileName, file, sizeof(client.fileName));
    client.clientNumber = number;

    WSADATA wsa;
    WSAStartup(MAKEWORD(2, 0), &wsa);

    client.sock = socket(AF_INET, SOCK_DGRAM, 0);

    client.serverAddr.sin_family = AF_INET;
    client.serverAddr.sin_port = htons(2000);
    client.serverAddr.sin_addr.s_addr = inet_addr(client.ipaddr);

    string msg = "i " + string(client.fileName);
    sendto(client.sock, msg.c_str(), msg.size(), 0,
        (sockaddr*)&client.serverAddr, sizeof(client.serverAddr));

    cout << "[Client " << client.clientNumber << "] INIT\n";
}

void WaitEnter() {
    char buf[16];
    SOCKADDR_IN srv;
    int sz = sizeof(srv);

    while (true) {
        int bytes = recvfrom(client.sock, buf, sizeof(buf), 0, (sockaddr*)&srv, &sz);
        if (bytes <= 0) continue;

        buf[bytes] = '\0';

        if (string(buf) == "ENTER") {
            cout << "[Client " << client.clientNumber << "] ENTER\n";
            return;
        }

        if (string(buf) == "WAIT") {
            cout << "[Client " << client.clientNumber << "] WAIT...\n";
        }
    }
}

void EnterDFS() {
    string msg = "e " + string(client.fileName);
    sendto(client.sock, msg.c_str(), msg.size(), 0,
        (sockaddr*)&client.serverAddr, sizeof(client.serverAddr));

    WaitEnter();
}

void LeaveDFS() {
    string msg = "l " + string(client.fileName);
    sendto(client.sock, msg.c_str(), msg.size(), 0,
        (sockaddr*)&client.serverAddr, sizeof(client.serverAddr));

    cout << "[Client " << client.clientNumber << "] LEAVE\n";
}

HDFS OpenDFSFile(const char* fileName) {
    HDFS hdfs;

    EnterDFS();

    hdfs.file.open(fileName, ios::in | ios::out);

    if (!hdfs.file.is_open()) {
        hdfs.file.open(fileName, ios::out);
        hdfs.file.close();
        hdfs.file.open(fileName, ios::in | ios::out);
    }

    hdfs.isValid = hdfs.file.is_open();
    return hdfs;
}

int WriteDFSFile(HDFS& hdfs, void* buf, int size) {
    if (!hdfs) return -1;
    hdfs.file.seekp(0, ios::end);
    hdfs.file.write((char*)buf, size);
    hdfs.file.flush();
    return size;
}

int ReadDFSFile(HDFS& hdfs, void* buf, int bufsize) {
    if (!hdfs) return -1;
    hdfs.file.read((char*)buf, bufsize);
    return (int)hdfs.file.gcount(); // количество реально прочитанных байт
}


void CloseDFSFile(HDFS& hdfs) {
    hdfs.file.close();
    LeaveDFS();
}

int main() {
    setlocale(LC_ALL, "rus");

    InitClient("10.209.80.52", "Z:\\file.txt", 1);

    HDFS hdfs = OpenDFSFile("Z:\\file.txt");

    if (!hdfs) {
        cout << "Ошибка открытия файла\n";
        return 0;
    }

    // запись 10 строк
    for (int i = 0; i < 10; i++) {
        time_t t = time(0);
        string data = "Client " + to_string(client.clientNumber) +
            " line " + to_string(i + 1) + " " + ctime(&t) + "\n";

        WriteDFSFile(hdfs, (void*)data.c_str(), data.size());

        this_thread::sleep_for(chrono::milliseconds(500));
    }

    hdfs.file.clear(); // сброс флагов ошибок
    hdfs.file.seekg(0, ios::beg); // переместить указатель чтения в начало

    cout << "--- Последние 10 строк ---\n";
    char buf[1024];
    string content;
    int bytesRead;

    // читаем весь файл в string
    while ((bytesRead = ReadDFSFile(hdfs, buf, sizeof(buf))) > 0) {
        content.append(buf, bytesRead);
    }

    // выводим последние 10 строк
    vector<string> lines;
    string line;
    istringstream iss(content);
    while (getline(iss, line)) {
        if (!line.empty())
            lines.push_back(line);
    }

    int start = max(0, (int)lines.size() - 10);
    for (int i = start; i < lines.size(); i++) {
        cout << lines[i] << "\n";
    }

    CloseDFSFile(hdfs);

    closesocket(client.sock);
    WSACleanup();
    return 0;
}