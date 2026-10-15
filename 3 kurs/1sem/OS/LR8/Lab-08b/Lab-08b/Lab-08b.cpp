#include <windows.h>

void Print(const char* text)
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

void Wait()
{
    Print("\nPress Enter...\n");

    char buffer[16];
    DWORD read;
    ReadConsoleA(
        GetStdHandle(STD_INPUT_HANDLE),
        buffer,
        sizeof(buffer) - 1,
        &read,
        NULL
    );
}


int main()
{
    SYSTEM_INFO si;
    GetSystemInfo(&si);
    DWORD pageSize = si.dwPageSize;

    // ===== STAGE 1 =====
    Print("Stage 1: Reserving 256 pages of virtual memory\n");

    SIZE_T totalSize = pageSize * 256;

    LPVOID base = VirtualAlloc(
        NULL,
        totalSize,
        MEM_RESERVE,
        PAGE_NOACCESS
    );

    if (!base)
    {
        Print("Error: MEM_RESERVE failed\n");
        return 1;
    }

    Print("Memory region base address: ");
    char buf[64];
    wsprintfA(buf, "%p\n", base);
    Print(buf);

    Wait();

    // ===== STAGE 2 =====
    Print("Stage 2: Committing the second half (128 pages)\n");

    LPVOID secondHalf = (LPBYTE)base + pageSize * 128;

    if (!VirtualAlloc(
        secondHalf,
        pageSize * 128,
        MEM_COMMIT,
        PAGE_READWRITE))
    {
        Print("Error: MEM_COMMIT failed\n");
        return 1;
    }

    Wait();

    // ===== STAGE 3 =====
    Print("Stage 3: Filling memory with sequential integers\n");

    int* data = (int*)secondHalf;
    int count = (pageSize * 128) / sizeof(int);

    for (int i = 0; i < count; i++)
        data[i] = i;

    Wait();

    // ===== STAGE 4 =====
    Print("Stage 4: Changing page protection to READ ONLY\n");

    DWORD oldProtect;
    VirtualProtect(
        secondHalf,
        pageSize * 128,
        PAGE_READONLY,
        &oldProtect
    );

    Wait();

    // ===== STAGE 5 =====
    Print("Stage 5: Decommitting physical memory (MEM_DECOMMIT)\n");

    VirtualFree(
        secondHalf,
        pageSize * 128,
        MEM_DECOMMIT
    );

    Wait();

    // ===== STAGE 6 =====
    Print("Stage 6: Releasing virtual memory (MEM_RELEASE)\n");

    VirtualFree(
        base,
        0,
        MEM_RELEASE
    );

    Wait();

    Print("Done.\n");
    return 0;
}
