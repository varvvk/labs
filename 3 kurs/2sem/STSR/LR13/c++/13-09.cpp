#include <iostream>
#include <winsock2.h>

#pragma comment(lib, "ws2_32.lib")

int main() {
    WSADATA wsa;
    WSAStartup(MAKEWORD(2,2), &wsa);

    SOCKET s = socket(AF_INET, SOCK_DGRAM, 0);

    sockaddr_in addr{};
    addr.sin_family = AF_INET;
    addr.sin_port = htons(8000);
    addr.sin_addr.s_addr = INADDR_ANY;

    bind(s, (sockaddr*)&addr, sizeof(addr));

    std::cout << "UDP server started...\n";

    char buffer[1024];
    sockaddr_in client;
    int len = sizeof(client);

    while (true) {
        int bytes = recvfrom(s, buffer, 1024, 0,
                            (sockaddr*)&client, &len);

        if (bytes <= 0) continue;

        buffer[bytes] = '\0';

        std::string resp = "ECHO: ";
        resp += buffer;

        sendto(s, resp.c_str(), resp.size(), 0,
               (sockaddr*)&client, len);
    }

    closesocket(s);
    WSACleanup();
}