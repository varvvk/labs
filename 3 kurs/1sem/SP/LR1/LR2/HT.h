#pragma once
#include <windows.h>
#include <string>
#include <cstring> // для strlen

#define DEFAULT_SIZE 100
#define DEFAULT_SNAPSHOT_INTERVAL 30000 // 30 секунд

struct Element {
    const char* key;
    int keyLength;
    const char* payload;
    int payloadLength;
};

struct HT {
    int size;
    int snapshotInterval;
    HANDLE hFile;
    HANDLE hMapFile;
    LPVOID pMapView;
    HANDLE hMutex;
    HANDLE hSnapshotTimer;
    bool isSnapshotActive;
};

// Убрали HTAPI:: из объявлений функций
HT* Create(int size, int snapshotInterval, const std::string& fileName);
HT* Open(const std::string& fileName);
BOOL Snap(HT* ht);
BOOL Close(HT* ht);
BOOL Insert(HT* ht, Element* element);
BOOL Delete(HT* ht, Element* element);
Element* Get(HT* ht, Element* element);
BOOL Update(HT* ht, Element* oldElement, Element* newElement);
char* GetLastError(HT* ht);