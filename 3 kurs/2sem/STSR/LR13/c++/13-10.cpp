#include <iostream>
#include <string>
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib")

int main() {
    WSADATA wsa;
    WSAStartup(MAKEWORD(2,2), &wsa);

    SOCKET s = socket(AF_INET, SOCK_DGRAM, 0);

    sockaddr_in server{};
    server.sin_family = AF_INET;
    server.sin_port = htons(8000);
    server.sin_addr.s_addr = inet_addr("127.0.0.1");

    char buffer[1024];
    sockaddr_in from;
    int fromLen = sizeof(from);

    std::string msg;

    std::cout << "Enter message: ";
    std::getline(std::cin, msg);

    // 🔹 отправка на сервер
    sendto(s, msg.c_str(), msg.size(), 0,
           (sockaddr*)&server, sizeof(server));

    // 🔹 получение ответа
    int bytes = recvfrom(s, buffer, 1024, 0,
                         (sockaddr*)&from, &fromLen);

    if (bytes > 0) {
        buffer[bytes] = '\0';
        std::cout << buffer << std::endl;
    }

    closesocket(s);
    WSACleanup();

    return 0;
}