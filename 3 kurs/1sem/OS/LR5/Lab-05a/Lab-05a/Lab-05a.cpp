// Lab05a.cpp
#include <windows.h>
#include <iostream>
#include <bitset>
#include <string>

std::string ProcPriorityClassToStr(DWORD cls) {
    switch (cls) {
    case IDLE_PRIORITY_CLASS:        return "IDLE_PRIORITY_CLASS";
    case BELOW_NORMAL_PRIORITY_CLASS:return "BELOW_NORMAL_PRIORITY_CLASS";
    case NORMAL_PRIORITY_CLASS:      return "NORMAL_PRIORITY_CLASS";
    case ABOVE_NORMAL_PRIORITY_CLASS:return "ABOVE_NORMAL_PRIORITY_CLASS";
    case HIGH_PRIORITY_CLASS:        return "HIGH_PRIORITY_CLASS";
    case REALTIME_PRIORITY_CLASS:    return "REALTIME_PRIORITY_CLASS";
    default:                         return "UNKNOWN";
    }
}

std::string ThreadPriorityToStr(int pr) {
    switch (pr) {
    case THREAD_PRIORITY_ERROR_RETURN: return "THREAD_PRIORITY_ERROR_RETURN";
    case THREAD_PRIORITY_IDLE:         return "THREAD_PRIORITY_IDLE";
    case THREAD_PRIORITY_LOWEST:       return "THREAD_PRIORITY_LOWEST";
    case THREAD_PRIORITY_BELOW_NORMAL: return "THREAD_PRIORITY_BELOW_NORMAL";
    case THREAD_PRIORITY_NORMAL:       return "THREAD_PRIORITY_NORMAL";
    case THREAD_PRIORITY_ABOVE_NORMAL: return "THREAD_PRIORITY_ABOVE_NORMAL";
    case THREAD_PRIORITY_HIGHEST:      return "THREAD_PRIORITY_HIGHEST";
    case THREAD_PRIORITY_TIME_CRITICAL:return "THREAD_PRIORITY_TIME_CRITICAL";
    default:                           return "UNKNOWN";
    }
}

// Helper: print affinity mask in binary
void PrintAffinityMaskBinary16(ULONG_PTR mask) {
    const size_t bits = 8;  // фиксированная ширина
    std::bitset<bits> b(static_cast<unsigned long long>(mask));
    std::cout << b.to_string();
}

int main() {
    setlocale(LC_ALL, "Russian");

    DWORD pid = GetCurrentProcessId();
    DWORD tid = GetCurrentThreadId();

    HANDLE hProc = GetCurrentProcess();
    HANDLE hThread = GetCurrentThread();

    DWORD procClass = GetPriorityClass(hProc);
    int threadPr = GetThreadPriority(hThread);

    ULONG_PTR procMask;
    ULONG_PTR sysMask;
    BOOL ok = GetProcessAffinityMask(hProc, &procMask, &sysMask);
    if (!ok) {
        std::cerr << "GetProcessAffinityMask failed, error = " << GetLastError() << "\n";
    }

    // Количество процессоров доступных для выполнения процессу: считаем единицы в procMask
    const size_t MASK_BITS = 8;

    std::bitset<MASK_BITS> procBits16(static_cast<unsigned long long>(procMask));
    size_t availableProcessors = procBits16.count();

    // Номер процессора, назначенного текущему потоку
    DWORD currentProcessor = GetCurrentProcessorNumber(); // работает на Win7+

    // Вывод
    std::cout << "Идентификатор текущего процесса (PID): " << pid << "\n";
    std::cout << "Идентификатор текущего потока  (TID): " << tid << "\n\n";

    std::cout << "Класс приоритетов текущего процесса: ";
    if (procClass != 0) std::cout << ProcPriorityClassToStr(procClass) << " (" << procClass << ")\n";
    else std::cout << "не удалось определить (GetPriorityClass вернул 0)\n";

    std::cout << "Приоритет текущего потока: " << ThreadPriorityToStr(threadPr)
        << " (" << threadPr << ")\n\n";

    std::cout << "Маска родственности процесса:\n";
    PrintAffinityMaskBinary16(procMask);
    std::cout << "\n\n";

    std::cout << "Системная маска родственности:\n";
    PrintAffinityMaskBinary16(sysMask);
    std::cout << "\n\n";

    std::cout << "Количество процессоров, доступных для выполнения процессу: "
        << availableProcessors << "\n";

    std::cout << "Номер процессора, назначенного текущему потоку: "
        << currentProcessor << "\n";

    return 0;
}
