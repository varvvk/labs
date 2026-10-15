#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <string>
#include <tchar.h>
#include <ctime>
#include <fstream>
#include "Winsock2.h"

#pragma comment(lib, "WS2_32.lib")

using namespace std;

struct CA
{
    char ipaddr[15];
    char resurce[20];
    int clientnumber;

    enum Status
    {
        NOINIT,
        INIT,
        ENTER,
        LEAVE,
        WAIT
    } status;
};

string GetStatusText(CA::Status status) {
    switch (status) {
    case CA::NOINIT: return "NOINIT";
    case CA::INIT:   return "INIT";
    case CA::ENTER:  return "ENTER";
    case CA::LEAVE:  return "LEAVE";
    case CA::WAIT:   return "WAIT";
    default:         return "UNKNOWN";
    }
}

CA SendToServer(CA ca)
{
    SOCKET cC;
    WSADATA wsaData;

    WSAStartup(MAKEWORD(2, 0), &wsaData);
    cC = socket(AF_INET, SOCK_DGRAM, NULL);

    SOCKADDR_IN serv;
    serv.sin_family = AF_INET;
    serv.sin_port = htons(2000);
    serv.sin_addr.s_addr = inet_addr(ca.ipaddr);

    SOCKADDR_IN clnt;
    int lc = sizeof(clnt);

    sendto(cC, (char*)&ca, sizeof(ca), NULL, (sockaddr*)&serv, sizeof(serv));
    recvfrom(cC, (char*)&ca, sizeof(ca), NULL, (sockaddr*)&clnt, &lc);

    closesocket(cC);
    WSACleanup();

    return ca;
}

CA InitCA(char ipaddr[15], char resurce[20], int number)
{
    CA ca;
    strcpy(ca.ipaddr, ipaddr);
    strcpy(ca.resurce, resurce);
    ca.clientnumber = number;
    ca.status = CA::INIT;

    ca = SendToServer(ca);

    cout << "INIT: " << GetStatusText(ca.status) << endl;
    return ca;
}

void EnterCA(CA& ca)
{
    do {
        ca.status = CA::ENTER;
        ca = SendToServer(ca);

        cout << "ENTER TRY: " << GetStatusText(ca.status) << endl;

        Sleep(2000);
    } while (ca.status != CA::ENTER);
}

void LeaveCA(CA& ca)
{
    ca.status = CA::LEAVE;
    ca = SendToServer(ca);

    cout << "LEAVE: " << GetStatusText(ca.status) << endl;
}

void CloseCA(CA& ca)
{
    ca.status = CA::NOINIT;
    ca = SendToServer(ca);

    cout << "CLOSE: " << GetStatusText(ca.status) << endl;
}

int _tmain(int argc, _TCHAR* argv[])
{
    CA s = InitCA((char*)"10.209.80.52", (char*)"Z:\\file.txt", 1);

    while (true)
    {
        int k;
        cout << "1-write 2-read 3-exit\n";
        cin >> k;

        if (k == 1)
        {
            EnterCA(s);

            ofstream file("Z:\\file.txt", ios::app);

            for (int i = 0; i < 5; i++)
            {
                time_t t = time(0);
                file << "Client " << s.clientnumber << " " << ctime(&t);
                Sleep(1000);
            }

            file.close();

            LeaveCA(s);
        }
        else if (k == 2)
        {
            ifstream file("Z:\\file.txt");
            string line;

            while (getline(file, line))
                cout << line << endl;

            file.close();
        }
        else if (k == 3)
        {
            break;
        }
    }

    CloseCA(s);
    return 0;
}