#pragma warning(disable : 4996)

#include <iostream>
#include <windows.h>
#include <conio.h>

#include "../OS13_HTCOM_LIB/pch.h"
#include "../OS13_HTCOM_LIB/OS13_HTCOM_LIB.h"

#ifdef _WIN64
#pragma comment(lib, "../x64/Debug/OS13_HTCOM_LIB.lib")
#else
#pragma comment(lib, "../Debug/OS13_HTCOM_LIB.lib")
#endif

using namespace std;

wchar_t* getWC(const char* c);

int main(int argc, char* argv[])
{
	HANDLE hStopEvent = CreateEvent(NULL,
		TRUE, //FALSE - автосбрасываемое; TRUE - ручное
		FALSE,
		L"Stop");

	HANDLE hRestartEvent = CreateEvent(NULL,
		TRUE, //FALSE - автосбрасываемое; TRUE - ручное
		FALSE,
		L"Restart");

	// Сбрасываем событие перезапуска при запуске сервера
	ResetEvent(hStopEvent);
	ResetEvent(hRestartEvent);

	// Создаем мьютекс для контроля доступа к хранилищу
	HANDLE hMutex = CreateMutex(NULL, TRUE, L"HTStorageMutex");
	if (!hMutex) {
		cout << "-- CreateMutex failed" << endl;
		return 1;
	}

	setlocale(LC_ALL, "Russian");

	try
	{
		cout << "[START] server start:" << endl;
		OS13_HTCOM_HANDEL h = OS13_HTCOM::Init();

		ht::HtHandle* ht = nullptr;
		wchar_t* fileName = getWC(argv[1]);
		ht = OS13_HTCOM::HT::open(h, fileName, false);
		if (ht)
		{
			cout << "HT-Storage Start" << endl;
			wcout << "filename: " << ht->fileName << endl;
			cout << "secSnapshotInterval: " << ht->secSnapshotInterval << endl;
			cout << "capacity: " << ht->capacity << endl;
			cout << "maxKeyLength: " << ht->maxKeyLength << endl;
			cout << "maxPayloadLength: " << ht->maxPayloadLength << endl;

			while (!kbhit() && WaitForSingleObject(hStopEvent, 0) == WAIT_TIMEOUT)
				SleepEx(0, TRUE);

			OS13_HTCOM::HT::snap(h, ht);
			OS13_HTCOM::HT::close(h, ht);
		}
		else
			cout << "-- open: error" << endl;

		cout << endl << "Release dll, if its needed:" << endl;
		OS13_HTCOM::Dispose(h);

		// Освобождаем мьютекс при остановке сервера
		ReleaseMutex(hMutex);
		CloseHandle(hMutex);
	}
	catch (const char* e) { cout << e << endl; }
	catch (int e) { cout << "HRESULT: " << e << endl; }

}

wchar_t* getWC(const char* c)
{
	wchar_t* wc = new wchar_t[strlen(c) + 1];
	mbstowcs(wc, c, strlen(c) + 1);

	return wc;
}