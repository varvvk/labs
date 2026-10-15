#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <chrono>
#include <thread>
#include <atomic>
#include <vector>
#include <algorithm>
#include <cstring>
#include <cstdint>

#pragma comment(lib, "ws2_32.lib")

#define SERVER_PORT 5000
#define NTP_SERVER "pool.ntp.org"
#define NTP_PORT 123
#define NTP_TIMESTAMP_DELTA 2208988800ull
#define UPDATE_INTERVAL 10000

std::atomic<uint64_t> Cs(0);          // время полученное от NTP
std::atomic<uint64_t> syncMoment(0);  // момент синхронизации

struct NTPPacket {
    uint8_t li_vn_mode;
    uint8_t stratum;
    uint8_t poll;
    uint8_t precision;
    uint32_t root_delay;
    uint32_t root_dispersion;
    uint32_t reference_id;
    uint32_t reference_timestamp[2];
    uint32_t originate_timestamp[2];
    uint32_t receive_timestamp[2];
    uint32_t transmit_timestamp[2];
};

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

uint64_t getServerTime() {
    uint64_t ntp = Cs.load();
    uint64_t sync = syncMoment.load();
    uint64_t now = getOSTime();

    return ntp + (now - sync);
}

uint64_t getNTPTime() {

    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) {
        std::cerr << "WSAStartup failed\n";
        return 0;
    }

    SOCKET sock = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (sock == INVALID_SOCKET) {
        WSACleanup();
        return 0;
    }

    DWORD timeout = 5000;
    setsockopt(sock, SOL_SOCKET, SO_RCVTIMEO, (char*)&timeout, sizeof(timeout));

    sockaddr_in serverAddr;
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(NTP_PORT);

    hostent* host = gethostbyname(NTP_SERVER);
    if (host == NULL) {
        closesocket(sock);
        WSACleanup();
        return 0;
    }

    serverAddr.sin_addr.s_addr = *(u_long*)host->h_addr_list[0];

    NTPPacket packet = {};
    packet.li_vn_mode = (0 << 6) | (4 << 3) | 3;

    sendto(sock, (char*)&packet, sizeof(packet), 0,
        (sockaddr*)&serverAddr, sizeof(serverAddr));

    int addrLen = sizeof(serverAddr);

    int recvLen = recvfrom(sock, (char*)&packet, sizeof(packet), 0,
        (sockaddr*)&serverAddr, &addrLen);

    if (recvLen == SOCKET_ERROR) {
        closesocket(sock);
        WSACleanup();
        return 0;
    }

    closesocket(sock);
    WSACleanup();

    uint64_t ntp_time =
        ntohl(packet.transmit_timestamp[0]) - NTP_TIMESTAMP_DELTA;

    uint64_t milliseconds =
        ntp_time * 1000 +
        (uint64_t)((double)ntohl(packet.transmit_timestamp[1]) * 1000.0 / 0xFFFFFFFF);

    return milliseconds;
}

void updateTime() {

    while (true) {

        uint64_t new_time = getNTPTime();

        if (new_time > 0) {

            Cs.store(new_time);
            syncMoment.store(getOSTime());

            std::cout << "[SYNC] NTP synchronized\n";
        }
        else {
            std::cout << "[ERROR] NTP sync failed\n";
        }

        std::this_thread::sleep_for(
            std::chrono::milliseconds(UPDATE_INTERVAL));
    }
}

void runServer() {

    SETSINCRO setsincro;
    GETSINCHRO getsincro;

    ZeroMemory(&setsincro, sizeof(setsincro));
    ZeroMemory(&getsincro, sizeof(getsincro));

    strcpy_s(setsincro.cmd, "SINCRO");
    strcpy_s(getsincro.cmd, "SINC");

    SYSTEMTIME tm;

    std::cout << "Server Run\n";

    WSADATA wsaData;

    if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0) {
        std::cerr << "WSAStartup failed\n";
        return;
    }

    SOCKET serverSock = socket(AF_INET, SOCK_DGRAM, 0);

    if (serverSock == INVALID_SOCKET) {
        WSACleanup();
        return;
    }

    SOCKADDR_IN serverAddr;

    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons(SERVER_PORT);

    if (bind(serverSock, (SOCKADDR*)&serverAddr, sizeof(serverAddr)) == SOCKET_ERROR) {

        std::cerr << "Bind failed\n";
        closesocket(serverSock);
        WSACleanup();
        return;
    }

    std::cout << "Listening on port " << SERVER_PORT << "\n";

    while (true) {

        SOCKADDR_IN client;
        int lc = sizeof(client);

        int recvLen = recvfrom(serverSock,
            (char*)&getsincro,
            sizeof(getsincro),
            0,
            (sockaddr*)&client,
            &lc);

        if (recvLen > 0 && strncmp(getsincro.cmd, "SINC", 4) == 0) {

            GetSystemTime(&tm);

            uint64_t serverTime = getServerTime();

            setsincro.correction =
                (int64_t)(serverTime - getsincro.curvalue);

            sendto(serverSock,
                (char*)&setsincro,
                sizeof(setsincro),
                0,
                (sockaddr*)&client,
                sizeof(client));

            std::cout
                << tm.wMonth << "."
                << tm.wDay << "."
                << tm.wYear << " - "
                << tm.wHour + 3 << ":"
                << tm.wMinute << ":"
                << tm.wSecond << "."
                << tm.wMilliseconds << "\n";

            std::cout
                << "Client: "
                << inet_ntoa(client.sin_addr)
                << " Client time: "
                << getsincro.curvalue
                << " Correction: "
                << setsincro.correction
                << "\n\n";
        }
    }

    closesocket(serverSock);
    WSACleanup();
}

int main() {

    std::cout << "Starting Time Synchronization Server\n";
    std::cout << "Using NTP server: " << NTP_SERVER << "\n\n";

    std::thread timeThread(updateTime);

    std::this_thread::sleep_for(std::chrono::seconds(2));

    runServer();

    timeThread.join();

    return 0;
}