#include <Windows.h>

#define MAX_FILE_SIZE 65536
#define INVALID_POS 0xFFFFFFFF

/* ================= Глобальные переменные ================= */

HANDLE g_hFile = INVALID_HANDLE_VALUE;
CHAR* g_Buffer = NULL;
DWORD  g_FileSize = 0;

/* ================= Консоль ================= */

void Print(const CHAR* s)
{
    DWORD w;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE),
        s, lstrlenA(s), &w, NULL);
}

void PrintLn(const CHAR* s)
{
    Print(s);
    Print("\r\n");
}

void ReadLine(CHAR* buf, DWORD max)
{
    DWORD r;
    ReadConsoleA(GetStdHandle(STD_INPUT_HANDLE),
        buf, max - 1, &r, NULL);

    if (r >= 2)
        buf[r - 2] = 0;
    else
        buf[0] = 0;
}

INT ReadInt()
{
    CHAR buf[32];
    ReadLine(buf, 32);

    INT sign = 1, val = 0, i = 0;

    if (buf[0] == '-')
    {
        sign = -1;
        i = 1;
    }

    for (; buf[i]; i++)
    {
        if (buf[i] < '0' || buf[i] > '9')
            return 0;
        val = val * 10 + (buf[i] - '0');
    }
    return val * sign;
}

/* ================= Файл ================= */

BOOL LoadFile()
{
    SetFilePointer(g_hFile, 0, NULL, FILE_BEGIN);
    ReadFile(g_hFile, g_Buffer, MAX_FILE_SIZE, &g_FileSize, NULL);
    return TRUE;
}

BOOL SaveFile()
{
    SetFilePointer(g_hFile, 0, NULL, FILE_BEGIN);
    WriteFile(g_hFile, g_Buffer, g_FileSize, &g_FileSize, NULL);
    SetEndOfFile(g_hFile);
    return TRUE;
}

/* ================= Строки ================= */

INT CountRows()
{
    if (g_FileSize == 0)
        return 0;

    INT rows = 0;
    DWORD i = 0;

    while (i < g_FileSize)
    {
        rows++;
        while (i < g_FileSize && g_Buffer[i] != '\n')
            i++;
        i++;
    }

    /* последняя пустая строка не считается */
    if (g_FileSize >= 2 &&
        g_Buffer[g_FileSize - 2] == '\r' &&
        g_Buffer[g_FileSize - 1] == '\n')
    {
        rows--;
    }

    return rows;
}

DWORD GetRowStart(INT row)
{
    if (row <= 1)
        return 0;

    INT current = 1;

    for (DWORD i = 0; i < g_FileSize; i++)
    {
        if (g_Buffer[i] == '\n')
        {
            current++;
            if (current == row)
                return i + 1;
        }
    }
    return INVALID_POS;
}

DWORD GetRowEnd(DWORD start)
{
    for (DWORD i = start; i < g_FileSize; i++)
        if (g_Buffer[i] == '\n')
            return i + 1;

    return g_FileSize;
}

DWORD GetInsertOffset(INT pos)
{
    if (pos <= 1)
        return 0;

    INT row = 1;

    for (DWORD i = 0; i < g_FileSize; i++)
    {
        if (g_Buffer[i] == '\n')
        {
            row++;
            if (row == pos)
                return i + 1;
        }
    }
    return g_FileSize;
}

/* ================= API ================= */

BOOL OpenFile(LPSTR path)
{
    if (g_hFile != INVALID_HANDLE_VALUE)
        return FALSE;

    g_hFile = CreateFileA(
        path,
        GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL
    );

    if (g_hFile == INVALID_HANDLE_VALUE)
        return FALSE;

    g_Buffer = (CHAR*)HeapAlloc(
        GetProcessHeap(),
        HEAP_ZERO_MEMORY,
        MAX_FILE_SIZE
    );

    if (!g_Buffer)
        return FALSE;

    LoadFile();
    return TRUE;
}

/* ---------- AddRow ---------- */

BOOL AddRow(LPSTR row, INT pos)
{
    if (g_hFile == INVALID_HANDLE_VALUE || row == NULL)
        return FALSE;

    LoadFile();
    INT rows = CountRows();

    if (pos == 0)
        pos = 1;
    else if (pos == -1)
        pos = rows + 1;
    else if (pos < 1 || pos > rows + 1)
        return FALSE;

    DWORD offset = GetInsertOffset(pos);
    DWORD len = lstrlenA(row);

    if (g_FileSize + len + 2 > MAX_FILE_SIZE)
        return FALSE;

    MoveMemory(
        g_Buffer + offset + len + 2,
        g_Buffer + offset,
        g_FileSize - offset
    );

    CopyMemory(g_Buffer + offset, row, len);
    g_Buffer[offset + len] = '\r';
    g_Buffer[offset + len + 1] = '\n';

    g_FileSize += len + 2;
    SaveFile();
    return TRUE;
}

/* ---------- RemRow ---------- */

BOOL RemRow(INT pos)
{
    if (g_hFile == INVALID_HANDLE_VALUE)
        return FALSE;

    LoadFile();
    INT rows = CountRows();
    INT target;

    if (pos == 0)
        target = 1;
    else if (pos == -1)
        target = rows;
    else if (pos > 0 && pos <= rows)
        target = pos;
    else
        return FALSE;

    DWORD start = GetRowStart(target);
    DWORD end = GetRowEnd(start);

    MoveMemory(
        g_Buffer + start,
        g_Buffer + end,
        g_FileSize - end
    );

    g_FileSize -= (end - start);
    SaveFile();
    return TRUE;
}

/* ---------- PrintRow ---------- */

BOOL PrintRow(INT pos)
{
    if (g_hFile == INVALID_HANDLE_VALUE)
        return FALSE;

    LoadFile();
    INT rows = CountRows();
    INT target;

    if (pos == 0)
        target = 1;
    else if (pos == -1)
        target = rows;
    else if (pos > 0 && pos <= rows)
        target = pos;
    else
        return FALSE;

    DWORD start = GetRowStart(target);
    DWORD end = GetRowEnd(start);

    CHAR tmp[1024];
    DWORD len = end - start;

    CopyMemory(tmp, g_Buffer + start, len);
    tmp[len] = 0;

    Print(tmp);
    return TRUE;
}

/* ---------- PrintRows ---------- */

BOOL PrintRows()
{
    if (g_hFile == INVALID_HANDLE_VALUE)
        return FALSE;

    LoadFile();
    Print(g_Buffer);
    return TRUE;
}

/* ---------- CloseFile ---------- */

BOOL CloseFile()
{
    if (g_hFile == INVALID_HANDLE_VALUE)
        return FALSE;

    CloseHandle(g_hFile);
    HeapFree(GetProcessHeap(), 0, g_Buffer);

    g_hFile = INVALID_HANDLE_VALUE;
    g_Buffer = NULL;
    g_FileSize = 0;
    return TRUE;
}

/* ================= Меню ================= */

int main()
{
    while (1)
    {
        PrintLn("\n===== Menu =====");
        PrintLn("1. Open file");
        PrintLn("2. Add row");
        PrintLn("3. Remove row");
        PrintLn("4. Print row");
        PrintLn("5. Print all rows");
        PrintLn("6. Close file");
        PrintLn("0. Exit");

        INT cmd = ReadInt();
        CHAR buf[256];
        INT pos;

        switch (cmd)
        {
        case 1:
            Print("File path: ");
            ReadLine(buf, 256);
            OpenFile(buf) ? PrintLn("OK") : PrintLn("ERROR");
            break;

        case 2:
            Print("Row: ");
            ReadLine(buf, 256);
            Print("Pos: ");
            pos = ReadInt();
            AddRow(buf, pos) ? PrintLn("OK") : PrintLn("ERROR");
            break;

        case 3:
            Print("Pos: ");
            pos = ReadInt();
            RemRow(pos) ? PrintLn("OK") : PrintLn("ERROR");
            break;

        case 4:
            Print("Pos: ");
            pos = ReadInt();
            PrintRow(pos) ? PrintLn("") : PrintLn("ERROR");
            break;

        case 5:
            PrintRows() ? PrintLn("") : PrintLn("ERROR");
            break;

        case 6:
            CloseFile() ? PrintLn("Closed") : PrintLn("ERROR");
            break;

        case 0:
            return 0;
        }
    }
}
