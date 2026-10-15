// Lab-06c.cpp
#include <windows.h>

const char userStr[] = "Userbc";
const int userLen = 6;

void Print(const char* txt) {
    DWORD written;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE), txt, lstrlenA(txt), &written, NULL);
}

void RunProcess(const char* name) {
    char buffer[128];
    HANDLE hSem = OpenSemaphoreA(SEMAPHORE_ALL_ACCESS, FALSE, "Global\\Lab-06cSem");
    if (!hSem) { Print("Cannot open semaphore\n"); return; }

    for (int i = 1; i <= 90; i++) {
        if (i == 30) WaitForSingleObject(hSem, INFINITE);

        char letter = userStr[(i - 1) % userLen];
        wsprintfA(buffer, "Process %s | iteration %02d | %c\n", name, i, letter);
        Print(buffer);

        if (i == 60) ReleaseSemaphore(hSem, 1, NULL);

        Sleep(100);
    }
}

void RunMain() {
    Print("Main starting...\n");

    HANDLE hSem = CreateSemaphoreA(NULL, 1, 1, "Global\\Lab-06cSem");
    if (!hSem) { Print("Cannot create semaphore\n"); return; }

    STARTUPINFOA si; PROCESS_INFORMATION piA, piB;
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);

    CreateProcessA(NULL, (LPSTR)"Lab-06c.exe A", NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &piA);
    CreateProcessA(NULL, (LPSTR)"Lab-06c.exe B", NULL, NULL, FALSE, CREATE_NEW_CONSOLE, NULL, NULL, &si, &piB);

    RunProcess("MAIN");

    WaitForSingleObject(piA.hProcess, INFINITE);
    WaitForSingleObject(piB.hProcess, INFINITE);

    CloseHandle(piA.hProcess); CloseHandle(piA.hThread);
    CloseHandle(piB.hProcess); CloseHandle(piB.hThread);
    CloseHandle(hSem);
}

int main(int argc, char* argv[]) {
    if (argc == 1) RunMain();
    else RunProcess(argv[1]);
    return 0;
}
