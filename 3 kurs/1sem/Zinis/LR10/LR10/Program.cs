using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LZ77Lab10
{
    class Program
    {
        static void Main(string[] args)
        {
            // Исходное сообщение из задания
            string pattern = "2000302013020130313031303130313333";  // 34 символа
            string noise = "123021301230123";                       // 15 символов шума

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 65; i++)
            {
                sb.Append(pattern);
                if (i % 5 == 0) sb.Append(noise);  // каждые 5 блоков добавляем шум
            }
            sb.Append("2000302013020130313031303130313333333");  // финальный кусок

            string original = sb.ToString();  // ≈ 65 × 34 + 13 × 15 + 37 ≈ **2450–2500 символов**

            // Список конфигураций (n1 - словарь, n2 - буфер)
            var configurations = new List<(int dictSize, int bufSize)>
            {
                (15, 13),  // Исходная из примера
                (7, 7),    // Меньшие окна
                (31, 17),  // Большие окна
                (63, 25)   // Ещё большие
            };

            Console.WriteLine($"Исходное сообщение:\n{original}");
            Console.WriteLine($"Длина: {original.Length} символов");
            int vDc = original.Length * 2;  // V_дс в битах (2 бита на символ, алфавит N=4)
            Console.WriteLine($"Объём исходных данных V_дс: {vDc} бит\n");

            // Таблица результатов
            Console.WriteLine("Оценка для разных размеров окон:");
            Console.WriteLine(" n1 | n2 | Триад | l(c) | V_пс (бит) | R1 (%) | R2 (%) | Время сжатия (мс) | Время распаковки (мс)");
            Console.WriteLine("---------------------------------------------------------------------------------------");

            foreach (var (dictSize, bufSize) in configurations)
            {
                // Измерение времени сжатия
                Stopwatch swCompress = Stopwatch.StartNew();
                var compressed = Compress(original, dictSize, bufSize);
                swCompress.Stop();

                // Измерение времени распаковки
                Stopwatch swDecompress = Stopwatch.StartNew();
                string decompressed = Decompress(compressed, dictSize);
                swDecompress.Stop();

                // Расчёт l(c) = ceil(log4(n1+1)) + ceil(log4(n2+1)) + 1 (четверичных цифр)
                int logN = 4;  // основание (N=4)
                int lC = (int)Math.Ceiling(Math.Log(dictSize + 1, logN)) +
                         (int)Math.Ceiling(Math.Log(bufSize + 1, logN)) + 1;

                // V_пс = количество триад * l(c) * 2 бита (каждая 4-ричная цифра = 2 бита)
                int numTriads = compressed.Count;
                int vPc = numTriads * lC * 2;

                // R1 = (V_пс / V_дс) * 100%
                double r1 = (vPc / (double)vDc) * 100;

                // R2 = (V_дс - V_пс) / V_дс * 100% = (1 - R1/100) * 100%
                double r2 = (1 - r1 / 100) * 100;

                // Вывод строки таблицы
                Console.WriteLine($" {dictSize,2} | {bufSize,2} | {numTriads,5} | {lC,4} | {vPc,10} | {r1,6:F2} | {r2,6:F2} | {swCompress.Elapsed.TotalMilliseconds,17:F6} | {swDecompress.Elapsed.TotalMilliseconds,21:F6}");

                // Проверка корректности (для отладки)
                if (decompressed != original)
                {
                    Console.WriteLine("   ОШИБКА: Распаковка не совпадает с оригиналом!");
                }
            }

            // Детальный вывод для первой конфигурации (как раньше)
            var firstConfig = configurations[0];
            Console.WriteLine($"\n\nДетальный вывод для n1={firstConfig.dictSize}, n2={firstConfig.bufSize}:");
            var compressedFirst = Compress(original, firstConfig.dictSize, firstConfig.bufSize, true);  // Включаем verbose

            Console.WriteLine("Результат сжатия (триады в четверичной системе):");
            Console.WriteLine("   p     q     s     (p и q — в 4-ричной системе)");
            Console.WriteLine("------------------------------------------------");

            foreach (var triad in compressedFirst)
            {
                string p4 = ToBase4(triad.p);
                string q4 = ToBase4(triad.q);
                string sDisplay = triad.s == '\0' ? "(end)" : triad.s.ToString();
                Console.WriteLine($"   {p4,4}  {q4,4}   {sDisplay}");
            }

            Console.WriteLine($"\nКоличество триад: {compressedFirst.Count}");

            string decompressedFirst = Decompress(compressedFirst, firstConfig.dictSize, true);  // Включаем verbose

            Console.WriteLine("Восстановленное сообщение:");
            Console.WriteLine(decompressedFirst);

            Console.WriteLine(decompressedFirst == original
                ? "\nВосстановление выполнено УСПЕШНО"
                : "\nОШИБКА ВОССТАНОВЛЕНИЯ!");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Преобразование числа в четверичную систему счисления
        static string ToBase4(int number)
        {
            if (number == 0) return "0";
            StringBuilder sb = new StringBuilder();
            while (number > 0)
            {
                sb.Insert(0, number % 4);
                number /= 4;
            }
            return sb.ToString();
        }

        static List<(int p, int q, char s)> Compress(string input, int dictSize, int bufSize, bool verbose = false)
        {
            var result = new List<(int p, int q, char s)>();

            StringBuilder dictionary = new StringBuilder(new string('0', dictSize));
            int pos = 0;
            int step = 0;

            if (verbose)
            {
                Console.WriteLine("\nШаги сжатия (первые 5 шагов):");
            }

            while (pos < input.Length)
            {
                if (verbose && step < 5)
                {
                    Console.WriteLine($"\nШаг {step + 1}:");
                    Console.WriteLine($"Позиция: {pos}");
                    Console.WriteLine($"Словарь: {dictionary}");
                }

                int remaining = input.Length - pos;
                int lookAheadLen = Math.Min(bufSize, remaining);
                string lookAhead = input.Substring(pos, lookAheadLen);

                if (verbose && step < 5)
                {
                    Console.WriteLine($"Буфер просмотра: {lookAhead}");
                }

                int bestLength = 0;
                int bestOffset = 0;  // 1-based

                // Поиск самого длинного совпадения
                for (int i = 0; i < dictSize; i++)
                {
                    int matchLen = 0;
                    while (matchLen < lookAheadLen &&
                           i + matchLen < dictSize &&
                           dictionary[i + matchLen] == lookAhead[matchLen])
                    {
                        matchLen++;
                    }

                    if (matchLen > bestLength)
                    {
                        bestLength = matchLen;
                        bestOffset = i + 1;
                    }
                }

                char nextChar = '\0';
                int advance = bestLength;

                if (bestLength < lookAheadLen)
                {
                    nextChar = lookAhead[bestLength];
                    advance++;  // +1 за новый символ
                }
                // иначе - конец строки, нового символа нет

                if (verbose && step < 5)
                {
                    Console.WriteLine($"Найденное совпадение: p={bestOffset}, q={bestLength}, s={(nextChar == '\0' ? "нет" : nextChar.ToString())}");
                }

                result.Add((bestOffset, bestLength, nextChar));

                // Что добавляем в словарь
                string toAdd = lookAhead.Substring(0, bestLength);
                if (nextChar != '\0')
                    toAdd += nextChar;

                dictionary.Append(toAdd);
                if (dictionary.Length > dictSize)
                    dictionary.Remove(0, dictionary.Length - dictSize);

                if (verbose && step < 5)
                {
                    Console.WriteLine($"Обновленный словарь: {dictionary}");
                }

                pos += advance;
                step++;
            }

            return result;
        }

        static string Decompress(List<(int p, int q, char s)> compressed, int dictSize, bool verbose = false)
        {
            var output = new StringBuilder();
            var dictionary = new StringBuilder(new string('0', dictSize));
            int step = 0;

            if (verbose)
            {
                Console.WriteLine("\nШаги распаковки (первые 5 шагов):");
            }

            foreach (var (p, q, s) in compressed)
            {
                if (verbose && step < 5)
                {
                    Console.WriteLine($"\nШаг {step + 1}:");
                    Console.WriteLine($"Триада: p={p}, q={q}, s={(s == '\0' ? "нет" : s.ToString())}");
                    Console.WriteLine($"Словарь: {dictionary}");
                }

                string matched = "";
                if (p > 0 && q > 0)
                {
                    int start = p - 1;
                    if (start + q <= dictionary.Length)
                        matched = dictionary.ToString().Substring(start, q);
                }

                if (verbose && step < 5)
                {
                    Console.WriteLine($"Совпадающая строка: {matched}");
                }

                string toAdd = matched;
                if (s != '\0')  // добавляем только существующий символ
                    toAdd += s;

                output.Append(toAdd);

                dictionary.Append(toAdd);
                if (dictionary.Length > dictSize)
                    dictionary.Remove(0, dictionary.Length - dictSize);

                if (verbose && step < 5)
                {
                    Console.WriteLine($"Добавлено в вывод: {toAdd}");
                    Console.WriteLine($"Обновленный словарь: {dictionary}");
                    Console.WriteLine($"Текущий вывод: {output}");
                }

                step++;
            }

            return output.ToString();
        }
    }
}