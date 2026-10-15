#include <windows.h>

void WriteString(const char* str) {
    DWORD written;
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    WriteConsoleA(hConsole, str, lstrlenA(str), &written, nullptr);
}

// Функция для конвертации числа в строку с ведущими нулями
void IntToStr2(int value, char* buffer) {
    buffer[0] = '0' + value / 10;
    buffer[1] = '0' + value % 10;
    buffer[2] = 0;
}

void IntToStr4(int value, char* buffer) {
    buffer[0] = '0' + (value / 1000) % 10;
    buffer[1] = '0' + (value / 100) % 10;
    buffer[2] = '0' + (value / 10) % 10;
    buffer[3] = '0' + value % 10;
    buffer[4] = 0;
}

int main() {
    SYSTEMTIME utcTime, localTime;
    GetSystemTime(&utcTime);      // UTC
    GetLocalTime(&localTime);     // Локальное время

    // Вычисляем смещение в часах
    int offsetHours = localTime.wHour - utcTime.wHour;
    if (offsetHours > 12) offsetHours -= 24;
    if (offsetHours < -12) offsetHours += 24;

    char sign = (offsetHours >= 0) ? '+' : '-';
    int absOffset = (offsetHours >= 0) ? offsetHours : -offsetHours;

    // Буферы для чисел
    char year[5], month[3], day[3], hour[3], min[3], sec[3], offset[3];
    IntToStr4(localTime.wYear, year);
    IntToStr2(localTime.wMonth, month);
    IntToStr2(localTime.wDay, day);
    IntToStr2(localTime.wHour, hour);
    IntToStr2(localTime.wMinute, min);
    IntToStr2(localTime.wSecond, sec);
    IntToStr2(absOffset, offset);

    // Формируем строку
    char output[64];
    int pos = 0;
    const char* parts[] = { year, "-", month, "-", day, "T", hour, ":", min, ":", sec, &sign, offset, "\n" };
    for (int i = 0; i < 14; ++i) {
        const char* p = parts[i];
        while (*p) output[pos++] = *p++;
    }
    output[pos] = 0;

    // Вывод
    WriteString(output);

    return 0;
}
