#include <windows.h>

// ================== Вывод ==================
void Print(const char* s)
{
    DWORD written;
    WriteConsoleA(GetStdHandle(STD_OUTPUT_HANDLE), s, lstrlenA(s), &written, NULL);
}

void PrintLine(const char* s)
{
    Print(s);
    Print("\r\n");
}

// ================== Ошибки ==================
void PrintErrorMessage(DWORD errorCode)
{
    char buffer[512];
    DWORD size = FormatMessageA(
        FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        errorCode,
        0,
        buffer,
        sizeof(buffer),
        NULL
    );
    if (size)
        PrintLine(buffer);
    else
        PrintLine("Unknown error");
}

// ================== Сравнение строк ==================
int StrCmpA(const char* a, const char* b)
{
    while (*a && *b)
    {
        if (*a != *b) return (*a - *b);
        a++;
        b++;
    }
    return (*a - *b);
}

// ================== Каталог ==================
void PrintDirectoryContents(const char* dirPath)
{
    char searchPath[MAX_PATH];
    lstrcpyA(searchPath, dirPath);
    lstrcatA(searchPath, "\\*");

    WIN32_FIND_DATAA findData;
    HANDLE hFind = FindFirstFileA(searchPath, &findData);
    if (hFind == INVALID_HANDLE_VALUE)
    {
        DWORD err = GetLastError();
        Print("Cannot open directory. ");
        PrintErrorMessage(err);
        return;
    }

    Print("Contents:\r\n");

    do
    {
        if (StrCmpA(findData.cFileName, ".") == 0 || StrCmpA(findData.cFileName, "..") == 0)
            continue;

        if (findData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
        {
            Print("[DIR]  ");
            PrintLine(findData.cFileName);
        }
        else
        {
            Print("[FILE] ");
            PrintLine(findData.cFileName);
        }

    } while (FindNextFileA(hFind, &findData));

    FindClose(hFind);
}

// ================== Отслеживание изменений ==================
void WatchDirectory(const char* dirPath)
{
    HANDLE hDir = CreateFileA(
        dirPath,
        FILE_LIST_DIRECTORY,
        FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
        NULL,
        OPEN_EXISTING,
        FILE_FLAG_BACKUP_SEMANTICS,
        NULL
    );

    if (hDir == INVALID_HANDLE_VALUE)
    {
        DWORD err = GetLastError();
        Print("Cannot watch directory. ");
        PrintErrorMessage(err);
        return;
    }

    BYTE buffer[4096];
    DWORD bytesReturned;
    FILE_NOTIFY_INFORMATION* fni;

    Print("Watching directory for changes...\r\n");

    while (1)
    {
        if (ReadDirectoryChangesW(
            hDir,
            buffer,
            sizeof(buffer),
            FALSE,
            FILE_NOTIFY_CHANGE_FILE_NAME |
            FILE_NOTIFY_CHANGE_DIR_NAME |
            FILE_NOTIFY_CHANGE_ATTRIBUTES |
            FILE_NOTIFY_CHANGE_SIZE |
            FILE_NOTIFY_CHANGE_LAST_WRITE |
            FILE_NOTIFY_CHANGE_LAST_ACCESS |
            FILE_NOTIFY_CHANGE_CREATION |
            FILE_NOTIFY_CHANGE_SECURITY,
            &bytesReturned,
            NULL,
            NULL
        ))
        {
            DWORD offset = 0;
            do
            {
                fni = (FILE_NOTIFY_INFORMATION*)(buffer + offset);

                char fileName[MAX_PATH];
                int len = WideCharToMultiByte(CP_ACP, 0, fni->FileName, fni->FileNameLength / sizeof(WCHAR), fileName, MAX_PATH - 1, NULL, NULL);
                fileName[len] = 0;

                switch (fni->Action)
                {
                case FILE_ACTION_ADDED:
                    Print("[ADDED] ");
                    break;
                case FILE_ACTION_REMOVED:
                    Print("[REMOVED] ");
                    break;
                case FILE_ACTION_MODIFIED:
                    Print("[MODIFIED] ");
                    break;
                case FILE_ACTION_RENAMED_OLD_NAME:
                    Print("[RENAMED FROM] ");
                    break;
                case FILE_ACTION_RENAMED_NEW_NAME:
                    Print("[RENAMED TO] ");
                    break;
                default:
                    Print("[UNKNOWN] ");
                    break;
                }

                PrintLine(fileName);

                offset = fni->NextEntryOffset;
            } while (fni->NextEntryOffset != 0);
        }
        else
        {
            DWORD err = GetLastError();
            Print("Failed to read changes. ");
            PrintErrorMessage(err);
            break;
        }
    }

    CloseHandle(hDir);
}

// ================== Main ==================
int main(int argc, char* argv[])
{
    if (argc < 2)
    {
        PrintLine("Usage: lab-09c.exe <directory_path>");
        return 0;
    }

    const char* dirPath = argv[1];

    DWORD attrs = GetFileAttributesA(dirPath);
    if (attrs == INVALID_FILE_ATTRIBUTES)
    {
        Print("Directory does not exist or cannot be accessed.\r\n");
        return 0;
    }

    if (!(attrs & FILE_ATTRIBUTE_DIRECTORY))
    {
        Print("Path is not a directory.\r\n");
        return 0;
    }

    PrintDirectoryContents(dirPath);
    WatchDirectory(dirPath);

    return 0;
}
