#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include "Winsock2.h"
#include <iostream>
#include <string>
#include <ctime>
#include <tchar.h>
#include <climits>
#pragma comment(lib, "WS2_32.lib")

#define SIP "26.162.87.27"//"26.162.87.27"
#define PORT 5000

using namespace std;

// Структура для запроса к серверу (п.6)
struct SyncRequest {
    char cmd[32];     // всегда "SINC"
    int curvalue;     // текущее значение счетчика Cc
};

// Структура для ответа от сервера (п.8)
struct SyncResponse {
    char cmd[32];     // всегда "SINCRO"
    int correction;   // значение коррекции
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

int _tmain(int argc, _TCHAR* argv[]) {
    setlocale(LC_CTYPE, "Russian");

    string IP = SIP;

    // ЗДЕСЬ ВЫ МОЖЕТЕ МЕНЯТЬ ЗНАЧЕНИЕ Tc
    int Tc = 1000; // периодичность запросов в тиках (1/1000 сек)

    cout << "=== Client for Time Synchronization Lab ===" << endl;
    cout << "Server IP: " << IP << ", Port: " << PORT << endl;
    cout << "Tc = " << Tc << " ticks (1 tick = 1/1000 sec)" << endl << endl;

    try {
        // Инициализация Winsock
        SOCKET cS;
        WSADATA wsaData;

        if (WSAStartup(MAKEWORD(2, 0), &wsaData) != 0)
            throw SetErrorMsgText("Startup: ", WSAGetLastError());

        if ((cS = socket(AF_INET, SOCK_DGRAM, NULL)) == INVALID_SOCKET)
            throw SetErrorMsgText("Socket: ", WSAGetLastError());

        SOCKADDR_IN serv;
        serv.sin_family = AF_INET;
        serv.sin_port = htons(PORT);
        serv.sin_addr.s_addr = inet_addr(IP.c_str());

        SyncRequest request;
        SyncResponse response;

        // Пункт 10: начальное значение счетчика = 0
        strcpy_s(request.cmd, "SINC");
        request.curvalue = 0;

        // Переменные для статистики
        int maxCorr = INT_MIN;
        int minCorr = INT_MAX;
        int sumCorr = 0;
        int corrections[10];

        SYSTEMTIME tm;
        int lensockaddr = sizeof(serv);

        cout << "Starting 10 requests with Tc = " << Tc << "..." << endl << endl;

        // 10 запросов
        for (int i = 0; i < 10; i++) {
            GetSystemTime(&tm);

            cout << "--- Request " << i + 1 << " ---" << endl;
            cout << "Time: " << tm.wHour << ":" << tm.wMinute << ":"
                << tm.wSecond << "." << tm.wMilliseconds << endl;

            // ПЕРЕД отправкой запроса - текущее значение curvalue
            cout << "Before request: Cc = " << request.curvalue << endl;

            // ШАГ А: Отправляем запрос с текущим curvalue
            if (sendto(cS, (char*)&request, sizeof(request), 0,
                (sockaddr*)&serv, sizeof(serv)) == SOCKET_ERROR) {
                throw SetErrorMsgText("Sendto: ", WSAGetLastError());
            }

            // ШАГ Б: Получаем ответ с correction
            if (recvfrom(cS, (char*)&response, sizeof(response), 0,
                (sockaddr*)&serv, &lensockaddr) == SOCKET_ERROR) {
                throw SetErrorMsgText("Recvfrom: ", WSAGetLastError());
            }
            // Проверка команды
            if (strcmp(response.cmd, "SINCRO") != 0) {
                cout << "Warning: Invalid response command: " << response.cmd << endl;
                continue;
            }

            // Сохраняем коррекцию
            corrections[i] = response.correction;

            // Обновляем статистику
            if (response.correction > maxCorr) maxCorr = response.correction;
            if (response.correction < minCorr) minCorr = response.correction;
            sumCorr += response.correction;

            cout << "Received correction = " << response.correction << endl;

            // ШАГ В: Корректируем счетчик (прибавляем correction) - Пункт 11
            request.curvalue = request.curvalue + response.correction;
            cout << "After correction: Cc = " << request.curvalue << endl;

            // ШАГ Г: Задержка между запросами - Пункт 12
            if (i < 9) { // не ждем после последнего запроса
                cout << "Sleeping for " << Tc << " ticks..." << endl;
                Sleep(Tc);

                // ШАГ Д: Увеличиваем счетчик на Tc ПОСЛЕ задержки - Пункт 13
                request.curvalue = request.curvalue + Tc;
                cout << "After adding Tc: Cc = " << request.curvalue << endl;
            }

            cout << "Current stats - Max: " << maxCorr << ", Min: " << minCorr << endl << endl;
        }

        // Вывод результатов
        cout << "\n========== RESULTS FOR Tc = " << Tc << " ==========" << endl;
        cout << "All correction values:" << endl;
        for (int i = 0; i < 10; i++) {
            cout << "  Request " << i + 1 << ": " << corrections[i] << endl;
        }
        cout << "\nMax correction: " << maxCorr << endl;
        cout << "Min correction: " << minCorr << endl;
        cout << "Average correction: " << (double)sumCorr / 10.0 << endl;
        cout << "===============================================" << endl;

        // Закрытие сокета
        if (closesocket(cS) == SOCKET_ERROR)
            throw SetErrorMsgText("Closesocket: ", WSAGetLastError());

        if (WSACleanup() == SOCKET_ERROR)
            throw SetErrorMsgText("Cleanup: ", WSAGetLastError());
    }
    catch (string errorMsgText) {
        cout << endl << "ERROR: " << errorMsgText << endl;
    }

    system("pause");
    return 0;
}