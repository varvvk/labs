#include <windows.h>

int main()
{
	HANDLE hStopEvent = CreateEvent(NULL,
		TRUE, //FALSE - автосбрасываемое; TRUE - ручное
		FALSE,
		L"Stop");

	HANDLE hRestartEvent = CreateEvent(NULL,
		TRUE, //FALSE - автосбрасываемое; TRUE - ручное
		FALSE,
		L"Restart");

	SetEvent(hStopEvent);
	SetEvent(hRestartEvent);

	// Освобождаем мьютекс, чтобы клиенты могли завершить работу
	HANDLE hMutex = OpenMutex(SYNCHRONIZE, FALSE, L"HTStorageMutex");
	if (hMutex) {
		ReleaseMutex(hMutex);
		CloseHandle(hMutex);
	}
}