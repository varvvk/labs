# Подробное объяснение работы COM-обертки OS13_HTCOM.dll

## Обзор

**OS13_HTCOM.dll** - это COM-компонент (In-Process Server), который оборачивает базовую библиотеку HT API (OS10_HTAPI.lib) и предоставляет доступ к функциональности хеш-таблицы через стандартный COM-интерфейс.

---

## 1. Архитектура COM-компонента

### Структура файлов:

```
OS13_HTCOM/
├── dllmain.cpp          - Точка входа DLL, глобальные переменные
├── ExportFunctions.cpp  - Экспортируемые COM-функции
├── ClassFactory.cpp     - Фабрика классов для создания компонентов
├── ComponentHT.cpp      - Реализация COM-компонента
├── ComponentHT.h        - Определение класса ComponentHT
├── Interface.h          - Определение COM-интерфейсов (IHT, IElement)
├── Registry.cpp         - Регистрация/отмена регистрации в реестре
└── OS13_HTCOM.def       - Файл экспорта функций
```

---

## 2. Жизненный цикл COM-компонента

### Этап 1: Регистрация компонента

**Команда:** `regsvr32 OS13_HTCOM.dll`

**Что происходит:**

1. **Windows вызывает `DllRegisterServer()`** (из ExportFunctions.cpp):
   ```cpp
   STDAPI DllRegisterServer()
   {
       return RegisterServer(g_hModule,
           CLSID_ComponentHT,
           g_szFriendlyName,
           g_szVerIndProgID,
           g_szProgID);
   }
   ```

2. **Registry.cpp создает записи в реестре Windows:**
   - `HKEY_CLASSES_ROOT\CLSID\{693c66d1-3ce5-4512-a311-782618dbc7a6}`
     - `InprocServer32` → путь к DLL
     - `ProgID` → "OS13.ComponentHT"
   - `HKEY_CLASSES_ROOT\OS13.ComponentHT`
     - `CLSID` → GUID компонента

3. **Результат:** Windows теперь знает, где находится COM-компонент и как его загрузить.

---

### Этап 2: Создание экземпляра компонента

**Клиент вызывает:** `CoCreateInstance(CLSID_ComponentHT, ...)`

**Последовательность вызовов:**

```
1. CoCreateInstance(CLSID_ComponentHT, ...)
   ↓
2. Windows ищет CLSID в реестре
   ↓
3. Windows загружает OS13_HTCOM.dll
   ↓
4. Windows вызывает DllGetClassObject(CLSID_ComponentHT, IID_IClassFactory, ...)
   ↓
5. DllGetClassObject создает ClassFactory
   ↓
6. ClassFactory::CreateInstance создает ComponentHT
   ↓
7. ComponentHT::QueryInterface возвращает запрошенный интерфейс
```

**Детальный разбор:**

#### Шаг 1: DllGetClassObject (ExportFunctions.cpp)
```cpp
STDAPI DllGetClassObject(const CLSID& clsid, const IID& iid, void** ppv)
{
    // Проверка: правильный ли CLSID запрошен?
    if (clsid != CLSID_ComponentHT)
        return CLASS_E_CLASSNOTAVAILABLE;
    
    // Создание фабрики класса
    ClassFactory* pFactory = new ClassFactory;
    if (pFactory == NULL)
        return E_OUTOFMEMORY;
    
    // Получение интерфейса IClassFactory
    HRESULT hr = pFactory->QueryInterface(iid, ppv);
    pFactory->Release();  // Уменьшаем счетчик ссылок
    
    return hr;
}
```

#### Шаг 2: ClassFactory::CreateInstance (ClassFactory.cpp)
```cpp
HRESULT ClassFactory::CreateInstance(IUnknown* pUnknownOuter, 
                                      const IID& iid, 
                                      void** ppv)
{
    // Агрегирование не поддерживается
    if (pUnknownOuter != NULL)
        return CLASS_E_NOAGGREGATION;
    
    // СОЗДАНИЕ КОМПОНЕНТА
    ComponentHT* pA = new ComponentHT;  // ← Здесь создается компонент!
    if (pA == NULL)
        return E_OUTOFMEMORY;
    
    // Увеличиваем счетчик активных компонентов
    // (в конструкторе ComponentHT: InterlockedIncrement(&g_cComponents))
    
    // Получение запрошенного интерфейса
    HRESULT hr = pA->QueryInterface(iid, ppv);
    
    // Освобождаем временную ссылку
    pA->Release();
    
    return hr;
}
```

#### Шаг 3: ComponentHT::QueryInterface (ComponentHT.cpp)
```cpp
HRESULT ComponentHT::QueryInterface(const IID& iid, void** ppv)
{
    if (iid == IID_IUnknown)
    {
        *ppv = (IHT*)this;  // IHT наследуется от IUnknown
    }
    else if (iid == IID_IHT)
    {
        *ppv = (IHT*)this;
        // Компонент поддерживает интерфейс IHT
    }
    else if (iid == IID_IElement)
    {
        *ppv = (IElement*)this;
        // Компонент поддерживает интерфейс IElement
    }
    else
    {
        *ppv = NULL;
        return E_NOINTERFACE;  // Интерфейс не поддерживается
    }
    
    // Увеличиваем счетчик ссылок
    ((IUnknown*)*ppv)->AddRef();
    return S_OK;
}
```

---

## 3. Структура ComponentHT

### Наследование:
```cpp
class ComponentHT : public IHT, public IElement
{
    // Реализует оба интерфейса одновременно!
    // Это называется "множественное наследование интерфейсов"
}
```

### Интерфейсы:

#### IHT (Interface.h):
```cpp
interface IHT : IUnknown {
    STDMETHOD(create(...)) PURE;
    STDMETHOD(open(...)) PURE;
    STDMETHOD(insert(...)) PURE;
    STDMETHOD(get(...)) PURE;
    STDMETHOD(update(...)) PURE;
    STDMETHOD(removeOne(...)) PURE;
    STDMETHOD(snap(...)) PURE;
    STDMETHOD(close(...)) PURE;
    // ... и т.д.
};
```

#### IElement (Interface.h):
```cpp
interface IElement : IUnknown {
    STDMETHOD(createGetElement(...)) PURE;
    STDMETHOD(createInsertElement(...)) PURE;
    STDMETHOD(createUpdateElement(...)) PURE;
};
```

---

## 4. Реализация методов ComponentHT

### Пример: метод create

**COM-интерфейс (Interface.h):**
```cpp
STDMETHOD(create(ht::HtHandle** htHandle, 
                 int capacity, 
                 int secSnapshotInterval, 
                 int maxKeyLength, 
                 int maxPayloadLength, 
                 const wchar_t* fileName)) PURE;
```

**Реализация (ComponentHT.cpp):**
```cpp
STDMETHODIMP ComponentHT::create(ht::HtHandle** htHandle, 
                                 int capacity, 
                                 int secSnapshotInterval, 
                                 int maxKeyLength, 
                                 int maxPayloadLength, 
                                 const wchar_t* fileName)
{
    // ПРОСТО ВЫЗОВ БАЗОВОЙ ФУНКЦИИ ИЗ HT API!
    *htHandle = ht::create(capacity, secSnapshotInterval, 
                           maxKeyLength, maxPayloadLength, fileName);
    return S_OK;
}
```

**Важно:** ComponentHT - это просто "тонкая обертка" над HT API. Вся реальная работа выполняется в `ht::create()` из OS10_HTAPI.lib.

### Все методы работают аналогично:

```cpp
// insert
STDMETHODIMP ComponentHT::insert(BOOL& rc, ht::HtHandle* htHandle, 
                                 const ht::Element* element)
{
    rc = ht::insert(htHandle, element);  // ← Вызов HT API
    return S_OK;
}

// get
STDMETHODIMP ComponentHT::get(ht::Element** rcElement, 
                              ht::HtHandle* htHandle, 
                              const ht::Element* element)
{
    *rcElement = ht::get(htHandle, element);  // ← Вызов HT API
    return S_OK;
}

// и т.д. для всех методов
```

---

## 5. Управление жизненным циклом (Reference Counting)

### IUnknown методы:

#### AddRef (ComponentHT.cpp):
```cpp
ULONG ComponentHT::AddRef()
{
    // Атомарно увеличиваем счетчик ссылок
    return InterlockedIncrement(&m_cRef);
}
```

#### Release (ComponentHT.cpp):
```cpp
ULONG ComponentHT::Release()
{
    // Атомарно уменьшаем счетчик ссылок
    if (InterlockedDecrement(&m_cRef) == 0)
    {
        // Если счетчик = 0, удаляем компонент
        InterlockedDecrement(&g_cComponents);  // Уменьшаем глобальный счетчик
        delete this;  // ← САМОРАЗРУШЕНИЕ!
        return 0;
    }
    return m_cRef;
}
```

**Принцип:** Когда последняя ссылка на компонент освобождается, компонент удаляет сам себя.

---

## 6. Работа с клиентами через OS13_HTCOM_LIB

### Как клиент использует компонент:

#### Шаг 1: Инициализация (OS13_HTCOM_LIB.cpp)
```cpp
OS13_HTCOM_HANDEL OS13_HTCOM::Init() {
    IUnknown* pIUnknown = NULL;
    
    // 1. Инициализация COM библиотеки
    CoInitialize(NULL);
    
    // 2. СОЗДАНИЕ КОМПОНЕНТА через COM
    HRESULT hr0 = CoCreateInstance(
        CLSID_ComponentHT,        // CLSID из реестра
        NULL,                      // Нет агрегации
        CLSCTX_INPROC_SERVER,      // In-Process (DLL в том же процессе)
        IID_IUnknown,              // Запрашиваем базовый интерфейс
        (void**)&pIUnknown         // Получаем указатель
    );
    
    if (SUCCEEDED(hr0))
        return pIUnknown;  // Возвращаем "handle" компонента
    else
        throw (int)hr0;
}
```

#### Шаг 2: Использование интерфейса (OS13_HTCOM_LIB.cpp)
```cpp
ht::HtHandle* OS13_HTCOM::HT::create(OS13_HTCOM_HANDEL h, 
                                     int capacity, ...)
{
    ht::HtHandle** ht = new ht::HtHandle*;
    
    // 1. Получаем интерфейс IHT из компонента
    HRESULT hr0 = ((IUnknown*)h)->QueryInterface(IID_IHT, (void**)&pIHT);
    if (SUCCEEDED(hr0))
    {
        // 2. Вызываем метод через COM-интерфейс
        HRESULT hr1 = pIHT->create(ht, capacity, ...);
        
        // 3. Освобождаем интерфейс
        pIHT->Release();
        
        return *ht;
    }
    else
        throw (int)hr0;
}
```

#### Шаг 3: Освобождение (OS13_HTCOM_LIB.cpp)
```cpp
void OS13_HTCOM::Dispose(OS13_HTCOM_HANDEL h) {
    // Освобождаем компонент (уменьшаем счетчик ссылок)
    ((IUnknown*)h)->Release();
    
    // Освобождаем неиспользуемые библиотеки
    CoFreeUnusedLibraries();
    
    // Деинициализация COM
    CoUninitialize();
}
```

---

## 7. Полный цикл вызова метода

**Пример:** Клиент вызывает `OS13_HTCOM::HT::create()`

```
1. OS13_CREATE.exe
   ↓
2. OS13_HTCOM::HT::create(h, ...)
   ↓
3. ((IUnknown*)h)->QueryInterface(IID_IHT, ...)
   ↓
4. ComponentHT::QueryInterface(IID_IHT, ...)
   ↓
5. Возвращает указатель на IHT
   ↓
6. pIHT->create(htHandle, ...)
   ↓
7. ComponentHT::create(...)
   ↓
8. ht::create(...)  ← ВЫЗОВ БАЗОВОЙ ФУНКЦИИ HT API
   ↓
9. OS10_HTAPI.lib выполняет работу
   ↓
10. Возврат результата обратно через все уровни
```

---

## 8. Счетчики и управление памятью

### Глобальные счетчики (dllmain.cpp):
```cpp
long g_cComponents = 0;   // Количество активных компонентов
long g_cServerLocks = 0;  // Счетчик блокировок сервера
```

### Когда увеличиваются:

**g_cComponents:**
- В конструкторе `ComponentHT()`: `InterlockedIncrement(&g_cComponents)`
- Уменьшается в деструкторе `~ComponentHT()`: `InterlockedDecrement(&g_cComponents)`

**g_cServerLocks:**
- В `ClassFactory::LockServer(TRUE)`: `InterlockedIncrement(&g_cServerLocks)`
- Уменьшается в `ClassFactory::LockServer(FALSE)`

### DllCanUnloadNow (ExportFunctions.cpp):
```cpp
STDAPI DllCanUnloadNow()
{
    // DLL можно выгрузить только если:
    // 1. Нет активных компонентов (g_cComponents == 0)
    // 2. Нет блокировок сервера (g_cServerLocks == 0)
    if ((g_cComponents == 0) && (g_cServerLocks == 0))
        return S_OK;  // Можно выгружать
    else
        return S_FALSE;  // Нельзя выгружать
}
```

---

## 9. Связь с базовой библиотекой

### Линковка (ComponentHT.cpp):
```cpp
#ifdef _WIN64
#pragma comment(lib, "../../lab10/x64/Debug/OS10_HTAPI.lib")
#else
#pragma comment(lib, "../../lab10/Debug/OS10_HTAPI.lib")
#endif
```

**Это означает:**
- OS13_HTCOM.dll **статически** линкуется с OS10_HTAPI.lib
- Все функции HT API доступны напрямую в DLL
- Нет необходимости в дополнительных DLL для HT API

### Включение заголовков (ComponentHT.cpp):
```cpp
#include "../../lab10/OS10_HTAPI/pch.h"
#include "../../lab10/OS10_HTAPI/HT.h"
```

---

## 10. Преимущества COM-обертки

### 1. **Межпроцессное взаимодействие**
   - Несколько процессов могут использовать один компонент
   - Каждый процесс получает свой экземпляр компонента

### 2. **Языковая независимость**
   - COM-интерфейсы можно использовать из разных языков
   - C++, C#, VB, Python и т.д.

### 3. **Версионность**
   - Интерфейсы имеют GUID, что позволяет версионировать API
   - Можно добавлять новые интерфейсы без нарушения совместимости

### 4. **Управление памятью**
   - Автоматическое управление через reference counting
   - Компонент удаляет сам себя, когда не нужен

### 5. **Регистрация в системе**
   - Компонент регистрируется один раз
   - Любое приложение может его использовать

---

## 11. Отличия от прямого использования HT API

### Прямое использование (lab10):
```cpp
// Статическая линковка
ht::HtHandle* ht = ht::create(...);
ht::insert(ht, element);
```

### Использование через COM (lab4-ht-com-dll):
```cpp
// Динамическое создание через COM
OS13_HTCOM_HANDEL h = OS13_HTCOM::Init();
ht::HtHandle* ht = OS13_HTCOM::HT::create(h, ...);
OS13_HTCOM::HT::insert(h, ht, element);
OS13_HTCOM::Dispose(h);
```

**Разница:**
- COM: компонент создается во время выполнения
- Прямое: код линкуется на этапе компиляции
- COM: можно заменить DLL без перекомпиляции клиента
- Прямое: нужно перекомпилировать при изменении API

---

## 12. Диаграмма взаимодействия

```
┌─────────────────────────────────────────┐
│         Клиент (OS13_CREATE.exe)        │
└──────────────┬──────────────────────────┘
               │
               │ CoCreateInstance(CLSID_ComponentHT, ...)
               ↓
┌─────────────────────────────────────────┐
│         Windows COM Runtime              │
│  - Ищет CLSID в реестре                  │
│  - Загружает OS13_HTCOM.dll              │
│  - Вызывает DllGetClassObject            │
└──────────────┬──────────────────────────┘
               │
               │ DllGetClassObject(...)
               ↓
┌─────────────────────────────────────────┐
│      OS13_HTCOM.dll                      │
│                                          │
│  DllGetClassObject                       │
│    → new ClassFactory                    │
│    → ClassFactory::CreateInstance        │
│      → new ComponentHT                   │
│      → ComponentHT::QueryInterface       │
│        → return IHT*                     │
└──────────────┬──────────────────────────┘
               │
               │ pIHT->create(...)
               ↓
┌─────────────────────────────────────────┐
│      ComponentHT::create(...)            │
│    → ht::create(...)                    │
└──────────────┬──────────────────────────┘
               │
               │ Вызов HT API
               ↓
┌─────────────────────────────────────────┐
│      OS10_HTAPI.lib                     │
│  - Создает File Mapping                 │
│  - Инициализирует хеш-таблицу            │
│  - Возвращает HtHandle*                  │
└─────────────────────────────────────────┘
```

---

## 13. Ключевые моменты

1. **ComponentHT - это обертка:** Все методы просто вызывают функции из HT API
2. **COM обеспечивает стандартизацию:** Единый способ доступа к функциональности
3. **Reference counting:** Автоматическое управление памятью
4. **In-Process Server:** DLL загружается в процесс клиента
5. **Регистрация обязательна:** Без регистрации Windows не найдет компонент
6. **Статическая линковка HT API:** OS13_HTCOM.dll содержит код HT API внутри себя

---

## 14. Отладка и мониторинг

### Вывод в консоль (для отладки):
```cpp
std::cout << "DllGetClassObject:\tСоздать фабрику класса" << std::endl;
std::cout << "Фабрика класса:\t\tСоздать компонент" << std::endl;
std::cout << "Компонент:\t\tВернуть указатель на IHT" << std::endl;
```

Эти сообщения помогают отслеживать жизненный цикл компонента.

---

## Заключение

COM-обертка OS13_HTCOM.dll - это **стандартизированный способ доступа** к функциональности HT API через COM-интерфейсы. Она не добавляет новой функциональности, а просто предоставляет существующий API в формате, совместимом с COM, что позволяет:

- Использовать HT API из разных процессов
- Легко заменять реализацию без изменения клиентов
- Управлять памятью автоматически
- Интегрироваться с другими COM-компонентами

