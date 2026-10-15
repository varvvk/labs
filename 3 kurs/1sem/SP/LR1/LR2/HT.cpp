#include "HT.h"
#include <iostream>
#include <string>
#include <chrono>
#include <thread>

// Убрали namespace HTAPI и используем просто функции

HT* Create(int size, int snapshotInterval, const std::string& fileName) {
    HT* ht = new HT();
    ht->size = size;
    ht->snapshotInterval = snapshotInterval;
    ht->isSnapshotActive = false;

    // Создание файла
    ht->hFile = CreateFileA(
        fileName.c_str(),
        GENERIC_READ | GENERIC_WRITE,
        FILE_SHARE_READ | FILE_SHARE_WRITE,
        NULL,
        CREATE_ALWAYS,
        FILE_ATTRIBUTE_NORMAL,
        NULL
    );

    if (ht->hFile == INVALID_HANDLE_VALUE) {
        delete ht;
        return nullptr;
    }

    // Установка размера файла
    LARGE_INTEGER fileSize;
    fileSize.QuadPart = sizeof(int) * 2 + size * sizeof(Element); // size и count + элементы
    SetFilePointerEx(ht->hFile, fileSize, NULL, FILE_BEGIN);
    SetEndOfFile(ht->hFile);

    // Создание File Mapping
    ht->hMapFile = CreateFileMapping(
        ht->hFile,
        NULL,
        PAGE_READWRITE,
        fileSize.HighPart,
        fileSize.LowPart,
        NULL
    );

    if (ht->hMapFile == NULL) {
        CloseHandle(ht->hFile);
        delete ht;
        return nullptr;
    }

    // Отображение в память
    ht->pMapView = MapViewOfFile(
        ht->hMapFile,
        FILE_MAP_ALL_ACCESS,
        0, 0, 0
    );

    if (ht->pMapView == NULL) {
        CloseHandle(ht->hMapFile);
        CloseHandle(ht->hFile);
        delete ht;
        return nullptr;
    }

    // Инициализация структур данных
    int* data = (int*)ht->pMapView;
    data[0] = size;    // размер таблицы
    data[1] = 0;       // количество элементов

    // Элементы начинаются после двух integer'ов
    Element* elements = (Element*)((char*)ht->pMapView + sizeof(int) * 2);
    for (int i = 0; i < size; i++) {
        elements[i].key = nullptr;
        elements[i].payload = nullptr;
    }

    // Создание мьютекса для потокобезопасности
    ht->hMutex = CreateMutex(NULL, FALSE, NULL);

    return ht;
}

BOOL Insert(HT* ht, Element* element) {
    WaitForSingleObject(ht->hMutex, INFINITE);

    int* data = (int*)ht->pMapView;
    int size = data[0];
    int count = data[1];
    Element* elements = (Element*)((char*)ht->pMapView + sizeof(int) * 2);

    if (count >= size) {
        ReleaseMutex(ht->hMutex);
        return FALSE;
    }

    // Поиск свободной ячейки
    for (int i = 0; i < size; i++) {
        if (elements[i].key == nullptr) {
            elements[i] = *element;
            data[1] = count + 1;
            break;
        }
    }

    ReleaseMutex(ht->hMutex);
    return TRUE;
}

Element* Get(HT* ht, Element* element) {
    WaitForSingleObject(ht->hMutex, INFINITE);

    int* data = (int*)ht->pMapView;
    int size = data[0];
    Element* elements = (Element*)((char*)ht->pMapView + sizeof(int) * 2);

    for (int i = 0; i < size; i++) {
        if (elements[i].key != nullptr &&
            strcmp(elements[i].key, element->key) == 0) {
            Element* result = &elements[i];
            ReleaseMutex(ht->hMutex);
            return result;
        }
    }

    ReleaseMutex(ht->hMutex);
    return nullptr;
}

BOOL Snap(HT* ht) {
    WaitForSingleObject(ht->hMutex, INFINITE);
    FlushViewOfFile(ht->pMapView, 0);
    ReleaseMutex(ht->hMutex);
    return TRUE;
}

BOOL Close(HT* ht) {
    ht->isSnapshotActive = false;

    if (ht->pMapView) UnmapViewOfFile(ht->pMapView);
    if (ht->hMapFile) CloseHandle(ht->hMapFile);
    if (ht->hFile) CloseHandle(ht->hFile);
    if (ht->hMutex) CloseHandle(ht->hMutex);
    if (ht->hSnapshotTimer) CloseHandle(ht->hSnapshotTimer);

    delete ht;
    return TRUE;
}