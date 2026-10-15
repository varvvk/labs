using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        string[] blocks =
        {
            "варя",
            "казакова",
            "песнетворчество"
        };

        var timeResults = new List<(string name, long direct, long reverse)>();

        // ===== ОСНОВНЫЕ СТРОКОВЫЕ БЛОКИ =====
        foreach (var block in blocks)
        {
            Console.WriteLine("\n====================================");
            Console.WriteLine($"Исходное сообщение: {block}");

            var sw = Stopwatch.StartNew();
            var (h, i) = DirectBWT(block);
            sw.Stop();
            long directTime = sw.ElapsedTicks;

            Console.WriteLine($"\nh = {h}");
            Console.WriteLine($"i = {i}");
            Console.WriteLine($"Время прямого преобразования: {directTime} тиков");

            sw.Restart();
            var restored = ReverseBWT(h, i);
            sw.Stop();
            long reverseTime = sw.ElapsedTicks;

            Console.WriteLine($"\nВосстановленное сообщение: {restored}");
            Console.WriteLine($"Время обратного преобразования: {reverseTime} тиков");

            timeResults.Add((block, directTime, reverseTime));
        }

        // ===== ЗАДАНИЕ 3: ASCII =====
        Console.WriteLine("\n====================================");
        Console.WriteLine("ЗАДАНИЕ 3. ASCII-ПРЕОБРАЗОВАНИЕ");

        string source = "песнетворчество";
        string firstThree = source.Substring(0, 3);

        Console.WriteLine($"Исходные символы: {firstThree}");

        string binary = ToAsciiBinary(firstThree);
        Console.WriteLine("Бинарная последовательность:");
        Console.WriteLine(binary);

        var swAscii = Stopwatch.StartNew();
        var (hAscii, iAscii) = DirectBWT(binary);
        swAscii.Stop();
        long directAsciiTime = swAscii.ElapsedTicks;

        Console.WriteLine($"\nh = {hAscii}");
        Console.WriteLine($"i = {iAscii}");
        Console.WriteLine($"Время прямого преобразования: {directAsciiTime} тиков");

        swAscii.Restart();
        string restoredBinary = ReverseBWT(hAscii, iAscii);
        swAscii.Stop();
        long reverseAsciiTime = swAscii.ElapsedTicks;

        Console.WriteLine("\nВосстановленная бинарная последовательность:");
        Console.WriteLine(restoredBinary);
        Console.WriteLine($"Время обратного преобразования: {reverseAsciiTime} тиков");

        timeResults.Add(("бинарный пес", directAsciiTime, reverseAsciiTime));

        // ===== СРАВНЕНИЕ ВРЕМЕНИ =====
        Console.WriteLine("\n====================================");
        Console.WriteLine("Сравнение времени\n");

        foreach (var r in timeResults)
        {
            Console.WriteLine($"Блок данных: {r.name}");
            Console.WriteLine($"  Прямое преобразование:  {r.direct} тиков");
            Console.WriteLine($"  Обратное преобразование:{r.reverse} тиков");

            if (r.reverse > r.direct)
                Console.WriteLine("  Вывод: обратное преобразование выполняется дольше\n");
            else
                Console.WriteLine("  Вывод: прямое преобразование выполняется дольше\n");
        }
    }

    // ===== ПРЯМОЕ ПРЕОБРАЗОВАНИЕ (BWT) =====
    static (string h, int i) DirectBWT(string message)
    {
        int k = message.Length;
        List<string> W1 = new();

        string row = message;
        for (int j = 0; j < k; j++)
        {
            W1.Add(row);
            row = row.Substring(1) + row[0];
        }

        Console.WriteLine("\nМатрица W1:");
        PrintMatrix(W1);

        List<string> W2 = W1.OrderBy(s => s).ToList();

        Console.WriteLine("\nМатрица W2 (после сортировки):");
        PrintMatrix(W2);

        StringBuilder h = new();
        foreach (var r in W2)
            h.Append(r[^1]);

        int i = W2.IndexOf(message) + 1;
        return (h.ToString(), i);
    }

    // ===== ОБРАТНОЕ ПРЕОБРАЗОВАНИЕ (как в примере) =====
    static string ReverseBWT(string h, int i)
    {
        int k = h.Length;

        List<string> table = h.Select(c => c.ToString()).ToList();
        Console.WriteLine("\nШаг 1:");
        PrintMatrix(table);

        table = table.OrderBy(s => s).ToList();
        Console.WriteLine("\nШаг 2:");
        PrintMatrix(table);

        for (int step = 1; step < k; step++)
        {
            for (int j = 0; j < k; j++)
                table[j] = h[j] + table[j];

            Console.WriteLine($"\nШаг {step + 2}:");
            PrintMatrix(table);

            table = table.OrderBy(s => s).ToList();
        }

        Console.WriteLine($"\nШаг {k + 1}:");
        PrintMatrix(table);

        return table[i - 1];
    }

    // ===== ВЫВОД МАТРИЦЫ =====
    static void PrintMatrix(List<string> matrix)
    {
        foreach (var row in matrix)
            Console.WriteLine(string.Join(" ", row.ToCharArray()));
    }

    // ===== ASCII → БИНАРНО =====
    static string ToAsciiBinary(string text)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in text)
            sb.Append(Convert.ToString((int)c, 2).PadLeft(16, '0'));
        return sb.ToString();
    }
}
