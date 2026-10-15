#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <map>
#include <string>
#include <queue>
#include <mutex>
#include <thread>
#include "Winsock2.h"

#pragma comment(lib, "WS2_32.lib")
using namespace std;

struct FileEntry {
    string fileName;
    bool inUse = false;
    queue<SOCKADDR_IN> waitQueue;
    mutex mtx;
};

SOCKET serverSocket;
map<string, FileEntry*> files;
mutex mapMutex;

bool SameClient(SOCKADDR_IN a, SOCKADDR_IN b) {
    return a.sin_addr.s_addr == b.sin_addr.s_addr &&
        a.sin_port == b.sin_port;
}

string GetClientAddr(SOCKADDR_IN& addr) {
    return string(inet_ntoa(addr.sin_addr)) + ":" + to_string(ntohs(addr.sin_port));
}

bool InQueue(queue<SOCKADDR_IN> q, SOCKADDR_IN client) {
    while (!q.empty()) {
        if (SameClient(q.front(), client))
            return true;
        q.pop();
    }
    return false;
}

void ProcessRequest(char* buf, SOCKADDR_IN clientAddr, int clientAddrSize) {
    string msg(buf);
    char command = msg[0];
    string fileName = msg.substr(2);

    FileEntry* fEntry;

    {
        lock_guard<mutex> guard(mapMutex);
        if (files.find(fileName) == files.end()) {
            files[fileName] = new FileEntry();
            files[fileName]->fileName = fileName;
        }
        fEntry = files[fileName];
    }

    lock_guard<mutex> lock(fEntry->mtx);

    if (command == 'e') {
        if (!fEntry->inUse) {
            fEntry->inUse = true;

            const char* msg = "ENTER";
            sendto(serverSocket, msg, strlen(msg), 0,
                (sockaddr*)&clientAddr, clientAddrSize);

            cout << "[ENTER] " << fileName << " -> " << GetClientAddr(clientAddr) << endl;
        }
        else {
            if (!InQueue(fEntry->waitQueue, clientAddr)) {
                fEntry->waitQueue.push(clientAddr);
            }

            const char* msg = "WAIT";
            sendto(serverSocket, msg, strlen(msg), 0,
                (sockaddr*)&clientAddr, clientAddrSize);

            cout << "[WAIT] " << fileName << " -> " << GetClientAddr(clientAddr) << endl;
        }
    }

    else if (command == 'l') {
        cout << "[LEAVE] " << fileName << " <- " << GetClientAddr(clientAddr) << endl;

        // удаояем клиента из очереди (если он там есть)
        queue<SOCKADDR_IN> newQueue;

        while (!fEntry->waitQueue.empty()) {
            SOCKADDR_IN c = fEntry->waitQueue.front();
            fEntry->waitQueue.pop();

            if (!SameClient(c, clientAddr)) {
                newQueue.push(c);
            }
        }

        fEntry->waitQueue = newQueue;

        // передаём доступ следующему
        if (!fEntry->waitQueue.empty()) {
            SOCKADDR_IN next = fEntry->waitQueue.front();
            fEntry->waitQueue.pop();

            const char* msg = "ENTER";
            sendto(serverSocket, msg, strlen(msg), 0,
                (sockaddr*)&next, sizeof(next));

            cout << "[NEXT ENTER] " << fileName << " -> " << GetClientAddr(next) << endl;
        }
        else {
            fEntry->inUse = false;
        }
    }

    else if (command == 'i') {
        cout << "[INIT] " << fileName << " <- " << GetClientAddr(clientAddr) << endl;
    }

    else if (command == 'c') {
        cout << "[CLOSE] " << fileName << " <- " << GetClientAddr(clientAddr) << endl;
    }
}

int main() {
    WSADATA wsaData;
    WSAStartup(MAKEWORD(2, 0), &wsaData);

    serverSocket = socket(AF_INET, SOCK_DGRAM, 0);

    SOCKADDR_IN serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(2000);
    serverAddr.sin_addr.s_addr = INADDR_ANY;

    bind(serverSocket, (sockaddr*)&serverAddr, sizeof(serverAddr));

    cout << "DFS Server running...\n";

    while (true) {
        char buf[256];
        SOCKADDR_IN clientAddr;
        int clientAddrSize = sizeof(clientAddr);

        int bytes = recvfrom(serverSocket, buf, sizeof(buf), 0,
            (sockaddr*)&clientAddr, &clientAddrSize);


        if (bytes > 0) {
            buf[bytes] = '\0';
            thread(ProcessRequest, buf, clientAddr, clientAddrSize).detach();
        }
    }

    closesocket(serverSocket);
    WSACleanup();
    return 0;
}
