// client.cpp
#include <iostream>
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib")

int main() {
    WSADATA wsa;
    WSAStartup(MAKEWORD(2,2), &wsa);

    SOCKET sock = socket(AF_INET, SOCK_STREAM, 0);

    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_port = htons(5000);
    addr.sin_addr.s_addr = inet_addr("127.0.0.1");

    connect(sock, (sockaddr*)&addr, sizeof(addr));

    send(sock, "Hello C++", 9, 0);

    char buffer[1024];
    int bytes = recv(sock, buffer, 1024, 0);
    buffer[bytes] = '\0';

    std::cout << buffer << std::endl;

    closesocket(sock);
    WSACleanup();
}