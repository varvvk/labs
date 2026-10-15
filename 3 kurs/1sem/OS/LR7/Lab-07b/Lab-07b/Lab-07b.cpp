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
    // Разворачиваем цифры
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
    DWORD startTime = GetTickCount();
    DWORD nextPrint1 = startTime + 5000;  // 5 секунд
    DWORD nextPrint2 = startTime + 10000; // 10 секунд
    DWORD endTime = startTime + 15000;    // 15 секунд

    while (true) {
        counter++;

        DWORD now = GetTickCount();

        if (now >= nextPrint1) {
            PrintCounter("Iterations after 5 seconds: ", counter);
            nextPrint1 = 0xFFFFFFFF; // Чтобы больше не печатать
        }
        if (now >= nextPrint2) {
            PrintCounter("Iterations after 10 seconds: ", counter);
            nextPrint2 = 0xFFFFFFFF; // Чтобы больше не печатать
        }
        if (now >= endTime) {
            PrintCounter("Final iterations: ", counter);
            break;
        }
    }

    return 0;
}
