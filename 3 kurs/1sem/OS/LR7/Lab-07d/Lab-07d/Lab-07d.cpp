#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
static DWORD my_wcslen(const WCHAR* s) {
    DWORD n = 0;
    while (s && s[n] != 0) n++;
    return n;
}

static void write_str(const WCHAR* str) {
    DWORD written;
    WriteConsoleW(GetStdHandle(STD_OUTPUT_HANDLE), str, my_wcslen(str), &written, NULL);
}

static void write_uint(DWORD value) {
    WCHAR buffer[32];
    int pos = 0;

    if (value == 0) {
        buffer[pos++] = L'0';
    }
    else {
        WCHAR temp[32];
        int tempPos = 0;
        while (value > 0) {
            temp[tempPos++] = (WCHAR)(L'0' + (value % 10));
            value /= 10;
        }
        for (int i = tempPos - 1; i >= 0; i--) {
            buffer[pos++] = temp[i];
        }
    }
    buffer[pos] = L'\0';
    write_str(buffer);
}

static void write_uint64(ULONGLONG value) {
    WCHAR buffer[32];
    int pos = 0;

    if (value == 0) {
        buffer[pos++] = L'0';
    }
    else {
        WCHAR temp[32];
        int tempPos = 0;
        while (value > 0) {
            temp[tempPos++] = (WCHAR)(L'0' + (value % 10));
            value /= 10;
        }
        for (int i = tempPos - 1; i >= 0; i--) {
            buffer[pos++] = temp[i];
        }
    }
    buffer[pos] = L'\0';
    write_str(buffer);
}

// Сборка командной строки с ОЧЕНЬ БОЛЬШИМ числом
static void build_cmd_line(WCHAR* out, ULONGLONG arg) {
    const WCHAR base_cmd[] = L"lab-07x.exe ";
    int pos = 0;

    // "lab-07x.exe "
    for (int i = 0; base_cmd[i]; i++) {
        out[pos++] = base_cmd[i];
    }

    // Добавляем очень большое число как строку
    WCHAR num_buf[32];
    int num_pos = 0;
    ULONGLONG temp = arg;

    if (temp == 0) {
        num_buf[num_pos++] = L'0';
    }
    else {
        while (temp > 0) {
            num_buf[num_pos++] = (WCHAR)(L'0' + (temp % 10));
            temp /= 10;
        }
    }

    // Записываем число в обратном порядке
    for (int i = num_pos - 1; i >= 0; i--) {
        out[pos++] = num_buf[i];
    }
    out[pos] = L'\0';
}

// Структура для передачи параметров в поток
typedef struct {
    HANDLE hProcess;
    DWORD timeout_ms;
    DWORD process_id;
    DWORD* pExitCode;
} PROCESS_MONITOR_PARAMS;

// Поток для мониторинга и завершения процесса
DWORD WINAPI MonitorProcessThread(LPVOID lpParam) {
    PROCESS_MONITOR_PARAMS* params = (PROCESS_MONITOR_PARAMS*)lpParam;

    write_str(L"PID ");
    write_uint(params->process_id);
    write_str(L": жду ");
    write_uint(params->timeout_ms / 1000);
    write_str(L" сек...\r\n");

    // Ждем завершения процесса с таймаутом
    DWORD wait_result = WaitForSingleObject(params->hProcess, params->timeout_ms);

    if (wait_result == WAIT_TIMEOUT) {
        // Время истекло - завершаем принудительно
        write_str(L"PID ");
        write_uint(params->process_id);
        write_str(L": время истекло! Завершаю...\r\n");
        TerminateProcess(params->hProcess, 9999); // Специальный код для принудительного завершения
        *(params->pExitCode) = 9999;
    }
    else if (wait_result == WAIT_OBJECT_0) {
        // Процесс завершился сам
        write_str(L"PID ");
        write_uint(params->process_id);
        write_str(L": завершился сам\r\n");
        GetExitCodeProcess(params->hProcess, params->pExitCode);
    }

    return 0;
}

// Функция для запуска процесса в новой консоли
static BOOL CreateProcessInNewConsole(LPWSTR cmdLine, PROCESS_INFORMATION* pi) {
    STARTUPINFOW si = { 0 };
    si.cb = sizeof(si);
    si.dwFlags = STARTF_USESHOWWINDOW;
    si.wShowWindow = SW_SHOWNORMAL;

    return CreateProcessW(
        NULL,
        cmdLine,
        NULL,
        NULL,
        FALSE,
        CREATE_NEW_CONSOLE,
        0,
        NULL,
        &si,
        pi
    );
}

int main() {

    // Требуемое время выполнения (в секундах и миллисекундах)
    const DWORD T1_SEC = 60;
    const DWORD T2_SEC = 120;
    const DWORD T1_MS = T1_SEC * 1000;
    const DWORD T2_MS = T2_SEC * 1000;


    const ULONGLONG HUGE_NUMBER_1 = 10000000;  // 10 миллионов
    const ULONGLONG HUGE_NUMBER_2 = 20000000;  // 20 миллионов

    // Формируем командные строки
    WCHAR cmd_line1[256], cmd_line2[256];
    build_cmd_line(cmd_line1, HUGE_NUMBER_1);
    build_cmd_line(cmd_line2, HUGE_NUMBER_2);

    PROCESS_INFORMATION pi1 = { 0 }, pi2 = { 0 };

    // Запускаем первый процесс
    write_str(L"Запуск первого дочернего процесса:\r\n");

    if (!CreateProcessInNewConsole(cmd_line1, &pi1)) {
        write_str(L"Ошибка запуска первого процесса\r\n");
        return 1;
    }

    write_str(L"  PID первого процесса: ");
    write_uint(pi1.dwProcessId);
    write_str(L"\r\n\r\n");

    // Запускаем второй процесс
    write_str(L"Запуск второго дочернего процесса:\r\n");

    if (!CreateProcessInNewConsole(cmd_line2, &pi2)) {
        write_str(L"Ошибка запуска второго процесса\r\n");
        CloseHandle(pi1.hProcess);
        CloseHandle(pi1.hThread);
        return 1;
    }

    write_str(L"  PID второго процесса: ");
    write_uint(pi2.dwProcessId);
    write_str(L"\r\n\r\n");

    // Засекаем общее время ожидания
    FILETIME start_filetime;
    GetSystemTimeAsFileTime(&start_filetime);

    DWORD exit_code1 = 0, exit_code2 = 0;

    // Параметры для потоков-мониторов
    PROCESS_MONITOR_PARAMS params1 = { pi1.hProcess, T1_MS, pi1.dwProcessId, &exit_code1 };
    PROCESS_MONITOR_PARAMS params2 = { pi2.hProcess, T2_MS, pi2.dwProcessId, &exit_code2 };

    // Создаем потоки для параллельного мониторинга
    HANDLE threads[2];
    threads[0] = CreateThread(NULL, 0, MonitorProcessThread, &params1, 0, NULL);
    threads[1] = CreateThread(NULL, 0, MonitorProcessThread, &params2, 0, NULL);

    // Ждем завершения обоих мониторов
    WaitForMultipleObjects(2, threads, TRUE, INFINITE);

    // Закрываем хэндлы потоков
    CloseHandle(threads[0]);
    CloseHandle(threads[1]);

    // Получаем конечное время
    FILETIME end_filetime;
    GetSystemTimeAsFileTime(&end_filetime);

    // Вычисляем общее время ожидания
    ULARGE_INTEGER start_uli, end_uli;
    start_uli.LowPart = start_filetime.dwLowDateTime;
    start_uli.HighPart = start_filetime.dwHighDateTime;
    end_uli.LowPart = end_filetime.dwLowDateTime;
    end_uli.HighPart = end_filetime.dwHighDateTime;

    ULONGLONG wait_time_ms = (end_uli.QuadPart - start_uli.QuadPart) / 10000;

    // Вывод общего времени
    write_str(L"Общее время ожидания родительского процесса: ");
    write_uint((DWORD)(wait_time_ms / 1000));
    write_str(L".");

    DWORD ms_remainder = (DWORD)(wait_time_ms % 1000);
    if (ms_remainder < 10) write_str(L"00");
    else if (ms_remainder < 100) write_str(L"0");
    write_uint(ms_remainder);
    write_str(L" сек\r\n\r\n");


    // Закрываем хэндлы процессов
    CloseHandle(pi1.hProcess);
    CloseHandle(pi1.hThread);
    CloseHandle(pi2.hProcess);
    CloseHandle(pi2.hThread);

    return 0;
}
