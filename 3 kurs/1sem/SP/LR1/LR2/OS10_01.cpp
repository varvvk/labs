#include "HT.h"
#include <iostream>

int main() {
    // Создание HT
    HT* ht = Create(100, 30000, "test.ht");
    if (!ht) {
        std::cout << "Error creating HT" << std::endl;
        return 1;
    }

    // Вставка элемента
    Element element;
    element.key = "testKey";
    element.keyLength = strlen("testKey");
    element.payload = "testPayload";
    element.payloadLength = strlen("testPayload");

    if (Insert(ht, &element)) {
        std::cout << "Insert successful" << std::endl;
    }

    // Получение элемента
    Element searchElement;
    searchElement.key = "testKey";
    searchElement.keyLength = strlen("testKey");

    Element* result = Get(ht, &searchElement);
    if (result) {
        std::cout << "Get result: " << result->payload << std::endl;
    }

    // Синхронный snapshot
    Snap(ht);

    // Закрытие HT
    Close(ht);

    return 0;
}