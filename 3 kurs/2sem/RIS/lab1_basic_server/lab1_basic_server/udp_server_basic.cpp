#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include "iostream"
#include "string.h"
#include "locale"
#include "time.h"
#include "Winsock2.h"
#include <tchar.h>
#include <map>
#include <vector>
#pragma comment(lib, "WS2_32.lib")

using namespace std;

#define SIP  "127.0.0.1"//"172.20.10.3"
#define PORT 5000

// Структура для запроса от клиента (п.6)
struct SyncRequest {
    char cmd[32];     // всегда "SINC"
    int curvalue;     // текущее значение счетчика Cc
};

// Структура для ответа клиенту (п.8)
struct SyncResponse {
    char cmd[32];     // всегда "SINCRO"
    int correction;   // значение коррекции
};

// Структура для хранения данных клиента
struct ClientData {
    int requestCount;           // номер запроса для этого клиента
    vector<int> corrections;    // история коррекций для вычисления среднего
};

string GetErrorMsgText(int code)
{
    string msgText;

    switch (code)
    {
    case WSAEINTR:         msgText = "Работа функции прервана\n";              break;
    case WSAEACCES:         msgText = "Разрешение отвергнуто\n";              break;
    case WSAEFAULT:         msgText = "Ошибочный адрес\n";                  break;
    case WSAEINVAL:         msgText = "Ошибка в аргументе\n";                break;
    case WSAEMFILE:         msgText = "Слишком много файлов открыто\n";          break;
    case WSAEWOULDBLOCK:     msgText = "Ресурс временно недоступен\n";            break;
    case WSAEINPROGRESS:     msgText = "Операция в процессе развития\n";          break;
    case WSAEALREADY:        msgText = "Операция уже выполняется\n";            break;
    case WSAENOTSOCK:        msgText = "Сокет задан неправильно\n";              break;
    case WSAEDESTADDRREQ:     msgText = "Требуется адрес расположения\n";          break;
    case WSAEMSGSIZE:         msgText = "Сообщение слишком длинное\n";              break;
    case WSAEPROTOTYPE:       msgText = "Неправильный тип протокола для сокета\n";      break;
    case WSAENOPROTOOPT:     msgText = "Ошибка в опции протокола\n";            break;
    case WSAEPROTONOSUPPORT:   msgText = "Протокол не поддерживается\n";            break;
    case WSAESOCKTNOSUPPORT:   msgText = "Тип сокета не поддерживается\n";          break;
    case WSAEOPNOTSUPP:       msgText = "Операция не поддерживается\n";            break;
    case WSAEPFNOSUPPORT:     msgText = "Тип протоколов не поддерживается\n";        break;
    case WSAEAFNOSUPPORT:     msgText = "Тип адресов не поддерживается протоколом\n";    break;
    case WSAEADDRINUSE:       msgText = "Адрес уже используется\n";              break;
    case WSAEADDRNOTAVAIL:     msgText = "Запрошенный адрес не может быть использован\n";    break;
    case WSAENETDOWN:       msgText = "Сеть отключена\n";                  break;
    case WSAENETUNREACH:     msgText = "Сеть не достижима\n";                break;
    case WSAENETRESET:       msgText = "Сеть разорвала соединение\n";            break;
    case WSAECONNABORTED:     msgText = "Программный отказ связи\n";              break;
    case WSAECONNRESET:       msgText = "Связь восстановлена\n";                break;
    case WSAENOBUFS:       msgText = "Не хватает памяти для буферов\n";          break;
    case WSAEISCONN:       msgText = "Сокет уже подключен\n";                break;
    case WSAENOTCONN:       msgText = "Сокет не подключен\n";                break;
    case WSAESHUTDOWN:       msgText = "Нельзя выполнить send: сокет завершил работу\n";  break;
    case WSAETIMEDOUT:       msgText = "Закончился отведенный интервал  времени\n";      break;
    case WSAECONNREFUSED:     msgText = "Соединение отклонено\n";              break;
    case WSAEHOSTDOWN:       msgText = "Хост в неработоспособном состоянии\n";        break;
    case WSAEHOSTUNREACH:     msgText = "Нет маршрута для хоста\n";              break;
    case WSAEPROCLIM:       msgText = "Слишком много процессов\n";              break;
    case WSASYSNOTREADY:     msgText = "Сеть не доступна\n";                break;
    case WSAVERNOTSUPPORTED:   msgText = "Данная версия недоступна\n";            break;
    case WSANOTINITIALISED:     msgText = "Не выполнена инициализация WS2_32.DLL\n";      break;
    case WSAEDISCON:       msgText = "Выполняется отключение\n";              break;
    case WSATYPE_NOT_FOUND:     msgText = "Класс не найден\n";                  break;
    case WSAHOST_NOT_FOUND:     msgText = "Хост не найден\n";                  break;
    case WSATRY_AGAIN:       msgText = "Неавторизированный хост не найден\n";        break;
    case WSANO_RECOVERY:     msgText = "Неопределенная ошибка\n";              break;
    case WSANO_DATA:       msgText = "Нет записи запрошенного типа\n";          break;
    case WSA_INVALID_HANDLE:   msgText = "Указанный дескриптор события  с ошибкой\n";      break;
    case WSA_INVALID_PARAMETER:   msgText = "Один или более параметров с ошибкой\n";        break;
    case WSA_IO_INCOMPLETE:     msgText = "Объект ввода-вывода не в сигнальном состоянии\n"; break;
    case WSA_IO_PENDING:     msgText = "Операция завершится позже\n";            break;
    case WSA_NOT_ENOUGH_MEMORY:   msgText = "Не достаточно памяти\n";              break;
    case WSA_OPERATION_ABORTED:   msgText = "Операция отвергнута\n";                break;
    case WSAEINVALIDPROCTABLE:   msgText = "Ошибочный сервис\n";                break;
    case WSAEINVALIDPROVIDER:   msgText = "Ошибка в версии сервиса\n";              break;
    case WSAEPROVIDERFAILEDINIT: msgText = "Невозможно инициализировать сервис\n";        break;
    case WSASYSCALLFAILURE:     msgText = "Аварийное завершение системного вызова\n";      break;
    default:           msgText = "Error\n";                      break;
    };
    return msgText;
}

string SetErrorMsgText(string msgText, int code) {
    return msgText + GetErrorMsgText(code);
}

int setAverageCorrection(const vector<int>& corrections) {
    if (corrections.empty()) return 0;

    int value = 0;
    int count = min((int)corrections.size(), 10); // берем последние 10

    for (int i = corrections.size() - count; i < corrections.size(); i++) {
        value += corrections[i];
    }
    return value / count;
}

int _tmain(int argc, _TCHAR* argv[]) {
    setlocale(LC_ALL, "Russian");

    cout << "=== Time Synchronization Server ===" << endl;
    cout << "IP: " << SIP << ", Port: " << PORT << endl << endl;

    try {
        SOCKET sS;
        WSADATA wsaData;

        if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0)
            throw SetErrorMsgText("Startup: ", WSAGetLastError());

        if ((sS = socket(AF_INET, SOCK_DGRAM, NULL)) == INVALID_SOCKET)
            throw SetErrorMsgText("Socket: ", WSAGetLastError());

        SOCKADDR_IN serv;
        serv.sin_family = AF_INET;
        serv.sin_port = htons(PORT);
        serv.sin_addr.s_addr = inet_addr(SIP);

        if (bind(sS, (LPSOCKADDR)&serv, sizeof(serv)) == SOCKET_ERROR)
            throw SetErrorMsgText("Bind_Server: ", WSAGetLastError());

        // Карта для хранения данных по каждому клиенту
        map<string, ClientData> clients;

        SYSTEMTIME tm;

        cout << "Server is running. Waiting for requests..." << endl << endl;

        while (true) {
            SOCKADDR_IN client;
            int lc = sizeof(client);

            SyncRequest request;
            SyncResponse response;

            // Получаем запрос от клиента
            int bytesReceived = recvfrom(sS, (char*)&request, sizeof(request),
                NULL, (sockaddr*)&client, &lc);

            if (bytesReceived == SOCKET_ERROR) {
                cout << "Error receiving: " << WSAGetLastError() << endl;
                continue;
            }

            // Проверяем команду
            if (strcmp(request.cmd, "SINC") != 0) {
                cout << "Unknown command: " << request.cmd << " from "
                    << inet_ntoa(client.sin_addr) << endl;
                continue;
            }

            GetSystemTime(&tm);

            // Пункт 9: время сервера в тиках с момента запуска
            clock_t Cs = clock();
            // Пункт 14: вычисляем correction = Cs - curvalue
            int correction = Cs - request.curvalue;

            // Получаем IP клиента как строку для идентификации
            string clientIP = inet_ntoa(client.sin_addr);

            // Обновляем данные клиента
            ClientData& data = clients[clientIP];
            data.requestCount++;
            data.corrections.push_back(correction);

            // Вычисляем среднее значение (по последним 10 запросам)
            int average = setAverageCorrection(data.corrections);

            // Формируем ответ
            strcpy_s(response.cmd, "SINCRO");
            response.correction = correction;

            // Отправляем ответ клиенту
            sendto(sS, (char*)&response, sizeof(response), 0,
                (sockaddr*)&client, sizeof(client));

            // Пункт 15: вывод информации после отправки ответа
            cout << "=========================================" << endl;
            cout << "Time: " << tm.wHour << ":" << tm.wMinute << ":"
                << tm.wSecond << "." << tm.wMilliseconds << endl;
            cout << "Client IP: " << clientIP << endl;
            cout << "Request #" << data.requestCount << endl;
            cout << "Cs = " << Cs << " ticks" << endl;
            cout << "curvalue = " << request.curvalue << endl;
            cout << "Correction = " << correction << endl;
            cout << "Average correction (last 10): " << average << endl;
            cout << "=========================================" << endl << endl;
        }

        if (closesocket(sS) == SOCKET_ERROR)
            throw SetErrorMsgText("close socket: ", WSAGetLastError());
        if (WSACleanup() == SOCKET_ERROR)
            throw SetErrorMsgText("Cleanup: ", WSAGetLastError());
    }
    catch (string errorMsgText) {
        cout << endl << "ERROR: " << errorMsgText << endl;
    }

    system("pause");
    return 0;
}