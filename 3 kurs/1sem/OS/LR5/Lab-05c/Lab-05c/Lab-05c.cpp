#include <windows.h>
#include <iostream>
#include <string>
#include <thread>
#include <chrono>
#include <conio.h>
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
    default: return L"UNKNOWN";
    }
}

std::wstring ThreadPriorityToStr(int pr) {
    switch (pr) {
    case THREAD_PRIORITY_IDLE: return L"THREAD_PRIORITY_IDLE";
    case THREAD_PRIORITY_LOWEST: return L"THREAD_PRIORITY_LOWEST";
    case THREAD_PRIORITY_BELOW_NORMAL: return L"THREAD_PRIORITY_BELOW_NORMAL";
    case THREAD_PRIORITY_NORMAL: return L"THREAD_PRIORITY_NORMAL";
    case THREAD_PRIORITY_ABOVE_NORMAL: return L"THREAD_PRIORITY_ABOVE_NORMAL";
    case THREAD_PRIORITY_HIGHEST: return L"THREAD_PRIORITY_HIGHEST";
    case THREAD_PRIORITY_TIME_CRITICAL: return L"THREAD_PRIORITY_TIME_CRITICAL";
    default: return L"UNKNOWN";
    }
}

std::wstring AffinityMaskToBinary(ULONG_PTR mask) {
    std::bitset<8> bits(static_cast<unsigned long long>(mask & 0xFF));
    return std::wstring(bits.to_string().begin(), bits.to_string().end());
}

// Функция потока с замером времени
void ThreadFunction(int threadNum, int priority, long long& execTimeMs) {
    auto start = std::chrono::high_resolution_clock::now();

    DWORD tid = GetCurrentThreadId();
    DWORD pid = GetCurrentProcessId();
    SetThreadPriority(GetCurrentThread(), priority);

    DWORD procClass = GetPriorityClass(GetCurrentProcess());

    const int totalIterations = 1000000;
    for (int i = 1; i <= totalIterations; ++i) {
        if (i % 1000 == 0) {
            DWORD cpuNumber = GetCurrentProcessorNumber();
            std::wcout << L"[Поток " << threadNum << L"] Итерация: " << i
                << L", PID: " << pid
                << L", TID: " << tid
                << L", Класс процесса: " << PriorityClassToStr(procClass)
                << L", Приоритет потока: " << ThreadPriorityToStr(priority)
                << L", CPU: " << cpuNumber << std::endl;
            std::this_thread::sleep_for(std::chrono::milliseconds(200));
        }
    }

    auto end = std::chrono::high_resolution_clock::now();
    execTimeMs = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();
}

int wmain(int argc, wchar_t* argv[]) {
    setlocale(LC_ALL, "Russian");

    if (argc != 5) {
        std::wcout << L"Использование: Lab-05c.exe <Mask> <ProcPriority> <Thread1Priority> <Thread2Priority>\n";
        return 0;
    }

    ULONG_PTR affinityMask = std::stoull(argv[1]);
    int procPriority = std::stoi(argv[2]);
    int thread1Priority = std::stoi(argv[3]);
    int thread2Priority = std::stoi(argv[4]);

    SetProcessAffinityMask(GetCurrentProcess(), affinityMask);
    SetPriorityClass(GetCurrentProcess(), PriorityFromInt(procPriority));

    std::wcout << L"Маска процессоров: " << AffinityMaskToBinary(affinityMask) << L"\n";
    std::wcout << L"Приоритет процесса: " << PriorityClassToStr(PriorityFromInt(procPriority)) << L"\n";
    std::wcout << L"Приоритет потока 1: " << ThreadPriorityToStr(thread1Priority) << L"\n";
    std::wcout << L"Приоритет потока 2: " << ThreadPriorityToStr(thread2Priority) << L"\n\n";

    // Время выполнения каждого потока
    long long time1 = 0, time2 = 0;

    auto start = std::chrono::high_resolution_clock::now();

    std::thread t1(ThreadFunction, 1, thread1Priority, std::ref(time1));
    std::thread t2(ThreadFunction, 2, thread2Priority, std::ref(time2));

    t1.join();
    t2.join();

    auto end = std::chrono::high_resolution_clock::now();
    auto elapsed_ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::wcout << L"\nВремя выполнения потока 1: " << time1 << L" мс\n";
    std::wcout << L"Время выполнения потока 2: " << time2 << L" мс\n";
    std::wcout << L"Общее время выполнения: " << elapsed_ms << L" мс\n";

    std::wcout << L"\nНажмите любую клавишу, чтобы закрыть окно...\n";
    _getwch();

    return 0;
}
