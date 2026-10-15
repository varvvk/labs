#include <windows.h>

/* ---------------- ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ ---------------- */

int g_init = 10;                 // глобальная инициализированная
int g_uninit;                    // глобальная неинициализированная
static int g_static_init = 20;   // глобальная static инициализированная
static int g_static_uninit;      // глобальная static неинициализированная

/* ---------------- ПРОИЗВОЛЬНАЯ ФУНКЦИЯ ---------------- */

void MyFunction()
{
}

/* ---------------- ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ---------------- */

void PrintText(const char* text)
{
    DWORD written;
    WriteConsoleA(
        GetStdHandle(STD_OUTPUT_HANDLE),
        text,
        lstrlenA(text),
        &written,
        NULL
    );
}

void PrintHex(const char* name, void* addr)
{
    char buffer[128];
    wsprintfA(buffer, "%s: 0x%p\r\n", name, addr);
    PrintText(buffer);
}

void PrintDec(const char* name, DWORD value)
{
    char buffer[128];
    wsprintfA(buffer, "%s: %lu\r\n", name, value);
    PrintText(buffer);
}

/* ---------------- MAIN ---------------- */

int main(int argc, char* argv[])
{
    /* ---------- ЛОКАЛЬНЫЕ ПЕРЕМЕННЫЕ ---------- */

    int l_init = 1;
    int l_uninit;

    static int l_static_init = 2;
    static int l_static_uninit;

    /* ---------- PID ---------- */

    DWORD pid = GetCurrentProcessId();

    PrintText("=== PROCESS INFO ===\r\n");
    PrintDec("PID", pid);

    PrintText("\r\n=== ADDRESSES ===\r\n\r\n");

    /* функции */
    PrintHex("main()", (void*)main);
    PrintHex("MyFunction()", (void*)MyFunction);

    PrintText("\r\n-- Global variables --\r\n");
    PrintHex("g_init", &g_init);
    PrintHex("g_uninit", &g_uninit);
    PrintHex("g_static_init", &g_static_init);
    PrintHex("g_static_uninit", &g_static_uninit);

    PrintText("\r\n-- Local variables --\r\n");
    PrintHex("l_init", &l_init);
    PrintHex("l_uninit", &l_uninit);
    PrintHex("l_static_init", &l_static_init);
    PrintHex("l_static_uninit", &l_static_uninit);

    PrintText("\r\n-- argc / argv --\r\n");
    PrintHex("argc", &argc);
    PrintHex("argv", &argv);

    PrintText("\r\nPress any key to exit...\r\n");

    /* ожидание ввода */
    char ch;
    DWORD read;
    ReadConsoleA(
        GetStdHandle(STD_INPUT_HANDLE),
        &ch,
        1,
        &read,
        NULL
    );

    return 0;
}
