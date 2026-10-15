#include <windows.h>
#include <tlhelp32.h>
#include <stdio.h>

int main() {
    DWORD currentPID = GetCurrentProcessId();
    printf("Current Process ID: %u\n\n", currentPID);
    printf("Threads running in this process:\n");

    HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
    if (hSnapshot == INVALID_HANDLE_VALUE) {
        printf("Failed to create snapshot!\n");
        return 1;
    }

    THREADENTRY32 te;
    te.dwSize = sizeof(THREADENTRY32);

    if (Thread32First(hSnapshot, &te)) {
        do {
            if (te.th32OwnerProcessID == currentPID) {
                printf("   Thread ID: %u\n", te.th32ThreadID);
            }
        } while (Thread32Next(hSnapshot, &te));
    }

    CloseHandle(hSnapshot);

    printf("\nThe program will exit in 5 seconds...\n");
    Sleep(5000);      // ← задержка 5 секунд

    return 0;
}
