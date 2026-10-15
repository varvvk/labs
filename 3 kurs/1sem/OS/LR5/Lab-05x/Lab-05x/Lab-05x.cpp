#include <windows.h>
#include <iostream>
#include <ctime>

int main() {
    setlocale(LC_ALL, "Russian");
    Sleep(100);

    // Стартовое время
    clock_t startTime = clock();

    DWORD pid = GetCurrentProcessId();
    DWORD tid = GetCurrentThreadId();

    HANDLE hProc = GetCurrentProcess();
    HANDLE hThread = GetCurrentThread();

    // Главный цикл
    for (int i = 1; i <= 1'000'000; i++) {

        if (i % 1000 == 0) {

            DWORD procClass = GetPriorityClass(hProc);
            int threadPr = GetThreadPriority(hThread);
            DWORD cpuNumber = GetCurrentProcessorNumber();

            std::cout << "Итерация: " << i << "\n";
            std::cout << "PID: " << pid << "\n";
            std::cout << "TID: " << tid << "\n";
            std::cout << "Класс приоритета процесса: " << procClass << "\n";
            std::cout << "Приоритет потока: " << threadPr << "\n";
            std::cout << "Процессор, назначенный для потока: " << cpuNumber << "\n";
            std::cout << "---------------------------------------------\n";

            // Задержка 200 мс
            Sleep(200);
        }
    }

    // Время работы программы
    clock_t endTime = clock();
    double elapsedSeconds = double(endTime - startTime) / CLOCKS_PER_SEC;

    std::cout << "\nВремя выполнения программы: " << elapsedSeconds << " секунд\n";

    system("pause");

    return 0;
}
