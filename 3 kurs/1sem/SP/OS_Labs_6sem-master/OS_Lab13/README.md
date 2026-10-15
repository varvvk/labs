## Как запустить лабу:
1. Собрать OS13_HTCOM
2. Запустить коммандную строку от имени администратора
3. cd C:\labs\SP\OS_Labs_6sem-master\OS_Lab13\lab4-ht-com-dll\x64\Debug
4. regsvr32 OS13_HTCOM.dll
5. Проверить в реестре: Компьютер\HKEY_CLASSES_ROOT\OS13.ComponentHT
6. Пересобрать OS13_HTCOM
7. Далее можно запускать OS13_START и параллельно проекты OS_04
 cd C:\labs\SP\OS_Labs_6sem-master\OS_Lab13\lab4-ht-com-dll\x64\Debug
OS13_CREATE.exe 2000 3 4 4 "HTspace.ht"
OS13_START.exe HTspace.ht