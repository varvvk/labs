// Lab_04x.cpp
#include <windows.h>
#include <stdio.h>
#include <string>

int main(int argc, char* argv[]) {
    if (argc < 2) {
        printf("Usage: Lab_04x <iterations>\n");
        return 1;
    }

    int iterations = atoi(argv[1]);
    std::string username = "Userbc";

    DWORD pid = GetCurrentProcessId();
    DWORD tid = GetCurrentThreadId();

    for (int i = 1; i <= iterations; ++i) {
        char ch = username[(i - 1) % username.size()];
        printf("%u - %u - %d - %c\n", pid, tid, i, ch);
        fflush(stdout);
        Sleep(400);
    }

    return 0;
}
