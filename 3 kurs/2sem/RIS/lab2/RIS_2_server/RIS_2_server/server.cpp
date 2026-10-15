#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <iostream>
#include <string>
#include <tchar.h>
#include <time.h>
#include "Winsock2.h"
#include <map>

#pragma comment(lib, "WS2_32.lib")

using namespace std;

struct CA
{
    char ipaddr[15];
    char resurce[20];
    int number;
    enum Status { NOINIT, INIT, ENTER, LEAVE, WAIT } status;
};

map<string, CA> resources;

int _tmain(int argc, _TCHAR* argv[])
{
    cout << "server run" << endl;

    SOCKET sS;
    WSADATA wsaData;

    WSAStartup(MAKEWORD(2, 0), &wsaData);
    sS = socket(AF_INET, SOCK_DGRAM, NULL);

    SOCKADDR_IN serv;
    serv.sin_family = AF_INET;
    serv.sin_port = htons(2000);
    serv.sin_addr.s_addr = INADDR_ANY;

    bind(sS, (LPSOCKADDR)&serv, sizeof(serv));

    while (true)
    {
        SOCKADDR_IN clnt;
        int lc = sizeof(clnt);

        CA ibuf;

        recvfrom(sS, (char*)&ibuf, sizeof(ibuf), NULL, (sockaddr*)&clnt, &lc);

        string key = ibuf.resurce;

        // --- ENTER / WAIT ---
        if (ibuf.status == CA::ENTER || ibuf.status == CA::WAIT)
        {
            auto it = resources.find(key);

            if (it != resources.end())
            {
                // ресурс занят
                ibuf.status = CA::WAIT;
                cout << "Client " << ibuf.number << " WAIT for resource\n";
            }
            else
            {
                // свободен
                resources[key] = ibuf;
                ibuf.status = CA::ENTER;
                cout << "Client " << ibuf.number << " ENTER critical section\n";
            }
        }

        // --- LEAVE ---
        else if (ibuf.status == CA::LEAVE)
        {
            resources.erase(key);
            ibuf.status = CA::LEAVE;
            cout << "Client " << ibuf.number << " LEAVE critical section\n";
        }

        // --- INIT ---
        else if (ibuf.status == CA::INIT)
        {
            cout << "Client " << ibuf.number << " INIT\n";
        }

        // --- NOINIT ---
        else if (ibuf.status == CA::NOINIT)
        {
            cout << "Client " << ibuf.number << " DESTROY\n";
        }

        sendto(sS, (char*)&ibuf, sizeof(ibuf), NULL, (sockaddr*)&clnt, sizeof(clnt));
    }

    closesocket(sS);
    WSACleanup();

    return 0;
}