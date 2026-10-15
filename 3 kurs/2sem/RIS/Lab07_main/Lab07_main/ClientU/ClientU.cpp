#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <stdio.h>
#include <WinSock2.h>
#pragma comment(lib, "WS2_32.lib")

static void printErrorMessage(const char* message)
{
	printf("%s: %d\n", message, WSAGetLastError());
}
int main()
{
	WSAData wsadata;
	if (WSAStartup(MAKEWORD(2, 2), &wsadata) != 0)
	{
		printErrorMessage("WSAStartup error");
		return -1;
	}
	SOCKET cS = socket(AF_INET, SOCK_DGRAM, 0);
	if (cS == INVALID_SOCKET)
	{
		printErrorMessage("Socket creation error");
		return -1;
	}
	struct sockaddr_in saddr = { 0 };
	saddr.sin_family = AF_INET;
	saddr.sin_port = htons(5555);
	saddr.sin_addr.s_addr = inet_addr("192.168.56.1");
	int ssize = sizeof(saddr);

	int running = 1;
	char buffer[] = "gettime";
	char recvbuffer[256];
	printf("[ClientU] All done. Starting request cycle...\n");
	DWORD timeout = 2000;
	setsockopt(cS, SOL_SOCKET, SO_RCVTIMEO, (const char*)&timeout, sizeof(timeout));
	while (running)
	{
		printf("[ClientU] Sending gettime request...\n");
		int sendResult = sendto(cS, buffer, sizeof(buffer), 0, (struct sockaddr*)&saddr, ssize);
		if (sendResult == SOCKET_ERROR)
		{
			printErrorMessage("Send error");
			return -1;
		}
		int recvResult = recvfrom(cS, recvbuffer, sizeof(recvbuffer) - 1, 0, (struct sockaddr*)&saddr, &ssize);
		if (recvResult == SOCKET_ERROR)
		{
			printErrorMessage("Recv error");
			return -1;
		}
		else
		{
			recvbuffer[recvResult] = '\0';
			printf("[ClientU] Request fullfilled: %s\n", recvbuffer);
		}
		Sleep(5000);
	}
	closesocket(cS);
	WSACleanup();
	return 0;
}