#include <windows.h>

void WriteString(const char* str) {
    DWORD written;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    WriteConsoleA(hConsole, str, lstrlenA(str), &written, nullptr);
}

// Функция конвертирует число в строку
void UIntToStr(unsigned long long value, char* buffer) {
    char temp[32];
    int pos = 0;
    if (value == 0) {
        buffer[0] = '0';
        buffer[1] = 0;
        return;
    }
    while (value > 0) {
        temp[pos++] = '0' + (value % 10);
        value /= 10;
    }
    for (int i = 0; i < pos; ++i) {
        buffer[i] = temp[pos - 1 - i];
    }
    buffer[pos] = 0;
}

void PrintCounter(const char* prefix, unsigned long long count) {
    char buffer[64];
    UIntToStr(count, buffer);
    char output[128];
    int pos = 0;
    const char* parts[] = { prefix, buffer, "\n" };
    for (int i = 0; i < 3; ++i) {
        const char* p = parts[i];
        while (*p) output[pos++] = *p++;
    }
    output[pos] = 0;
    WriteString(output);
}

int main() {
    unsigned long long counter = 0;

    // Создаем периодический таймер ожидания
    HANDLE hTimer = CreateWaitableTimer(NULL, FALSE, NULL); // FALSE = автоматический сброс
    if (!hTimer) return 1;

    LARGE_INTEGER liDueTime;
    liDueTime.QuadPart = -30000000LL; // 3 секунды в 100-наносекундах, отрицательное = relative time

    // Устанавливаем периодический таймер каждые 3 секунды
    if (!SetWaitableTimer(hTimer, &liDueTime, 3000, NULL, NULL, FALSE)) {
        CloseHandle(hTimer);
        return 1;
    }

    int elapsedIntervals = 0;
    int totalIntervals = 5; // 5 интервалов по 3 секунды = 15 секунд

    while (elapsedIntervals < totalIntervals) {
        // Интенсивный подсчет до срабатывания таймера
        while (WaitForSingleObject(hTimer, 0) != WAIT_OBJECT_0) {
            counter++;
        }

        elapsedIntervals++;
        PrintCounter("Counter value: ", counter);
    }

    PrintCounter("Final counter: ", counter);

    CancelWaitableTimer(hTimer);
    CloseHandle(hTimer);

    return 0;
}
