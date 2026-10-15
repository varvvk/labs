#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <chrono>
#include <thread>
#include <climits>
#include <iomanip>
#include <cstring>
#include <cstdint>
#include <algorithm>

#pragma comment(lib, "ws2_32.lib")

#define SERVER_IP "26.162.87.27"
#define SERVER_PORT 5000
#define REQUEST_INTERVAL 14
#define NUM_REQUESTS 10

struct GETSINCHRO {
    char cmd[8];
    int64_t curvalue;
};

struct SETSINCRO {
    char cmd[8];
    int64_t correction;
};

uint64_t getOSTime() {
    auto now = std::chrono::system_clock::now();
    auto duration =
        std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch());
    return duration.count();
}

void runClient() {

    SYSTEMTIME tm;
    GETSINCHRO getsincro;
    SETSINCRO setsincro;

    memset(&setsincro, 0, sizeof(setsincro));
    memset(&getsincro, 0, sizeof(getsincro));

    strcpy_s(getsincro.cmd, "SINC");

    int64_t Cc = getOSTime();
    getsincro.curvalue = Cc;

    std::cout << "Client run\n";
    std::cout << "Connecting to server: "
        << SERVER_IP << ":" << SERVER_PORT << "\n";

    WSADATA wsaData;

    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "[ERROR] WSAStartup failed\n";
        return;
    }

    SOCKET clientSock = socket(AF_INET, SOCK_DGRAM, 0);

    if (clientSock == INVALID_SOCKET) {
        std::cerr << "[ERROR] Socket creation failed: "
            << WSAGetLastError() << "\n";
        WSACleanup();
        return;
    }

    DWORD timeout = 3000;
    setsockopt(clientSock, SOL_SOCKET, SO_RCVTIMEO,
        (char*)&timeout, sizeof(timeout));

    SOCKADDR_IN serverAddr;

    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(SERVER_PORT);
    inet_pton(AF_INET, SERVER_IP, &serverAddr.sin_addr);

    int64_t maxcor = INT64_MIN;
    int64_t mincor = INT64_MAX;
    int64_t totalCorrection = 0;
    int64_t totalDiff = 0;

    for (int i = 0; i < NUM_REQUESTS; ++i) {

        GetSystemTime(&tm);

        getsincro.curvalue = Cc;

        int sent = sendto(clientSock,
            (char*)&getsincro,
            sizeof(getsincro),
            0,
            (sockaddr*)&serverAddr,
            sizeof(serverAddr));

        if (sent == SOCKET_ERROR) {
            std::cerr << "[ERROR] Send failed: "
                << WSAGetLastError() << "\n";
            continue;
        }

        int serverAddrLen = sizeof(serverAddr);

        int recvLen = recvfrom(clientSock,
            (char*)&setsincro,
            sizeof(setsincro),
            0,
            (sockaddr*)&serverAddr,
            &serverAddrLen);

        if (recvLen == SOCKET_ERROR) {
            std::cerr << "[ERROR] Receive failed: "
                << WSAGetLastError() << "\n";
            continue;
        }

        if (strncmp(setsincro.cmd, "SINCRO", 6) != 0) {
            std::cerr << "[ERROR] Unexpected response: "
                << setsincro.cmd << "\n";
            continue;
        }

        uint64_t ostime = getOSTime();

        Cc = Cc + setsincro.correction;

        if (setsincro.correction > maxcor) maxcor = setsincro.correction;
        if (setsincro.correction < mincor) mincor = setsincro.correction;

        totalCorrection += setsincro.correction;

        int64_t diff = Cc - (int64_t)ostime;
        totalDiff += diff;

        std::cout << "----------------------------------\n";

        std::cout << tm.wYear << "-"
            << tm.wMonth << "-"
            << tm.wDay << " "
            << tm.wHour + 3 << ":"
            << tm.wMinute << ":"
            << tm.wSecond << "."
            << tm.wMilliseconds << "\n";

        std::cout << "Request #" << i + 1 << "\n";

        std::cout << "Client time before correction: "
            << getsincro.curvalue << " ms\n";

        std::cout << "Correction from server: "
            << setsincro.correction << " ms\n";

        std::cout << "Client time after correction: "
            << Cc << " ms\n";

        std::cout << "OS time: "
            << ostime << " ms\n";

        std::cout << "Difference (Cc - OS): "
            << diff << " ms\n";

        std::cout << "Stats - Max corr: "
            << maxcor
            << " Min corr: "
            << mincor
            << " Avg corr: "
            << (totalCorrection / (i + 1))
            << "\n";

        std::cout << "----------------------------------\n\n";

        if (i < NUM_REQUESTS - 1) {

            std::this_thread::sleep_for(
                std::chrono::seconds(REQUEST_INTERVAL));

            Cc += REQUEST_INTERVAL * 1000;
        }
    }

    std::cout << "\n=========== FINAL STATISTICS ===========\n";

    std::cout << "Total requests: "
        << NUM_REQUESTS << "\n";

    std::cout << "Average correction: "
        << (totalCorrection / NUM_REQUESTS)
        << " ms\n";

    std::cout << "Average (Cc - OS time): "
        << (totalDiff / NUM_REQUESTS)
        << " ms\n";

    std::cout << "Max correction: "
        << maxcor << " ms\n";

    std::cout << "Min correction: "
        << mincor << " ms\n";

    std::cout << "========================================\n\n";

    closesocket(clientSock);
    WSACleanup();
}

int main() {

    runClient();

    std::cout << "Press any key to exit...\n";
    getchar();

    return 0;
}