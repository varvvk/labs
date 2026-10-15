#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <stdio.h>
#include <time.h>
#include <WinSock2.h>
#include <thread>
#include <vector>
#include <string>
#include <fstream>
#include <iostream>
#include <algorithm>
#pragma comment(lib, "WS2_32.lib")

SOCKET sS;
std::string my_ip;
std::string coordinator_ip = "";
bool is_coordinator = false;
int missed_pings = 0;
bool election_in_progress = false;
bool got_ok_in_election = false;

std::vector<std::string> cluster_ips;

void printErrorMessage(const char* message) {
    printf("%s: %d\n", message, WSAGetLastError());
}

void loadClusterNodes(const std::string& filename) {
    std::ifstream file(filename);
    std::string ip;
    if (file.is_open()) {
        cluster_ips.clear();
        while (std::getline(file, ip)) {
            if (!ip.empty()) {
                cluster_ips.push_back(ip);
            }
        }
        file.close();
    }
    else {
        printf("[Error] Cannot open nodes.txt!\n");
    }
}

void sendMessage(const std::string& ip, const std::string& msg) {
    struct sockaddr_in dest = { 0 };
    dest.sin_family = AF_INET;
    dest.sin_port = htons(5555);
    dest.sin_addr.s_addr = inet_addr(ip.c_str());
    sendto(sS, msg.c_str(), (int)msg.length(), 0, (struct sockaddr*)&dest, sizeof(dest));
}

void ElectionAndPingThread() {
    while (true) {
        Sleep(5000);

        if (is_coordinator) {
            continue;
        }

        if (coordinator_ip.empty()) {
            missed_pings = 3;
        }
        else {
            sendMessage(coordinator_ip, "ping");
            missed_pings++;
        }

        if (missed_pings >= 3 && !election_in_progress) {
            printf("[Bully] Coordinator is down! Starting election...\n");
            election_in_progress = true;
            got_ok_in_election = false;

            unsigned long my_ip_num = ntohl(inet_addr(my_ip.c_str()));

            for (const auto& ip : cluster_ips) {
                unsigned long other_ip_num = ntohl(inet_addr(ip.c_str()));
                if (other_ip_num > my_ip_num) {
                    sendMessage(ip, "election");
                }
            }

            Sleep(2000);

            if (!got_ok_in_election) {
                printf("[Bully] I won, I am the new coordinator.\n");
                is_coordinator = true;
                coordinator_ip = my_ip;
                election_in_progress = false;
                missed_pings = 0;

                std::ofstream outfile("config.txt");
                if (outfile.is_open()) {
                    outfile << my_ip;
                    outfile.close();
                }

                for (const auto& ip : cluster_ips) {
                    if (ip != my_ip) {
                        sendMessage(ip, "iamcoordinator");
                    }
                }
            }
            else {
                printf("[Bully] Received OK from higher node. Dropping candidacy.\n");
                election_in_progress = false;
                missed_pings = 0;
            }
        }
    }
}

void ListenerThread() {
    char recvbuffer[256];
    struct sockaddr_in caddr = { 0 };
    int csize = sizeof(caddr);

    while (true) {
        int recvResult = recvfrom(sS, recvbuffer, sizeof(recvbuffer) - 1, 0, (struct sockaddr*)&caddr, &csize);
        if (recvResult > 0) {
            recvbuffer[recvResult] = '\0';
            std::string msg(recvbuffer);
            std::string sender_ip = inet_ntoa(caddr.sin_addr);

            if (msg == "gettime") {
                if (is_coordinator) {
                    time_t rawtime;
                    struct tm* timeinfo;
                    char timebuffer[80];
                    time(&rawtime);
                    timeinfo = localtime(&rawtime);
                    strftime(timebuffer, sizeof(timebuffer), "%d%m%Y:%H:%M:%S", timeinfo);

                    sendto(sS, timebuffer, (int)strlen(timebuffer), 0, (struct sockaddr*)&caddr, csize);
                    printf("[Server] Time sent to agent: %s\n", sender_ip.c_str());
                }
            }
            else if (msg == "ping") {
                sendMessage(sender_ip, "pong");
            }
            else if (msg == "pong") {
                missed_pings = 0;
            }
            else if (msg == "election") {
                sendMessage(sender_ip, "ok");
                coordinator_ip = "";
                missed_pings = 3;
            }
            else if (msg == "ok") {
                got_ok_in_election = true;
            }
            else if (msg == "iamcoordinator") {
                printf("[Bully] Node %s announced as new coordinator.\n", sender_ip.c_str());
                coordinator_ip = sender_ip;
                is_coordinator = false;
                missed_pings = 0;
                election_in_progress = false;
            }
        }
    }
}

int main(int argc, char* argv[]) {
    if (argc < 2) {
        printf("Usage: %s <IP_ADDRESS>\n", argv[0]);
        return -1;
    }
    my_ip = argv[1];

    loadClusterNodes("nodes.txt");

    if (std::find(cluster_ips.begin(), cluster_ips.end(), my_ip) == cluster_ips.end()) {
        printf("[Error] IP address %s is not in nodes.txt! Access denied.\n", my_ip.c_str());
        return -1;
    }

    WSAData wsadata;
    if (WSAStartup(MAKEWORD(2, 2), &wsadata) != 0) {
        printErrorMessage("WSAStartup error");
        return -1;
    }

    sS = socket(AF_INET, SOCK_DGRAM, 0);
    if (sS == INVALID_SOCKET) {
        printErrorMessage("Socket creation error");
        return -1;
    }

    struct sockaddr_in saddr = { 0 };
    saddr.sin_family = AF_INET;
    saddr.sin_port = htons(5555);
    saddr.sin_addr.s_addr = inet_addr(my_ip.c_str());

    if (bind(sS, (struct sockaddr*)&saddr, sizeof(saddr)) == SOCKET_ERROR) {
        printf("Bind error for IP %s\n", my_ip.c_str());
        closesocket(sS);
        WSACleanup();
        return -1;
    }

    printf("Time Server started on IP: %s\n", my_ip.c_str());

    std::thread listener(ListenerThread);
    std::thread pinger(ElectionAndPingThread);

    listener.join();
    pinger.join();

    closesocket(sS);
    WSACleanup();
    return 0;
}