#include <iostream>
#include <winsock2.h>
#include <string>

#pragma comment(lib, "ws2_32.lib")

int main() {
    WSADATA wsa;
    WSAStartup(MAKEWORD(2,2), &wsa);

    SOCKET serverSock = socket(AF_INET, SOCK_STREAM, 0);

    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_port = htons(5000);
    addr.sin_addr.s_addr = INADDR_ANY;

    bind(serverSock, (sockaddr*)&addr, sizeof(addr));
    listen(serverSock, 5);

    std::cout << "Server started...\n";

    while (true) {
        SOCKET client = accept(serverSock, NULL, NULL);
        std::cout << "Client connected\n";

        char buffer[1024];

        while (true) {
            int bytes = recv(client, buffer, 1024, 0);

            if (bytes <= 0) {
                std::cout << "Client disconnected\n";
                break;
            }

            std::string msg(buffer, bytes);

            std::string response = "ECHO: " + msg;

            send(client, response.c_str(), response.size(), 0);
        }

        closesocket(client);
    }

    closesocket(serverSock);
    WSACleanup();
}