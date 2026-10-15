#include <windows.h>

void Print(const char* txt)
{
    DWORD written;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE),
        txt, lstrlenA(txt), &written, NULL);
}

void RunProcess(const char* name)
{
    const char userStr[] = "Userbc";
    const int userLen = 6;

    char buffer[128];

    // Открываем событие
    HANDLE hEvent = OpenEventA(EVENT_ALL_ACCESS, FALSE, "Global\\Lab06dEvent");
    if (!hEvent)
    {
        Print("Cannot open event\n");
        return;
    }

    // Дочерние процессы должны ждать до 16-й итерации
    if (strcmp(name, "MAIN") != 0)
    {
        // Ждём команду от родителя
        WaitForSingleObject(hEvent, INFINITE);
    }

    for (int i = 1; i <= 90; i++)
    {
        char letter = userStr[(i - 1) % userLen];

        wsprintfA(buffer,
            "Process %s | iteration %02d | %c\n",
            name, i, letter);

        Print(buffer);

        Sleep(100);
    }
}

void RunMain()
{
    Print("Main starting...\n");

    // -------------------------
    // Создаём событие (на старте сброшено)
    // -------------------------
    HANDLE hEvent = CreateEventA(
        NULL,
        TRUE,     // manual reset event
        FALSE,    // initially nonsignaled
        "Global\\Lab06dEvent"
    );

    if (!hEvent)
    {
        Print("Cannot create event\n");
        return;
    }

    STARTUPINFOA si;
    PROCESS_INFORMATION piA, piB;
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);

    // -------------------------
    // Запускаем процесс A
    // -------------------------
    if (!CreateProcessA(
        NULL,
        (LPSTR)"Lab-06d.exe A",
        NULL, NULL, FALSE,
        CREATE_NEW_CONSOLE,
        NULL, NULL,
        &si, &piA))
    {
        Print("Cannot start process A\n");
        return;
    }

    // -------------------------
    // Запускаем процесс B
    // -------------------------
    if (!CreateProcessA(
        NULL,
        (LPSTR)"Lab-06d.exe B",
        NULL, NULL, FALSE,
        CREATE_NEW_CONSOLE,
        NULL, NULL,
        &si, &piB))
    {
        Print("Cannot start process B\n");
        return;
    }

    // -------------------------
    // Основной процесс выполняет итерации 1–15 ТОЛЬКО ОН
    // -------------------------
    const char userStr[] = "Userbc";
    const int userLen = 6;
    char buffer[128];

    for (int i = 1; i <= 15; i++)
    {
        char letter = userStr[(i - 1) % userLen];

        wsprintfA(buffer,
            "Process MAIN | iteration %02d | %c\n",
            i, letter);

        Print(buffer);

        Sleep(100);
    }

    // -------------------------
    // На 16-й итерации → событие в сигнал
    // -------------------------
    Print("MAIN → releasing children...\n");
    SetEvent(hEvent);

    // -------------------------
    // Теперь MAIN продолжает 16–90
    // -------------------------
    for (int i = 16; i <= 90; i++)
    {
        char letter = userStr[(i - 1) % userLen];

        wsprintfA(buffer,
            "Process MAIN | iteration %02d | %c\n",
            i, letter);

        Print(buffer);

        Sleep(100);
    }

    // Ждём A и B
    WaitForSingleObject(piA.hProcess, INFINITE);
    WaitForSingleObject(piB.hProcess, INFINITE);

    CloseHandle(piA.hProcess);
    CloseHandle(piA.hThread);
    CloseHandle(piB.hProcess);
    CloseHandle(piB.hThread);

    CloseHandle(hEvent);
}

int main(int argc, char* argv[])
{
    if (argc == 1)
    {
        RunMain();
    }
    else
    {
        RunProcess(argv[1]); // A или B
    }

    return 0;
}
