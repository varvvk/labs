#include <Windows.h>

#define BUFFER_SIZE 512

// =====================
// Console output
// =====================
void Print(LPCSTR text)
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

// =====================
// Print FILETIME
// =====================
void PrintFileTime(FILETIME ft)
{
    SYSTEMTIME st;
    FileTimeToSystemTime(&ft, &st);

    char buffer[64];
    wsprintfA(
        buffer,
        "%02d.%02d.%04d %02d:%02d:%02d\n",
        st.wDay, st.wMonth, st.wYear,
        st.wHour, st.wMinute, st.wSecond
    );

    Print(buffer);
}

// =====================
// Text/Binary check
// =====================
BOOL IsTextFile(HANDLE hFile)
{
    BYTE buffer[BUFFER_SIZE];
    DWORD read;

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);

    if (!ReadFile(hFile, buffer, BUFFER_SIZE, &read, NULL))
        return FALSE;

    for (DWORD i = 0; i < read; i++)
        if (buffer[i] == 0)
            return FALSE;

    return TRUE;
}

// =====================
// Binary file type
// =====================
void PrintBinaryType(HANDLE hFile)
{
    BYTE buffer[4];
    DWORD read;

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);
    ReadFile(hFile, buffer, 4, &read, NULL);

    if (buffer[0] == 'M' && buffer[1] == 'Z')
        Print("Binary file: PE (EXE/DLL)\n");
    else if (buffer[0] == 0x7F &&
        buffer[1] == 'E' &&
        buffer[2] == 'L' &&
        buffer[3] == 'F')
        Print("Binary file: ELF\n");
    else if (buffer[0] == 'P' && buffer[1] == 'K')
        Print("Binary file: ZIP / Office document\n");
    else
        Print("Binary file: unknown type\n");
}

// =====================
// File information
// =====================
void PrintInfo(LPSTR FileName)
{
    HANDLE hFile = CreateFileA(
        FileName,
        GENERIC_READ,
        FILE_SHARE_READ,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL
    );

    if (hFile == INVALID_HANDLE_VALUE)
    {
        Print("Error opening file\n");
        return;
    }

    BY_HANDLE_FILE_INFORMATION info;
    if (!GetFileInformationByHandle(hFile, &info))
    {
        Print("Error getting file information\n");
        CloseHandle(hFile);
        return;
    }

    // File name
    Print("File name: ");
    Print(FileName);
    Print("\n");

    // File size
    ULONGLONG size =
        ((ULONGLONG)info.nFileSizeHigh << 32) | info.nFileSizeLow;

    ULONGLONG kib_int = size / 1024;
    ULONGLONG kib_frac = (size % 1024) * 100 / 1024;

    ULONGLONG mib_int = size / (1024 * 1024);
    ULONGLONG mib_frac = (size % (1024 * 1024)) * 100 / (1024 * 1024);

    char buffer[128];
    wsprintfA(
        buffer,
        "File size: %I64u B (%I64u.%02I64u KiB, %I64u.%02I64u MiB)\n",
        size,
        kib_int, kib_frac,
        mib_int, mib_frac
    );
    Print(buffer);

    // File type
    DWORD type = GetFileType(hFile);
    Print("File type: ");
    if (type == FILE_TYPE_DISK) Print("Disk file\n");
    else if (type == FILE_TYPE_CHAR) Print("Character device\n");
    else if (type == FILE_TYPE_PIPE) Print("Pipe\n");
    else Print("Unknown\n");

    // Time info
    Print("Creation time: ");
    PrintFileTime(info.ftCreationTime);

    Print("Last access: ");
    PrintFileTime(info.ftLastAccessTime);

    Print("Last modification: ");
    PrintFileTime(info.ftLastWriteTime);

    // Text / binary
    if (IsTextFile(hFile))
        Print("Text file\n");
    else
        PrintBinaryType(hFile);

    CloseHandle(hFile);
}

// =====================
// Print text file content
// =====================
void PrintText(LPSTR FileName)
{
    HANDLE hFile = CreateFileA(
        FileName,
        GENERIC_READ,
        FILE_SHARE_READ,
        NULL,
        OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL,
        NULL
    );

    if (hFile == INVALID_HANDLE_VALUE)
    {
        Print("Error opening file\n");
        return;
    }

    if (!IsTextFile(hFile))
    {
        Print("File is not a text file\n");
        CloseHandle(hFile);
        return;
    }

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);

    char buffer[BUFFER_SIZE + 1];
    DWORD read;

    Print("\n----- FILE CONTENT -----\n");

    while (ReadFile(hFile, buffer, BUFFER_SIZE, &read, NULL) && read)
    {
        buffer[read] = 0;
        Print(buffer);
    }

    Print("\n-----------------------\n");

    CloseHandle(hFile);
}

// =====================
// Entry point
// =====================
int main(int argc, char* argv[])
{
    if (argc < 2)
    {
        Print("Usage: Lab-09a <file_path>\n");
        return 1;
    }

    PrintInfo(argv[1]);
    PrintText(argv[1]);

    return 0;
}
