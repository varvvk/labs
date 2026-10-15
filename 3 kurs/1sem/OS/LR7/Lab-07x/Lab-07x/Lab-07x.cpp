#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

static int u64_to_wstr(ULONGLONG v, WCHAR* out) {
    WCHAR tmp[32];
    int n = 0;
    if (v == 0) { out[0] = L'0'; return 1; }
    while (v > 0) { tmp[n++] = (WCHAR)(L'0' + (v % 10ULL)); v /= 10ULL; }
    for (int i = 0; i < n; ++i) out[i] = tmp[n - 1 - i];
    return n;
}

static void put3(WCHAR* p, DWORD v) { // 000..999
    p[0] = (WCHAR)(L'0' + (v / 100) % 10);
    p[1] = (WCHAR)(L'0' + (v / 10) % 10);
    p[2] = (WCHAR)(L'0' + (v % 10));
}

static void write_line2(ULONGLONG idx, ULONGLONG value) {
    // формат: "123: 456\r\n"
    WCHAR buf[128];
    int pos = 0;

    pos += u64_to_wstr(idx, buf + pos);
    buf[pos++] = L':';
    buf[pos++] = L' ';
    pos += u64_to_wstr(value, buf + pos);
    buf[pos++] = L'\r';
    buf[pos++] = L'\n';

    DWORD written = 0;
    WriteConsoleW(GetStdHandle(STD_OUTPUT_HANDLE), buf, (DWORD)pos, &written, NULL);
}

static void write_elapsed(DWORD ms) {
    // "Elapsed: 12345 ms (12.345 s)\r\n"
    WCHAR buf[160];
    int pos = 0;

    const WCHAR* p1 = L"Elapsed: ";
    while (*p1) buf[pos++] = *p1++;

    pos += u64_to_wstr((ULONGLONG)ms, buf + pos);

    const WCHAR* p2 = L" ms (";
    while (*p2) buf[pos++] = *p2++;

    DWORD sec = ms / 1000;
    DWORD rem = ms % 1000;

    pos += u64_to_wstr((ULONGLONG)sec, buf + pos);
    buf[pos++] = L'.';
    put3(buf + pos, rem); pos += 3;

    const WCHAR* p3 = L" s)\r\n";
    while (*p3) buf[pos++] = *p3++;

    DWORD written = 0;
    WriteConsoleW(GetStdHandle(STD_OUTPUT_HANDLE), buf, (DWORD)pos, &written, NULL);
}

static DWORD parse_u32_arg_or_default(DWORD defVal) {
    LPCWSTR cmd = GetCommandLineW();
    int i = 0;


    if (cmd[i] == L'"') { i++; while (cmd[i] && cmd[i] != L'"') i++; if (cmd[i] == L'"') i++; }
    else { while (cmd[i] && cmd[i] != L' ' && cmd[i] != L'\t') i++; }

    while (cmd[i] == L' ' || cmd[i] == L'\t') i++;
    if (!cmd[i]) return defVal;

    DWORD v = 0;
    int any = 0;
    while (cmd[i] >= L'0' && cmd[i] <= L'9') {
        any = 1;
        DWORD d = (DWORD)(cmd[i] - L'0');
        v = v * 10 + d;
        i++;
    }
    if (!any || v == 0) return defVal;
    return v;
}

int main() {
    // Сколько простых выводим 
    DWORD N = parse_u32_arg_or_default(1000);

    DWORD* primes = (DWORD*)HeapAlloc(GetProcessHeap(), 0, (SIZE_T)N * sizeof(DWORD));
    if (!primes) return 1;

    DWORD start = GetTickCount(); // “время Windows” в мс :contentReference[oaicite:1]{index=1}

    DWORD count = 0;
    DWORD candidate = 2;

    // 1-й простой
    primes[count++] = 2;
    write_line2(1, 2);
    candidate = 3;

    while (count < N) {
        int isPrime = 1;

        for (DWORD i = 0; i < count; ++i) {
            DWORD p = primes[i];
            ULONGLONG pp = (ULONGLONG)p * (ULONGLONG)p;
            if (pp > (ULONGLONG)candidate) break;
            if (candidate % p == 0) { isPrime = 0; break; }
        }

        if (isPrime) {
            primes[count] = candidate;
            write_line2((ULONGLONG)count + 1ULL, (ULONGLONG)candidate);
            count++;
        }
        candidate += 2;
    }

    DWORD elapsed = GetTickCount() - start;
    write_elapsed(elapsed);

    HeapFree(GetProcessHeap(), 0, primes);
    return 0;
}
