// Lab-04a.cpp
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

// Обёртка для потока
DWORD WINAPI ThreadFunc(LPVOID lpParam) {
    int iterations = *(int*)lpParam;
    Lab_04x(iterations);
    return 0;
}

int main() {
    int iter1 = 50;   // для первого потока
    int iter2 = 125;  // для второго потока
    int mainIter = 100; // для главного потока

    // Создание потоков
    HANDLE hThread1 = CreateThread(NULL, 0, ThreadFunc, &iter1, 0, NULL);
    if (hThread1 == NULL) {
        printf("Failed to create thread 1\n");
        return 1;
    }

    HANDLE hThread2 = CreateThread(NULL, 0, ThreadFunc, &iter2, 0, NULL);
    if (hThread2 == NULL) {
        printf("Failed to create thread 2\n");
        return 1;
    }

    // Главный поток также выполняет Lab_04x
    Lab_04x(mainIter);

    // Ожидание завершения дочерних потоков
    HANDLE threads[2] = { hThread1, hThread2 };
    WaitForMultipleObjects(2, threads, TRUE, INFINITE);

    // Закрытие дескрипторов потоков
    CloseHandle(hThread1);
    CloseHandle(hThread2);

    return 0;
}
