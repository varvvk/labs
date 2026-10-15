#include <windows.h>
#include <stdio.h>
#include <string>

void Lab_04x(int iterations) {
    std::string username = "Userbc";

    DWORD pid = GetCurrentProcessId();
    DWORD tid = GetCurrentThreadId();

    for (int i = 1; i <= iterations; ++i) {
        char ch = username[(i - 1) % username.size()];
        printf("%u - %u - %d - %c\n", pid, tid, i, ch);
        fflush(stdout);
        Sleep(400);
    }
}

DWORD WINAPI ThreadFunc(LPVOID lpParam) {
    int iterations = *(int*)lpParam;
    Lab_04x(iterations);
    return 0;
}

int main() {
    int iter1 = 50;
    int iter2 = 125;
    int mainIter = 100;

    HANDLE hThread1 = CreateThread(NULL, 0, ThreadFunc, &iter1, 0, NULL);
    HANDLE hThread2 = CreateThread(NULL, 0, ThreadFunc, &iter2, 0, NULL);

    if (!hThread1 || !hThread2) {
        printf("Failed to create threads\n");
        return 1;
    }

    std::string username = "Userbc";
    DWORD pid = GetCurrentProcessId();
    DWORD tid = GetCurrentThreadId();

    for (int i = 1; i <= mainIter; ++i) {
        if (i == 40) {
            TerminateThread(hThread2, 0);
            printf("Thread 2 terminated at iteration %d\n", i);
        }

        char ch = username[(i - 1) % username.size()];
        printf("%u - %u - %d - %c\n", pid, tid, i, ch);
        fflush(stdout);
        Sleep(400);
    }

    // Ждём оставшийся поток
    WaitForSingleObject(hThread1, INFINITE);

    CloseHandle(hThread1);
    CloseHandle(hThread2); // дескриптор второго потока может уже быть недействительным после TerminateThread

    return 0;
}
