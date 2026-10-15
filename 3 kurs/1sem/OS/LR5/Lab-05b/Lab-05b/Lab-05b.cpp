#include <windows.h>
#include <iostream>
#include <string>
#include <bitset>

DWORD PriorityFromInt(int val) {
    switch (val) {
    case 1: return IDLE_PRIORITY_CLASS;
    case 2: return BELOW_NORMAL_PRIORITY_CLASS;
    case 3: return NORMAL_PRIORITY_CLASS;
    case 4: return ABOVE_NORMAL_PRIORITY_CLASS;
    case 5: return HIGH_PRIORITY_CLASS;
    case 6: return REALTIME_PRIORITY_CLASS;
    default: return NORMAL_PRIORITY_CLASS;
    }
}

std::wstring PriorityClassToStr(DWORD cls) {
    switch (cls) {
    case IDLE_PRIORITY_CLASS: return L"IDLE_PRIORITY_CLASS";
    case BELOW_NORMAL_PRIORITY_CLASS: return L"BELOW_NORMAL_PRIORITY_CLASS";
    case NORMAL_PRIORITY_CLASS: return L"NORMAL_PRIORITY_CLASS";
    case ABOVE_NORMAL_PRIORITY_CLASS: return L"ABOVE_NORMAL_PRIORITY_CLASS";
    case HIGH_PRIORITY_CLASS: return L"HIGH_PRIORITY_CLASS";
    case REALTIME_PRIORITY_CLASS: return L"REALTIME_PRIORITY_CLASS";
    default: return L"UNKNOWN_PRIORITY_CLASS";
    }
}

std::wstring AffinityMaskToBinary(ULONG_PTR mask) {
    std::bitset<8> bits(static_cast<unsigned long long>(mask & 0xFF));
    return std::wstring(bits.to_string().begin(), bits.to_string().end());
}

int wmain(int argc, wchar_t* argv[])
{
    setlocale(LC_ALL, "Russian");

    if (argc != 4) {
        std::wcout << L"Использование: Lab-05b.exe <Mask> <Priority1> <Priority2>\n";
        return 0;
    }

    ULONG_PTR affinityMask = std::stoull(argv[1]);
    int pr1 = std::stoi(argv[2]);
    int pr2 = std::stoi(argv[3]);

    DWORD PriorityClass1 = PriorityFromInt(pr1);
    DWORD PriorityClass2 = PriorityFromInt(pr2);

    std::wcout << L"Полученные параметры:\n";
    std::wcout << L"Маска процессоров: " << AffinityMaskToBinary(affinityMask) << L"\n";
    std::wcout << L"Приоритет процесса 1: " << PriorityClassToStr(PriorityClass1) << L"\n";
    std::wcout << L"Приоритет процесса 2: " << PriorityClassToStr(PriorityClass2) << L"\n\n";

    // Получаем ПОЛНЫЙ путь к дочернему EXE
    wchar_t exePath[MAX_PATH];
    DWORD len = GetFullPathNameW(L"Lab-05x.exe", MAX_PATH, exePath, NULL);

    if (len == 0 || len >= MAX_PATH) {
        std::wcerr << L"Ошибка: не удалось получить путь к Lab-05x.exe\n";
        return 1;
    }

    // Командная строка должна быть модифицируемой!
    wchar_t cmdLine1[MAX_PATH];
    wchar_t cmdLine2[MAX_PATH];

    wcscpy_s(cmdLine1, exePath);
    wcscpy_s(cmdLine2, exePath);

    STARTUPINFOW si1 = { sizeof(si1) };
    PROCESS_INFORMATION pi1 = {};

    STARTUPINFOW si2 = { sizeof(si2) };
    PROCESS_INFORMATION pi2 = {};

    // --- ПРОЦЕСС 1 ---
    if (!CreateProcessW(
        NULL,
        cmdLine1,            // имя EXE в командной строке
        NULL, NULL, FALSE,
        CREATE_NEW_CONSOLE,  // отдельное окно
        NULL, NULL,
        &si1, &pi1))
    {
        std::wcerr << L"Не удалось запустить процесс 1. Ошибка: " << GetLastError() << L"\n";
        return 1;
    }

    SetProcessAffinityMask(pi1.hProcess, affinityMask);
    SetPriorityClass(pi1.hProcess, PriorityClass1);

    // --- ПРОЦЕСС 2 ---
    if (!CreateProcessW(
        NULL,
        cmdLine2,
        NULL, NULL, FALSE,
        CREATE_NEW_CONSOLE,
        NULL, NULL,
        &si2, &pi2))
    {
        std::wcerr << L"Не удалось запустить процесс 2. Ошибка: " << GetLastError() << L"\n";
        return 1;
    }

    SetProcessAffinityMask(pi2.hProcess, affinityMask);
    SetPriorityClass(pi2.hProcess, PriorityClass2);

    std::wcout << L"Дочерние процессы запущены.\n";

    WaitForSingleObject(pi1.hProcess, INFINITE);
    WaitForSingleObject(pi2.hProcess, INFINITE);

    std::wcout << L"Оба дочерних процесса завершены.\n";

    CloseHandle(pi1.hProcess);
    CloseHandle(pi1.hThread);
    CloseHandle(pi2.hProcess);
    CloseHandle(pi2.hThread);

    return 0;
}
