// Lab-06a.cpp
#include <windows.h>

CRITICAL_SECTION cs;

const char userStr[] = "Userbc";
const int userLen = 6;

struct ThreadInfo {
    const char* name;
};

void Print(const char* text) {
    DWORD written;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE), text, lstrlenA(text), &written, NULL);
}

DWORD WINAPI ThreadFunc(LPVOID param) {
    ThreadInfo* info = (ThreadInfo*)param;
    char buffer[128];

    for (int i = 1; i <= 90; i++) {
        if (i == 30) EnterCriticalSection(&cs);

        char letter = userStr[(i - 1) % userLen];
        wsprintfA(buffer, "Thread %s | iteration %02d | %c\n", info->name, i, letter);
        Print(buffer);

        if (i == 60) LeaveCriticalSection(&cs);

        Sleep(100);
    }

    return 0;
}

int main() {
    InitializeCriticalSection(&cs);

    ThreadInfo a = { "A" };
    ThreadInfo b = { "B" };
    ThreadInfo mainInfo = { "MAIN" };

    HANDLE hA = CreateThread(NULL, 0, ThreadFunc, &a, 0, NULL);
    HANDLE hB = CreateThread(NULL, 0, ThreadFunc, &b, 0, NULL);

    ThreadFunc(&mainInfo);

    WaitForSingleObject(hA, INFINITE);
    WaitForSingleObject(hB, INFINITE);

    CloseHandle(hA);
    CloseHandle(hB);
    DeleteCriticalSection(&cs);

    return 0;
}
