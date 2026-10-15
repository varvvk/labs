#pragma warning(disable : 4996)

#include <iostream>
#include <windows.h>
#include <string>
#include <sstream>

#include "../OS13_HTCOM_LIB/pch.h"
#include "../OS13_HTCOM_LIB/OS13_HTCOM_LIB.h"

#ifdef _WIN64
#pragma comment(lib, "../x64/Debug/OS13_HTCOM_LIB.lib")
#else
#pragma comment(lib, "../Debug/OS13_HTCOM_LIB.lib")
#endif

using namespace std;

string intToString(int number);

int main(int argc, char* argv[])
{
	setlocale(LC_ALL, "Russian");

	try
	{
		cout << "Запуск клиента:" << endl;
		OS13_HTCOM_HANDEL h = OS13_HTCOM::Init();

		ht::HtHandle* ht = nullptr;

		while (true) {
			if (!ht) {
				ht = OS13_HTCOM::HT::open(h, L"HTspace.ht", true);
				if (ht) {
					cout << "-- open: success" << endl;
				}
				else {
					cout << "-- open: waiting for HTStorage..." << endl;
					Sleep(3000);
					continue;
				}
			}

			HANDLE hMutex = OpenMutex(SYNCHRONIZE, FALSE, L"HTStorageMutex");
			if (!hMutex) {
				cout << "HTStorage not available. Waiting for restart..." << endl;

				if (ht) {
					OS13_HTCOM::HT::close(h, ht);
					ht = nullptr;
				}

				Sleep(3000);
				continue;
			}
			CloseHandle(hMutex);

			int numberKey = rand() % 50;
			string key = intToString(numberKey);
			cout << key << endl;

			ht::Element* element = OS13_HTCOM::Element::createInsertElement(h, key.c_str(), key.length() + 1, "0", 2);
			if (OS13_HTCOM::HT::insert(h, ht, element)) {
				cout << "-- insert: success" << endl;
			}
			else {
				cout << "-- insert: error" << endl;

				OS13_HTCOM::HT::close(h, ht);
				ht = nullptr;
			}

			delete element;
			Sleep(1000);
		}

		if (ht) {
			OS13_HTCOM::HT::close(h, ht);
		}

		cout << endl << "Освобождаем dll, если необходимо:" << endl;
		OS13_HTCOM::Dispose(h);
	}
	catch (const char* e) { cout << e << endl; }
	catch (int e) { cout << "HRESULT: " << e << endl; }

}

string intToString(int number)
{
	stringstream convert;
	convert << number;

	return convert.str();
}
