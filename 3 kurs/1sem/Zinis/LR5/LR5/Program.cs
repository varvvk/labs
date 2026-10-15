using System;
using System.Collections.Generic;
using System.Linq;

namespace ParityCode2D
{
    class Program
    {
        static Random rand = new Random();

        static void Main()
        {
            const int rows = 5;
            const int cols = 8;
            const int k = rows * cols;

            Console.WriteLine("Выберите способ ввода сообщения:");
            Console.WriteLine("1 - Ввести вручную");
            Console.WriteLine("2 - Сгенерировать случайное 40-битное сообщение");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine()?.Trim();

            string input;
            if (choice == "2")
            {
                input = GenerateRandomBits(k);
                Console.WriteLine($"\nСгенерированное сообщение ({k} бит):");
                Console.WriteLine(input);
            }
            else
            {
                Console.WriteLine($"\nВведите двоичное слово длиной {k} бит:");
                input = Console.ReadLine()?.Trim();

                while (input == null || input.Length != k || !input.All(c => c == '0' || c == '1'))
                {
                    Console.WriteLine($"Ошибка! Введите {k} бит (0/1):");
                    input = Console.ReadLine()?.Trim();
                }
            }

            // === 1. Исходная матрица ===
            int[,] matrix = new int[rows, cols];
            int idx = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = input[idx++] - '0';

            Console.WriteLine("\nИсходная матрица Xk:");
            PrintMatrix(matrix);

            // === 2. Эталонные паритеты ===
            int[] rowParity = CalcRowParity(matrix, rows, cols);
            int[] colParity = CalcColParity(matrix, rows, cols);

            Console.WriteLine("\nЭталонные паритеты:");
            Console.WriteLine("Строки:  " + string.Join("", rowParity));
            Console.WriteLine("Столбцы: " + string.Join("", colParity));

            // === 3. Кодовое слово Xn ===
            string codeword = input + string.Join("", rowParity) + string.Join("", colParity);
            Console.WriteLine($"\nКодовое слово Xn = {codeword}");

            // === 4. Генерация ошибок ===
            Console.Write("\nВведите количество ошибок для моделирования: ");
            if (!int.TryParse(Console.ReadLine(), out int errorCount) || errorCount < 1)
                errorCount = 1;

            char[] corrupted = codeword.ToCharArray();
            var flippedPositions = new HashSet<int>();
            for (int e = 0; e < errorCount; e++)
            {
                int pos;
                do
                {
                    pos = rand.Next(corrupted.Length);
                } while (flippedPositions.Contains(pos) && flippedPositions.Count < corrupted.Length);
                flippedPositions.Add(pos);

                corrupted[pos] = corrupted[pos] == '0' ? '1' : '0';
            }

            string Yn = new string(corrupted);
            Console.WriteLine($"\nСлово с ошибками Yn = {Yn}");
            Console.WriteLine("Позиции перевёрнутых битов (в кодовом слове): " + string.Join(", ", flippedPositions.OrderBy(x => x)));

            // === 5. Матрица с ошибками ===
            int[,] corruptedMatrix = new int[rows, cols];
            idx = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    corruptedMatrix[i, j] = Yn[idx++] - '0';

            Console.WriteLine("\nМатрица с ошибками (Yn -> первые 40 бит):");
            PrintMatrix(corruptedMatrix);

            // === 6. Паритеты для Yn ===
            int[] rowCheck = CalcRowParity(corruptedMatrix, rows, cols);
            int[] colCheck = CalcColParity(corruptedMatrix, rows, cols);

            Console.WriteLine("\nПаритеты для Yn (после искажения):");
            Console.WriteLine("Строки:  " + string.Join("", rowCheck));
            Console.WriteLine("Столбцы: " + string.Join("", colCheck));

            // === 7. Сравнение паритетов ===
            var rowMismatch = new List<int>();
            var colMismatch = new List<int>();

            for (int i = 0; i < rows; i++)
                if (rowCheck[i] != rowParity[i]) rowMismatch.Add(i);

            for (int j = 0; j < cols; j++)
                if (colCheck[j] != colParity[j]) colMismatch.Add(j);

            Console.WriteLine("\nНайденные несовпадения:");
            Console.WriteLine("Несовпадающие строки: " + (rowMismatch.Count == 0 ? "-" : string.Join(", ", rowMismatch)));
            Console.WriteLine("Несовпадающие столбцы: " + (colMismatch.Count == 0 ? "-" : string.Join(", ", colMismatch)));

            // === 8. Поиск ошибок ===
            var foundPositions = new List<(int row, int col)>();
            foreach (var r in rowMismatch)
                foreach (var c in colMismatch)
                    foundPositions.Add((r, c));

            if (foundPositions.Count == 0)
            {
                Console.WriteLine("\nОшибки не обнаружены (или они повлияли только на контрольные биты).");
                Console.WriteLine("Матрица остаётся без изменений:");
                PrintMatrix(corruptedMatrix);
            }
            else
            {
                Console.WriteLine("\nПересечения (возможные позиции ошибок):");
                foreach (var p in foundPositions)
                    Console.WriteLine($"  строка {p.row}, столбец {p.col} (линейный индекс = {p.row * cols + p.col})");

                if (foundPositions.Count == 1)
                {
                    var p = foundPositions[0];
                    Console.WriteLine($"\nАвтоисправление: строка {p.row}, столбец {p.col}");
                    corruptedMatrix[p.row, p.col] ^= 1;

                    Console.WriteLine("\nМатрица после исправления (Yn'):");
                    PrintMatrix(corruptedMatrix);
                }
                else
                {
                    Console.WriteLine("\nНесколько пересечений — неоднозначно. Автоисправление не выполняется.");
                    Console.WriteLine("Матрица остаётся без изменений:");
                    PrintMatrix(corruptedMatrix);
                }
            }

            // === 9. Проверка результата ===
            string corrected = string.Concat(Enumerable.Range(0, rows)
                .SelectMany(i => Enumerable.Range(0, cols).Select(j => corruptedMatrix[i, j].ToString())));

            bool equal = corrected == input;
            Console.WriteLine($"\nВосстановленное информационное слово: {corrected}");
            Console.WriteLine(equal ? "Исходное слово восстановлено." : "Исходное слово НЕ восстановлено.");

            var flippedInInfo = flippedPositions.Where(p => p < k).OrderBy(x => x).ToList();
            if (flippedInInfo.Count > 0)
                Console.WriteLine("Перевёрнутые позиции в информационной части (линейные индексы): " + string.Join(", ", flippedInInfo));
            else
                Console.WriteLine("Перевёрнутых битов в информационной части не было (ошибки только в контрольных битах).");



            //ЗАДАНИЕ 6: АНАЛИЗ КОРРЕКТИРУЮЩЕЙ СПОСОБНОСТИ
            Console.WriteLine("\nАНАЛИЗ КОРРЕКТИРУЮЩЕЙ СПОСОБНОСТИ КОДА");

            Console.Write("Введите количество тестовых кодовых слов N для каждой кратности: ");
            if (!int.TryParse(Console.ReadLine(), out int N) || N <= 0) N = 1000;

            Console.Write("Введите максимальную кратность ошибок для анализа (например, 5): ");
            if (!int.TryParse(Console.ReadLine(), out int maxErrors) || maxErrors < 1) maxErrors = 3;

            Console.WriteLine($"\nЗапуск анализа: N = {N}, кратности ошибок от 1 до {maxErrors}...\n");

            var results = new List<(int multiplicity, int N1, int N2, int N3)>();

            for (int errMult = 1; errMult <= maxErrors; errMult++)
            {
                int N1 = N;
                int N2 = 0;
                int N3 = 0;

                for (int test = 0; test < N; test++)
                {
                    // 1. Генерация случайного сообщения
                    string msg = GenerateRandomBits(k);
                    int[,] testMatrix = new int[rows, cols];
                    int idx2 = 0;
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            testMatrix[i, j] = msg[idx2++] - '0';

                    // 2. Эталонные паритеты
                    int[] testRowParity = CalcRowParity(testMatrix, rows, cols);
                    int[] testColParity = CalcColParity(testMatrix, rows, cols);
                    string Xn = msg + string.Join("", testRowParity) + string.Join("", testColParity);

                    // 3. Внесение errMult ошибок
                    char[] testCorrupted = Xn.ToCharArray();
                    var testFlipped = new HashSet<int>();
                    while (testFlipped.Count < errMult)
                    {
                        int pos = rand.Next(testCorrupted.Length);
                        if (testFlipped.Add(pos))
                            testCorrupted[pos] = testCorrupted[pos] == '0' ? '1' : '0';
                    }
                    string testYn = new string(testCorrupted);

                    // 4. Декодирование
                    int[,] testCorrMatrix = new int[rows, cols];
                    idx2 = 0;
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            testCorrMatrix[i, j] = testYn[idx2++] - '0';

                    int[] testRowCheck = CalcRowParity(testCorrMatrix, rows, cols);
                    int[] testColCheck = CalcColParity(testCorrMatrix, rows, cols);

                    var testRowMismatch = new List<int>();
                    var testColMismatch = new List<int>();
                    for (int i = 0; i < rows; i++)
                        if (testRowCheck[i] != testRowParity[i]) testRowMismatch.Add(i);
                    for (int j = 0; j < cols; j++)
                        if (testColCheck[j] != testColParity[j]) testColMismatch.Add(j);

                    var testFoundPositions = new List<(int r, int c)>();
                    foreach (var r in testRowMismatch)
                        foreach (var c in testColMismatch)
                            testFoundPositions.Add((r, c));

                    testFoundPositions = testFoundPositions.Distinct().ToList();

                    string correctedInfo = "";
                    bool correctedFully = false;

                    if (testFoundPositions.Count == 1 && errMult == 1)
                    {
                        var (r, c) = testFoundPositions[0];
                        testCorrMatrix[r, c] ^= 1;
                        correctedInfo = string.Concat(
                            Enumerable.Range(0, rows)
                                      .SelectMany(i => Enumerable.Range(0, cols)
                                                        .Select(j => testCorrMatrix[i, j].ToString())));
                        correctedFully = correctedInfo == msg;
                    }
                    else
                    {
                        correctedInfo = string.Concat(
                            Enumerable.Range(0, rows)
                                      .SelectMany(i => Enumerable.Range(0, cols)
                                                        .Select(j => testCorrMatrix[i, j].ToString())));
                        correctedFully = correctedInfo == msg;
                    }

                    // Статистика
                    if (testFoundPositions.Count == errMult) N2++;
                    if (correctedFully) N3++;
                }

                results.Add((errMult, N1, N2, N3));
            }

            // === ВЫВОД РЕЗУЛЬТАТОВ АНАЛИЗА ===
            Console.WriteLine("\nРЕЗУЛЬТАТЫ АНАЛИЗА КОРРЕКТИРУЮЩЕЙ СПОСОБНОСТИ:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"Кратность",-10} {"N1",-8} {"N2",-8} {"N3",-8} {"P_обн",-10} {"P_исп",-10}");
            Console.WriteLine(new string('-', 80));

            foreach (var (mult, N1, N2, N3) in results)
            {
                double P_detect = (double)N2 / N1;
                double P_correct = (double)N3 / N1;
                Console.WriteLine($"{mult,-10} {N1,-8} {N2,-8} {N3,-8} {P_detect,-10:F6} {P_correct,-10:F6}");
            }

            Console.WriteLine(new string('-', 80));
            Console.WriteLine("P_обн = N2/N1 — вероятность правильного обнаружения кратности");
            Console.WriteLine("P_исп = N3/N1 — вероятность полного исправления (Xn == Yn')");
        }

        // ==== ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ====

        static string GenerateRandomBits(int length)
        {
            return string.Concat(Enumerable.Range(0, length).Select(_ => rand.Next(2).ToString()));
        }

        static int[] CalcRowParity(int[,] matrix, int rows, int cols)
        {
            int[] parity = new int[rows];
            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                for (int j = 0; j < cols; j++)
                    sum += matrix[i, j];
                parity[i] = sum % 2;
            }
            return parity;
        }

        static int[] CalcColParity(int[,] matrix, int rows, int cols)
        {
            int[] parity = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                int sum = 0;
                for (int i = 0; i < rows; i++)
                    sum += matrix[i, j];
                parity[j] = sum % 2;
            }
            return parity;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j]);
                Console.WriteLine();
            }
        }
    }
}