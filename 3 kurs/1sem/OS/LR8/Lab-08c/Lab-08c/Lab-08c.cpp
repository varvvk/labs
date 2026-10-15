#define _WIN32_WINNT 0x0600
#include <windows.h>

void Print(const char* text)
{
    DWORD written;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE), text, lstrlenA(text), &written, NULL);
}

void PrintNumber(SIZE_T num)
{
    char buffer[32];
    wsprintfA(buffer, "%I64u", (unsigned long long)num); // для 64-битных систем
    Print(buffer);
}

void HeapInfo(HANDLE heap)
{
    PROCESS_HEAP_ENTRY entry;
    entry.lpData = NULL;

    SIZE_T totalSize = 0;
    Print("Heap information:\n");

    while (HeapWalk(heap, &entry))
    {
        char buffer[128];
        const char* type = (entry.wFlags & PROCESS_HEAP_ENTRY_BUSY) ? "BUSY" : "FREE/OTHER";

        wsprintfA(buffer, "Address: %p, Size: %I64u, Type: %s\n",
            entry.lpData, (unsigned long long)entry.cbData, type);
        Print(buffer);

        totalSize += entry.cbData;
    }

    DWORD err = GetLastError();
    if (err != ERROR_NO_MORE_ITEMS)
    {
        Print("HeapWalk error\n");
    }

    Print("Total heap size: ");
    PrintNumber(totalSize);
    Print("\n\n");
}

void PauseAndClear()
{
    Print("Press Enter...\n");
    char buffer[16];
    DWORD read;
    ReadConsoleA(GetStdHandle(STD_INPUT_HANDLE), buffer, sizeof(buffer) - 1, &read, NULL);
    system("cls");
}

int main()
{
    // 1. Создание пользовательской кучи: 1 МБ старт, 8 МБ максимум
    HANDLE heap = HeapCreate(0, 1024 * 1024, 8 * 1024 * 1024);
    if (!heap)
    {
        Print("HeapCreate failed\n");
        return 1;
    }

    Print("Heap created\n");
    HeapInfo(heap); PauseAndClear();

    // 2. Выделение блоков
    const int blockCount = 5;          // можно поменять на 5
    const SIZE_T blockSize = 1024 * 1024; // можно поменять на 1 МБ
    int** blocks = (int**)HeapAlloc(heap, 0, blockCount * sizeof(int*));
    if (!blocks)
    {
        Print("HeapAlloc for block pointers failed\n");
        HeapDestroy(heap);
        return 1;
    }

    for (int i = 0; i < blockCount; i++)
    {
        blocks[i] = (int*)HeapAlloc(heap, 0, blockSize);
        if (!blocks[i])
        {
            Print("HeapAlloc for block failed\n");
            break;
        }

        // Заполнение блока целыми числами
        SIZE_T count = blockSize / sizeof(int);
        for (SIZE_T j = 0; j < count; j++)
            blocks[i][j] = (int)j;

        char buf[64];
        wsprintfA(buf, "Allocated block %d\n", i+1);
        Print(buf);

        HeapInfo(heap); PauseAndClear();
    }

    // 3. Освобождение всех блоков
    for (int i = 0; i < blockCount; i++)
    {
        if (blocks[i])
            HeapFree(heap, 0, blocks[i]);
    }

    Print("All blocks freed\n");
    HeapInfo(heap); PauseAndClear();

    // 4. Освобождение массива указателей
    HeapFree(heap, 0, blocks);

    // 5. Уничтожение кучи
    HeapDestroy(heap);
    Print("Heap destroyed\n");

    return 0;
}
