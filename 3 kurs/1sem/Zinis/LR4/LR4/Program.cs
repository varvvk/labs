using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class HammingCoder
{
    private int k, r, n;
    private int[,] H; // Проверочная матрица
    private Random rng = new Random();

    public HammingCoder(int dataLength)
    {
        k = dataLength;
        r = CalculateRedundantBits(k);
        n = k + r;
        H = BuildHammingMatrix(k, r);
    }

    // Публичное свойство для доступа к проверочной матрице
    public int[,] GetH() => H;

    // Расчет количества избыточных битов - простое вычисление
    private int CalculateRedundantBits(int k)
    {
        return (int)Math.Ceiling(Math.Log(k, 2)) + 1;
    }

    // Построение проверочной матрицы Хемминга
    private int[,] BuildHammingMatrix(int k, int r)
    {
        int n = k + r;
        int[,] H = new int[r, n];

        // Единичная матрица для проверочных битов (правая часть)
        for (int i = 0; i < r; i++)
            H[i, k + i] = 1;

        // Формирование левой части (информационные биты)
        var available = new List<int[]>();
        for (int num = 1; num < (1 << r); num++)
        {
            if ((num & (num - 1)) == 0) continue; // исключаем степени двойки
            int[] v = new int[r];
            for (int b = 0; b < r; b++)
                v[b] = (num >> (r - 1 - b)) & 1;
            available.Add(v);
        }

        for (int col = 0; col < k; col++)
        {
            int[] colVec;
            if (col < available.Count) colVec = available[col];
            else
            {
                colVec = GenerateUniqueRandomVector(H, r, n);
            }

            for (int i = 0; i < r; i++)
                H[i, col] = colVec[i];
        }

        return H;
    }

    private int[] GenerateUniqueRandomVector(int[,] H, int r, int n)
    {
        while (true)
        {
            int[] tryV = new int[r];
            for (int i = 0; i < r; i++)
                tryV[i] = rng.Next(0, 2);
            if (tryV.Sum() < 2) continue;

            bool unique = true;
            for (int j = 0; j < n; j++)
            {
                bool equal = true;
                for (int i = 0; i < r; i++)
                    if (H[i, j] != tryV[i]) { equal = false; break; }
                if (equal) { unique = false; break; }
            }
            if (unique) return tryV;
        }
    }

    // Кодирование информационного слова
    public int[] Encode(int[] data)
    {
        int[] Xr = ComputeParityFromP(H, data, r, k);
        int[] Xn = new int[n];
        Array.Copy(data, 0, Xn, 0, k);
        Array.Copy(Xr, 0, Xn, k, r);
        return Xn;
    }

    // Вычисление проверочных битов из информационных
    private int[] ComputeParityFromP(int[,] H, int[] Xk, int r, int k)
    {
        int[] Xr = new int[r];
        for (int i = 0; i < r; i++)
        {
            int s = 0;
            for (int j = 0; j < k; j++)
                s ^= (H[i, j] & Xk[j]);
            Xr[i] = s & 1;
        }
        return Xr;
    }

    // Умножение матрицы H на вектор
    public int[] MultiplyHbyVector(int[] v)
    {
        int[] result = new int[r];
        for (int i = 0; i < r; i++)
        {
            int sum = 0;
            for (int j = 0; j < n; j++)
                sum ^= (H[i, j] & v[j]);
            result[i] = sum & 1;
        }
        return result;
    }

    // Поиск позиции ошибки по синдрому
    public int FindErrorPosition(int[] S)
    {
        for (int j = 0; j < n; j++)
        {
            bool equal = true;
            for (int i = 0; i < r; i++)
            {
                if (H[i, j] != S[i]) { equal = false; break; }
            }
            if (equal) return j;
        }
        return -1;
    }

    // Генерация уникальных случайных позиций для ошибок
    public List<int> GenerateUniqueRandomPositions(int count)
    {
        var list = Enumerable.Range(0, n).ToList();
        var res = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int idx = rng.Next(list.Count);
            res.Add(list[idx]);
            list.RemoveAt(idx);
        }
        return res;
    }

    // Проверка уникальности синдромов
    public bool CheckSyndromeUniqueness()
    {
        var map = new HashSet<string>();
        for (int j = 0; j < n; j++)
        {
            string col = string.Concat(Enumerable.Range(0, r).Select(i => H[i, j].ToString()));
            if (!map.Add(col))
            {
                return false;
            }
        }
        return true;
    }

    // Вывод матрицы
    public void PrintMatrix(string name)
    {
        Console.WriteLine($"{name} ({r}x{n}):");
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(H[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    // Вывод вектора
    public void PrintVector(int[] vector, string name)
    {
        Console.WriteLine($"{name}: {string.Join("", vector)}");
    }

    // Получение параметров кода
    public void GetParameters(out int k_val, out int r_val, out int n_val)
    {
        k_val = k;
        r_val = r;
        n_val = n;
    }
}

class Program
{
    static void Main()
    {
        string message = "i1#";
        int[] binaryData = StringToBinary(message);

        Console.WriteLine($"Исходное сообщение: {message}");
        Console.WriteLine($"Двоичное представление: {string.Join("", binaryData)}");
        Console.WriteLine($"Длина информационного слова (k): {binaryData.Length}");

        HammingCoder coder = new HammingCoder(binaryData.Length);

        // Получаем параметры кода
        int k, r, n;
        coder.GetParameters(out k, out r, out n);
        Console.WriteLine($"Параметры кода: k={k}, r={r}, n={n}");
        Console.WriteLine($"Формула: r = ceil(log2({k})) + 1 = {r}");

        // Проверка уникальности синдромов
        Console.WriteLine("\nПроверка уникальности столбцов матрицы H:");
        bool unique = coder.CheckSyndromeUniqueness();
        if (!unique)
        {
            Console.WriteLine("Обнаружены дублирующиеся синдромы. Завершение программы.");
            return;
        }

        // Получаем проверочную матрицу
        coder.PrintMatrix("Проверочная матрица H");

        // Формирование кодового слова
        int[] Xk = binaryData;
        int[] Xn = coder.Encode(Xk);

        // Извлекаем проверочные биты Xr
        int[] Xr = new int[r];
        Array.Copy(Xn, k, Xr, 0, r);

        Console.WriteLine("\nФОРМИРОВАНИЕ КОДОВОГО СЛОВА");
        Console.WriteLine($"Информационные биты Xk: {string.Join("", Xk)}");
        Console.WriteLine($"Проверочные биты Xr:     {string.Join("", Xr)}");
        Console.WriteLine($"Полное кодовое слово Xn: {string.Join("", Xn)}");

        // Проверка корректности кодирования
        int[] syndromeCheck = coder.MultiplyHbyVector(Xn);
        Console.WriteLine("\nПроверка корректности кодирования (синдром H*Xn):");
        Console.WriteLine(string.Join("", syndromeCheck));
        if (syndromeCheck.All(b => b == 0))
            Console.WriteLine("Результат: синдром равен нулю. Кодирование выполнено корректно.");
        else
            Console.WriteLine("Результат: обнаружено несоответствие. Кодирование выполнено с ошибкой.");

        // 5. Моделирование ошибок
        Console.WriteLine("\nРАБОТА С ОШИБКАМИ");
        var scenarios = new (string name, int count)[] {
            ("Без ошибок", 0),
            ("Одна ошибка", 1),
            ("Две ошибки", 2)
        };

        foreach (var (name, count) in scenarios)
        {
            Console.WriteLine($"\nВариант проверки задания: {name}");
            Console.WriteLine(new string('-', 60));

            int[] Yn = (int[])Xn.Clone();
            List<int> errorPositions = new();

            if (count > 0)
            {
                errorPositions = coder.GenerateUniqueRandomPositions(count);
                Console.WriteLine($"Ошибки внесены в позиции: {string.Join(", ", errorPositions.Select(p => p + 1))}");
                foreach (int p in errorPositions)
                    Yn[p] = 1 - Yn[p];
            }

            Console.WriteLine($"Принятое слово Yn: {string.Join("", Yn)}");

            int[] Yk = Yn.Take(k).ToArray();
            int[] YrActual = Yn.Skip(k).Take(r).ToArray();
            int[] YrRecalc = ComputeParityFromP(coder.GetH(), Yk, r, k);

            Console.WriteLine($"Yr (принятые проверочные биты): {string.Join("", YrActual)}");
            Console.WriteLine($"Yr' (пересчитанные из Yk):      {string.Join("", YrRecalc)}");

            int[] S = coder.MultiplyHbyVector(Yn);
            Console.WriteLine($"Синдром S = H * Yn: {string.Join("", S)}");

            int[] En = new int[n];
            int[] corrected = (int[])Yn.Clone();

            if (S.All(b => b == 0))
            {
                if (count == 0)
                    Console.WriteLine("Ошибок не обнаружено.");
                else
                    Console.WriteLine("Синдром нулевой, но ошибки имеются. Возможна маскировка.");
            }
            else
            {
                int errorPos = coder.FindErrorPosition(S);
                if (errorPos != -1)
                {
                    En[errorPos] = 1;
                    corrected[errorPos] = 1 - corrected[errorPos];
                    Console.WriteLine($"Обнаружена ошибка в позиции {errorPos + 1}. Исправление выполнено.");
                }
                else
                {
                    Console.WriteLine("Ошибка не может быть локализована (возможно, множественная ошибка).");
                }
            }

            Console.WriteLine($"Вектор ошибки En: {string.Join("", En)}");
            Console.WriteLine($"Исправленное слово: {string.Join("", corrected)}");

            bool identical = corrected.SequenceEqual(Xn);
            Console.WriteLine(identical
                ? "Результат: кодовое слово восстановлено корректно."
                : "Результат: восстановление не удалось.");
        }
    }

    static int[] ComputeParityFromP(int[,] H, int[] Xk, int r, int k)
    {
        int[] Xr = new int[r];
        for (int i = 0; i < r; i++)
        {
            int s = 0;
            for (int j = 0; j < k; j++)
                s ^= (H[i, j] & Xk[j]);
            Xr[i] = s & 1;
        }
        return Xr;
    }

    static int[] StringToBinary(string str)
    {
        List<int> binary = new List<int>();
        foreach (char c in str)
        {
            string binaryChar = Convert.ToString((int)c, 2).PadLeft(8, '0');
            foreach (char bit in binaryChar)
            {
                binary.Add(bit - '0');
            }
        }
        return binary.ToArray();
    }
}