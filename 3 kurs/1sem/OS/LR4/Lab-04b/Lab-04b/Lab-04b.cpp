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
    int iter1 = 50;   // для первого потока
    int iter2 = 125;  // для второго потока
    int mainIter = 100; // для главного потока

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
        // Управление потоками
        if (i == 20) SuspendThread(hThread1);
        if (i == 60) ResumeThread(hThread1);
        if (i == 40) SuspendThread(hThread2);
        if (i == mainIter) ResumeThread(hThread2);

        char ch = username[(i - 1) % username.size()];
        printf("%u - %u - %d - %c\n", pid, tid, i, ch);
        fflush(stdout);
        Sleep(400);
    }

    HANDLE threads[2] = { hThread1, hThread2 };
    WaitForMultipleObjects(2, threads, TRUE, INFINITE);

    CloseHandle(hThread1);
    CloseHandle(hThread2);

    return 0;
}
