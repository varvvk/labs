#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <stdio.h>
#include <WinSock2.h>
#pragma comment(lib, "WS2_32.lib")

static void printErrorMessage(const char* message)
{
	printf("%s: %d\n", message, WSAGetLastError());
}

int main(int argc, char* argv[])
{
	if (argc < 2) {
		printf("Usage: %s <AGENT_IP>\n", argv[0]);
		return -1;
	}

	WSAData wsadata;
	if (WSAStartup(MAKEWORD(2, 2), &wsadata) != 0)
	{
		printErrorMessage("WSAStartup error");
		return -1;
	}

	SOCKET sS = socket(AF_INET, SOCK_DGRAM, 0);
	if (sS == INVALID_SOCKET)
	{
		printErrorMessage("Socket creation error");
		return -1;
	}

	struct sockaddr_in caddr = { 0 };
	struct sockaddr_in saddr = { 0 };
	saddr.sin_family = AF_INET;
	saddr.sin_port = htons(5555);
	saddr.sin_addr.s_addr = inet_addr(argv[1]);

	int ssize = sizeof(saddr);
	if (bind(sS, (struct sockaddr*)&saddr, ssize) == SOCKET_ERROR)
	{
		printErrorMessage("Socket bind error (check if IP exists in OS)");
		closesocket(sS);
		WSACleanup();
		return -1;
	}

	printf("[Agent] Running on %s:5555\n", argv[1]);

	int running = 1;
	char recvbuffer[256];
	char coordinator[256];

	while (running)
	{
		int csize = sizeof(caddr);
		struct sockaddr_in coordaddr = { 0 };
		coordaddr.sin_family = AF_INET;
		coordaddr.sin_port = htons(5555);
		int coordsize = sizeof(coordaddr);

		int recvResult = recvfrom(sS, recvbuffer, sizeof(recvbuffer) - 1, 0, (struct sockaddr*)&caddr, &csize);
		if (recvResult == SOCKET_ERROR)
		{
			printErrorMessage("Client request receive error");
			continue;
		}

		recvbuffer[recvResult] = '\0';

		if (strcmp(recvbuffer, "gettime") == 0)
		{
			FILE* file = fopen("config.txt", "r");
			if (file)
			{
				size_t count = fread(coordinator, sizeof(char), sizeof(coordinator) - 1, file);
				coordinator[count] = '\0';
				fclose(file);

				coordaddr.sin_addr.s_addr = inet_addr(coordinator);

				sendto(sS, "gettime", 7, 0, (struct sockaddr*)&coordaddr, sizeof(coordaddr));

				int brrecvResult = recvfrom(sS, recvbuffer, sizeof(recvbuffer) - 1, 0, (struct sockaddr*)&coordaddr, &coordsize);
				if (brrecvResult != SOCKET_ERROR)
				{
					recvbuffer[brrecvResult] = '\0';

					printf("[LOG] Client: %s -> Coordinator: %s\n", inet_ntoa(caddr.sin_addr), coordinator);

					sendto(sS, recvbuffer, brrecvResult, 0, (struct sockaddr*)&caddr, csize);
				}
				else {
					printErrorMessage("Coordinator response timeout/error");
				}
			}
			else {
				printf("[Error] config.txt not found! No coordinator known.\n");
			}
		}
	}

	closesocket(sS);
	WSACleanup();
	return 0;
}