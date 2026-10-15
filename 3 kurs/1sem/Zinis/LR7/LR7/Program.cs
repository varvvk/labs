using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class InterleaverWithCyclicCode
{
    const int COLUMNS = 6;
    const int MESSAGE_LENGTH_BYTES = 15;
    const int INFO_WORD_LENGTH = 6;
    const int CODEWORD_LENGTH = 10;
    const int TOTAL_BITS = 120;
    const int NUM_INFO_WORDS = 12;

    // Порождающий полином для циклического кода: x^4 + x + 1 = 10011
    static readonly int[] gCoeffs = new int[] { 1, 0, 0, 1, 1 }; // 10011

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // 1. Генерация 12 информационных слов
        bool[][] infoWords = GenerateInfoWords();
        Console.WriteLine("1. Сгенерированные 12 информационных слов:");
        PrintInfoWords(infoWords);
        Console.WriteLine();

        // 2. Кодирование циклическим кодом
        bool[][] encodedWords = EncodeWords(infoWords);
        Console.WriteLine("2. Закодированные слова:");
        PrintEncodedWords(encodedWords);
        Console.WriteLine();

        // 3. Формирование полного сообщения (120 бит)
        bool[] fullMessage = FormFullMessage(encodedWords);
        Console.WriteLine("3. Полное сообщение (120 бит):");
        PrintBitArray(fullMessage, 10);
        Console.WriteLine();

        // 4. Перемежение (матричный интерливинг по битам)
        bool[,] interleaveMatrix = CreateInterleaveMatrix(fullMessage);
        Console.WriteLine("4. Матрица перемежения:");
        PrintInterleaveMatrix(interleaveMatrix);

        bool[] interleavedMessage = Interleave(interleaveMatrix);
        Console.WriteLine("Сообщение после перемежения:");
        PrintBitArray(interleavedMessage, 10);
        Console.WriteLine();

        // --- Запускаем блок 5–8 для errorLength = 4, 5, 7 ---
        int[] errorLengths = { 4, 5, 7 };

        foreach (int errorLength in errorLengths)
        {
            Console.WriteLine();
            Console.WriteLine($"      ЗАПУСК ДЛЯ ДЛИНЫ ОШИБОК = {errorLength}");
            Console.WriteLine();

            // 5. Внесение пакета ошибок
            Random rnd = new Random();
            int errorStart = rnd.Next(0, interleavedMessage.Length - errorLength);
            bool[] messageWithErrors = IntroduceErrors(interleavedMessage, errorStart, errorLength);
            Console.WriteLine($"5. Сообщение с пакетом ошибок (с позиции {errorStart}, длина {errorLength}):");
            PrintBitArrayWithErrors(messageWithErrors, interleavedMessage, 10);
            Console.WriteLine();

            // 6. Деперемежение
            bool[,] deinterleaveMatrix = CreateDeinterleaveMatrix(messageWithErrors);
            Console.WriteLine("6. Матрица деперемежения:");
            PrintDeinterleaveMatrix(deinterleaveMatrix);

            bool[] deinterleavedMessage = Deinterleave(deinterleaveMatrix);
            Console.WriteLine("Сообщение после деперемежения:");
            PrintBitArray(deinterleavedMessage, 10);
            Console.WriteLine();

            // 7. Сравнение исходного и деперемеженного сообщения
            Console.WriteLine("7. Сравнение исходного и деперемеженного сообщения:");
            Console.WriteLine("Исходное:  " + GroupBits(fullMessage, 10));
            Console.WriteLine("После деп.:" + GroupBits(deinterleavedMessage, 10));
            Console.WriteLine();

            // 8. Декодирование и исправление ошибок
            Console.WriteLine("8. Декодирование и исправление ошибок:\n");

            bool[][] receivedWords = SplitIntoWords(deinterleavedMessage);
            bool[][] referenceWords = SplitIntoWords(fullMessage);

            bool[][] singleErrorSyndromes = BuildSingleErrorSyndromes(CODEWORD_LENGTH, gCoeffs);

            bool[][] correctedWords = new bool[receivedWords.Length][];

            for (int idx = 0; idx < receivedWords.Length; idx++)
            {
                bool[] Yn = (bool[])receivedWords[idx].Clone();
                bool[] Xn = referenceWords[idx];

                Console.WriteLine($"Обработка словa #{idx + 1}:");
                Console.WriteLine($"Получено:  {BitArrayToString(Yn)}");
                Console.WriteLine($"Эталон:    {BitArrayToString(Xn)}");

                bool[] syndrome = CalculateParityBits(Yn, gCoeffs);
                Console.WriteLine($"Синдром (остаток Yn/g): {BoolArrayToString(syndrome)}");

                if (IsAllZero(syndrome))
                {
                    Console.WriteLine("Синдром = 0 -> ошибок не обнаружено.\n");
                    correctedWords[idx] = Yn;
                    continue;
                }

                int foundPos = FindSingleErrorPosition(syndrome, singleErrorSyndromes);

                if (foundPos >= 0)
                {
                    Console.WriteLine($"Найдена и исправлена одиночная ошибка.");
                    Yn[foundPos] = !Yn[foundPos];
                    Console.WriteLine($"Исправленное слово: {BitArrayToString(Yn)}\n");
                }
                else
                {
                    var pair = FindDoubleErrorPositions(syndrome, singleErrorSyndromes);
                    if (pair.Item1 >= 0)
                    {
                        Console.WriteLine($"Обнаружено 2 ошибки. Исправление невозможно.\n");
                    }
                    else
                    {
                        Console.WriteLine("Синдром не совпадает ни с одним столбцом и ни с XOR пары столбцов -> ≥3 ошибок. Исправление невозможно.\n");
                    }
                }

                correctedWords[idx] = Yn;
            }

            bool[] correctedMessage = FormFullMessage(correctedWords);
            bool[] originalMessage = FormFullMessage(referenceWords);

            Console.WriteLine("Исправленное полное сообщение (по 10 бит):");
            PrintBitArray(correctedMessage, 10);
            Console.WriteLine("Эталонное полное сообщение (по 10 бит):");
            PrintBitArray(originalMessage, 10);
            Console.WriteLine();

            bool equal = correctedMessage.SequenceEqual(originalMessage);
            Console.WriteLine(equal ? "Результат: Ошибки исправлены успешно! Полное сообщение совпадает с эталоном."
                                   : "Результат: Есть неисправлённые ошибки — полное сообщение НЕ совпадает с эталоном.");

            RunStatisticsManyTimes(fullMessage, interleavedMessage, errorLength);
        }
    }

    // 1. Генерация информационных слов
    static bool[][] GenerateInfoWords()
    {
        bool[][] words = new bool[NUM_INFO_WORDS][];
        Random rnd = new Random(42); // Фиксированный seed для воспроизводимости

        for (int i = 0; i < NUM_INFO_WORDS; i++)
        {
            words[i] = new bool[INFO_WORD_LENGTH];
            for (int j = 0; j < INFO_WORD_LENGTH; j++)
            {
                words[i][j] = rnd.Next(2) == 1;
            }
        }

        return words;
    }

    // 2. Кодирование циклическим кодом (реальное деление полиномов)
    static bool[][] EncodeWords(bool[][] infoWords)
    {
        int r = gCoeffs.Length - 1; // степень g
        bool[][] encodedWords = new bool[NUM_INFO_WORDS][];

        for (int i = 0; i < NUM_INFO_WORDS; i++)
        {
            // Создаем полином: infoWord * x^r (в виде массива длины n = k + r)
            bool[] shifted = new bool[CODEWORD_LENGTH]; // длина n
            for (int j = 0; j < INFO_WORD_LENGTH; j++)
                shifted[j] = infoWords[i][j];
            for (int j = INFO_WORD_LENGTH; j < CODEWORD_LENGTH; j++)
                shifted[j] = false; // x^r — добавленные нули в конце

            // Вычисляем проверочные биты через деление полиномов
            bool[] parityBits = CalculateParityBits(shifted, gCoeffs);

            // Формируем кодовое слово Xn = Xk || R
            encodedWords[i] = new bool[CODEWORD_LENGTH];
            for (int j = 0; j < INFO_WORD_LENGTH; j++)
                encodedWords[i][j] = infoWords[i][j];
            for (int j = 0; j < parityBits.Length; j++)
                encodedWords[i][INFO_WORD_LENGTH + j] = parityBits[j];

            // Дополнительный диагностический вывод, аналогичный примеру
            Console.WriteLine($"Информационное слово {i + 1}: {BitArrayToString(infoWords[i])}");
            Console.WriteLine($"Xk*X^r: {BoolArrayToString(shifted)}");
            Console.WriteLine($"Остаток R после деления на g(X): {BoolArrayToString(parityBits)}");
            Console.WriteLine($"Кодовое слово Xn: {BitArrayToString(encodedWords[i])}");
            Console.WriteLine();
        }

        return encodedWords;
    }

    // Реальное вычисление проверочных битов: делим dividend (длина n, старший слева) на g (старший слева), возвращаем остаток длины r
    static bool[] CalculateParityBits(bool[] dividendBool, int[] g)
    {
        int n = dividendBool.Length;
        int degG = g.Length - 1; // степень g (r)
        int r = degG;

        // Преобразуем в int[] для удобства операций XOR
        int[] temp = new int[n];
        for (int i = 0; i < n; i++) temp[i] = dividendBool[i] ? 1 : 0;

        // g ожидается в виде старший->младший (g[0] — при x^degG)
        for (int i = 0; i <= n - (degG + 1); i++)
        {
            if (temp[i] == 1)
            {
                for (int j = 0; j <= degG; j++)
                {
                    temp[i + j] ^= g[j];
                }
            }
        }

        // Остаток — последние r битов (начиная с позиции n - r)
        bool[] rem = new bool[r];
        int start = n - r;
        for (int i = 0; i < r; i++)
        {
            rem[i] = (start + i < n) ? (temp[start + i] == 1) : false;
        }

        return rem;
    }

    // 3. Формирование полного сообщения
    static bool[] FormFullMessage(bool[][] encodedWords)
    {
        bool[] message = new bool[TOTAL_BITS];
        int index = 0;

        foreach (var word in encodedWords)
        {
            foreach (bool bit in word)
            {
                if (index < TOTAL_BITS)
                {
                    message[index++] = bit;
                }
            }
        }

        // Заполняем оставшиеся биты нулями если нужно
        while (index < TOTAL_BITS)
        {
            message[index++] = false;
        }

        return message;
    }

    // 4. Создание матрицы перемежения (битовый матричный интерливинг)
    static bool[,] CreateInterleaveMatrix(bool[] message)
    {
        int rows = (int)Math.Ceiling((double)message.Length / COLUMNS);
        bool[,] matrix = new bool[rows, COLUMNS];

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if (index < message.Length)
                    matrix[i, j] = message[index++];
                else
                    matrix[i, j] = false;
            }
        }

        return matrix;
    }

    // Перемежение: читаем столбцами (как в оригинале)
    static bool[] Interleave(bool[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        bool[] result = new bool[rows * cols];
        int index = 0;

        for (int j = 0; j < cols; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                result[index++] = matrix[i, j];
            }
        }

        return result;
    }

    // 5. Внесение ошибок (инвертирование пакета бит)
    static bool[] IntroduceErrors(bool[] message, int start, int length)
    {
        bool[] withErrors = (bool[])message.Clone();

        for (int i = start; i < start + length && i < message.Length; i++)
        {
            withErrors[i] = !withErrors[i]; // Инвертируем бит
        }

        return withErrors;
    }

    // 6. Создание матрицы деперемежения (запись по столбцам)
    static bool[,] CreateDeinterleaveMatrix(bool[] message)
    {
        int rows = (int)Math.Ceiling((double)message.Length / COLUMNS);
        bool[,] matrix = new bool[rows, COLUMNS];

        int index = 0;
        for (int j = 0; j < COLUMNS; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                if (index < message.Length)
                    matrix[i, j] = message[index++];
                else
                    matrix[i, j] = false;
            }
        }

        return matrix;
    }

    // Деперемежение: читаем построчно
    static bool[] Deinterleave(bool[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        bool[] result = new bool[rows * cols];
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[index++] = matrix[i, j];
            }
        }

        return result;
    }

    // Разбиение на слова для декодирования
    static bool[][] SplitIntoWords(bool[] message)
    {
        int numWords = (int)Math.Ceiling((double)message.Length / CODEWORD_LENGTH);
        bool[][] words = new bool[numWords][];

        for (int i = 0; i < numWords; i++)
        {
            words[i] = new bool[CODEWORD_LENGTH];
            int bitsToCopy = Math.Min(CODEWORD_LENGTH, message.Length - i * CODEWORD_LENGTH);
            Array.Copy(message, i * CODEWORD_LENGTH, words[i], 0, bitsToCopy);
        }

        return words;
    }

    // Построение синдромов единичных ошибок: для каждой позиции pos (0..n-1) вычислить remainder(e_pos / g)
    static bool[][] BuildSingleErrorSyndromes(int n, int[] g)
    {
        int degG = g.Length - 1;
        bool[][] synd = new bool[n][];
        for (int pos = 0; pos < n; pos++)
        {
            bool[] e = new bool[n];
            e[pos] = true; // единичная ошибка в позиции pos
            synd[pos] = CalculateParityBits(e, g);
        }
        return synd;
    }

    // Найти позицию одиночной ошибки — сравнить синдром с каждым столбцом H (singleErrorSyndromes)
    static int FindSingleErrorPosition(bool[] syndrome, bool[][] singleErrorSyndromes)
    {
        for (int col = 0; col < singleErrorSyndromes.Length; col++)
        {
            if (SyndromesEqual(syndrome, singleErrorSyndromes[col]))
                return col;
        }
        return -1;
    }

    // Найти пару позиций такой, что XOR их синдромов равен заданному синдрому (поиск двойной ошибки)
    static Tuple<int, int> FindDoubleErrorPositions(bool[] syndrome, bool[][] singleErrorSyndromes)
    {
        int n = singleErrorSyndromes.Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                bool match = true;
                for (int b = 0; b < syndrome.Length; b++)
                {
                    bool xorBit = singleErrorSyndromes[i][b] ^ singleErrorSyndromes[j][b];
                    if (xorBit != syndrome[b]) { match = false; break; }
                }
                if (match) return Tuple.Create(i, j);
            }
        }
        return Tuple.Create(-1, -1);
    }

    // Подсчет различающихся битов между двумя массивами
    static int CountBitErrors(bool[] arr1, bool[] arr2)
    {
        int errors = 0;
        int minLength = Math.Min(arr1.Length, arr2.Length);

        for (int i = 0; i < minLength; i++)
        {
            if (arr1[i] != arr2[i])
            {
                errors++;
            }
        }

        return errors;
    }

    // Вспомогательные методы для вывода
    static void PrintInfoWords(bool[][] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            Console.WriteLine($"Слово {i + 1}: {BitArrayToString(words[i])}");
        }
    }

    static void PrintEncodedWords(bool[][] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            Console.WriteLine($"X{i + 1}: {BitArrayToString(words[i])}");
        }
    }

    static void PrintBitArray(bool[] bits, int groupSize)
    {
        for (int i = 0; i < bits.Length; i++)
        {
            Console.Write(bits[i] ? "1" : "0");
            if ((i + 1) % groupSize == 0)
                Console.Write(" ");
        }
        Console.WriteLine();
    }

    static void PrintBitArrayWithErrors(bool[] withErrors, bool[] original, int groupSize)
    {
        for (int i = 0; i < withErrors.Length; i++)
        {
            if (withErrors[i] != original[i])
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write(withErrors[i] ? "1" : "0");

            if ((i + 1) % groupSize == 0)
                Console.Write(" ");

            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void PrintInterleaveMatrix(bool[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] ? "1" : "0");
            }
            Console.WriteLine();
        }
    }

    static void PrintDeinterleaveMatrix(bool[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] ? "1" : "0");
            }
            Console.WriteLine();
        }
    }

    static string BitArrayToString(bool[] bits)
    {
        return new string(bits.Select(b => b ? '1' : '0').ToArray());
    }

    static string BoolArrayToString(bool[] bits)
    {
        return BitArrayToString(bits);
    }

    static string GroupBits(bool[] bits, int groupSize)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < bits.Length; i++)
        {
            sb.Append(bits[i] ? '1' : '0');

            if ((i + 1) % groupSize == 0)
                sb.Append(' ');
        }

        return sb.ToString().TrimEnd();
    }

    static bool IsAllZero(bool[] a)
    {
        foreach (var b in a) if (b) return false;
        return true;
    }

    static bool SyndromesEqual(bool[] a, bool[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
        return true;
    }



    static ErrorStats ComputeErrorStatistics(bool[][] receivedWords, bool[][] referenceWords, bool[][] correctedWords)
    {
        ErrorStats S = new ErrorStats();

        // ВАЖНО: ДОЛЖНО быть ДО цикла!
        S.words = receivedWords.Length;

        for (int i = 0; i < receivedWords.Length; i++)
        {
            S.totalErrors += CountBitErrors(receivedWords[i], referenceWords[i]);

            bool[] syndrome = CalculateParityBits(receivedWords[i], gCoeffs);
            if (!IsAllZero(syndrome))
                S.detectedErrors++;

            if (correctedWords[i].SequenceEqual(referenceWords[i]))
                S.correctedErrors++;
            else
                S.uncorrectedErrors++;
        }

        return S;
    }


    public static void RunStatisticsManyTimes(
    bool[] fullMessage,
    bool[] interleavedMessage,
    int errorLength,
    int iterations = 30)
    {
        ErrorStats total = new ErrorStats();
        Random rnd = new Random();

        for (int it = 0; it < iterations; it++)
        {
            // --------- 5. Новый пакет ошибок ---------
            int errorStart = rnd.Next(0, interleavedMessage.Length - errorLength);
            bool[] messageWithErrors = IntroduceErrors(interleavedMessage, errorStart, errorLength);

            // --------- 6. Деперемежение ---------
            bool[,] deinterleaveMatrix = CreateDeinterleaveMatrix(messageWithErrors);
            bool[] deinterleaved = Deinterleave(deinterleaveMatrix);

            // --------- 7. Разбиение на слова ---------
            bool[][] receivedWords = SplitIntoWords(deinterleaved);
            bool[][] referenceWords = SplitIntoWords(fullMessage);

            // --------- 8. Декодирование ---------
            bool[][] correctedWords = new bool[receivedWords.Length][];
            bool[][] singleErrorSyndromes = BuildSingleErrorSyndromes(CODEWORD_LENGTH, gCoeffs);

            for (int idx = 0; idx < receivedWords.Length; idx++)
            {
                bool[] Yn = (bool[])receivedWords[idx].Clone();
                bool[] Xn = referenceWords[idx];

                bool[] syndrome = CalculateParityBits(Yn, gCoeffs);

                if (!IsAllZero(syndrome))
                {
                    int pos = FindSingleErrorPosition(syndrome, singleErrorSyndromes);
                    if (pos >= 0)
                        Yn[pos] = !Yn[pos];
                }

                correctedWords[idx] = Yn;
            }

            // --------- Подсчёт статистики ---------
            ErrorStats S = ComputeErrorStatistics(receivedWords, referenceWords, correctedWords);
            total.Add(S);
        }

        int words = total.words / iterations;

        Console.WriteLine("\n--- Итоговая статистика по 30 прогонов ---");
        Console.WriteLine($"Среднее число ошибок в сообщении: {(double)total.totalErrors / iterations:F2}");
        Console.WriteLine($"Среднее число слов с обнаруженными ошибками: {(double)total.detectedErrors / iterations:F2}/{words}");
        Console.WriteLine($"Среднее число слов с исправленными ошибками: {(double)total.correctedErrors / iterations:F2}/{words}");
        Console.WriteLine($"Среднее число слов с неисправленными ошибками: {(double)total.uncorrectedErrors / iterations:F2}/{words}");

        double detectionEfficiency = (double)total.detectedErrors / (words * iterations) * 100;
        double correctionEfficiency = (double)total.correctedErrors / (words * iterations) * 100;

        Console.WriteLine($"Эффективность обнаружения: {detectionEfficiency:F1}%");
        Console.WriteLine($"Эффективность исправления: {correctionEfficiency:F1}%");
    }


}

class ErrorStats
{
    public int totalErrors;
    public int detectedErrors;
    public int correctedErrors;
    public int uncorrectedErrors;

    public int words;

    public void Add(ErrorStats s)
    {
        totalErrors += s.totalErrors;
        detectedErrors += s.detectedErrors;
        correctedErrors += s.correctedErrors;
        uncorrectedErrors += s.uncorrectedErrors;

        // добавляем накопление количества слов
        words += s.words;
    }

}
