using System;
using System.Linq;
using System.Collections.Generic;

class CyclicCodeStepByStep
{
    // Исходный массив (как у тебя был): коэффициенты от младшей степени к старшей
    // (gCoeffsLowToHigh[0] = x^0, ..., gCoeffsLowToHigh[5] = x^5)
    static readonly int[] gCoeffsLowToHigh = new int[] { 1, 1, 1, 1, 0, 1 }; // x^5 + x^3 + x^2 + x + 1

    static void Main()
    {
        int r = 5;
        int n = (1 << r) - 1; // 31
        int k = n - r;        // 26

        // Переворачиваем gCoeffs, чтобы g[0] = коэффициент при x^5 (старший)
        int[] g = (int[])gCoeffsLowToHigh.Clone();
        Array.Reverse(g); // теперь g[0] — старший коэффициент (x^5), g[5] — свободный член

        Console.WriteLine($"Порождающий полином: x^5 + x^3 + x^2 + x + 1 (r={r})");
        Console.WriteLine($"(внутреннее представление g: старший-коэффициент-первым) g = {string.Join("", g)}");
        Console.WriteLine($"n = 2^{r} - 1 = {(1 << r)} - 1 = {n}");
        Console.WriteLine($"n = {n}, k = {k}\n");

        // 1️⃣ Построение циклической порождающей матрицы
        int[,] G_cyclic = BuildCyclicGeneratorMatrix(g, n, k);
        Console.WriteLine("Порождающая матрица G в циклической форме (первые 6 строк):");
        PrintBinaryMatrix(G_cyclic, Math.Min(6, k), n);
        Console.WriteLine();

        // 2️⃣ Приведение к канонической форме [I_k | P] через XOR
        int[,] G_systematic = ConvertToSystematicCanonical(G_cyclic, k, n, 6);
        Console.WriteLine("\nПорождающая матрица G в канонической форме (первые 6 строк):");
        PrintBinaryMatrix(G_systematic, Math.Min(6, k), n);
        Console.WriteLine();

        // 3️⃣ Построение проверочной матрицы H = [P^T | I_r]
        int[,] P = ExtractSubmatrix(G_systematic, 0, k - 1, k, n - 1);
        int[,] H = BuildParityCheckFromP(P, k, r);
        Console.WriteLine("Проверочная матрица H:");
        PrintBinaryMatrix(H, r, n);
        Console.WriteLine();

        // 4️⃣ Случайное информационное слово длины k
        var rnd = new Random();
        int[] message = new int[k];
        for (int i = 0; i < k; i++) message[i] = rnd.Next(2);

        Console.WriteLine("Информационное слово:");
        Console.WriteLine(string.Join("", message));
        Console.WriteLine();

        // 5️⃣ Кодирование через полиномиальный алгоритм (правильный порядок битов)
        int[] codeword = EncodeWithPolynomial(message, n, k, r, g);
        Console.WriteLine("Кодовое слово Xn:");
        Console.WriteLine(string.Join("", codeword));

        // --- Введение ошибок и проверка синдрома ---
        int numErrors = 2; // пробуем 0, 1 и 2 ошибки поочередно
        var errorRnd = new Random();

        for (int e = 0; e <= numErrors; e++)
        {
            int[] Yn = (int[])codeword.Clone();
            HashSet<int> errorPositions = new HashSet<int>();

            // Генерация e случайных позиций ошибок
            while (errorPositions.Count < e)
                errorPositions.Add(errorRnd.Next(n));

            foreach (var pos in errorPositions)
                Yn[pos] ^= 1;

            Console.WriteLine($"\nКодовое слово с {e} ошибками (позиции {string.Join(", ", errorPositions)}):");
            Console.WriteLine(string.Join("", Yn));

            // --- Вычисление синдрома через деление полиномов ---
            int[] syndrome = PolyMod(Yn, g, r);
            Console.WriteLine("Синдром (остаток Yn/g(X)): " + string.Join("", syndrome));

            // --- Исправление одиночной ошибки через поиск столбца в H ---
            if (syndrome.Any(bit => bit == 1))
            {
                if (e == 1)
                {
                    int errorPos = -1;
                    for (int j = 0; j < n; j++)
                    {
                        bool match = true;
                        for (int i = 0; i < r; i++)
                            if (H[i, j] != syndrome[i])
                            {
                                match = false;
                                break;
                            }
                        if (match)
                        {
                            errorPos = j;
                            break;
                        }
                    }

                    if (errorPos >= 0)
                    {
                        Yn[errorPos] ^= 1; // исправление ошибки
                        Console.WriteLine($"Исправлена одиночная ошибка в позиции {errorPos}. Исправленное слово:");
                        Console.WriteLine(string.Join("", Yn));
                    }
                    else
                    {
                        Console.WriteLine("Синдром не соответствует ни одному столбцу H. Ошибка не исправлена.");
                    }
                }
                else if (e == 2)
                {
                    Console.WriteLine("Две ошибки обнаружены. Невозможно исправить.");
                }
            }
            else
            {
                Console.WriteLine("Ошибок нет, синдром равен нулю.");
            }
        }
    }

    // --- Построение циклической матрицы ---
    // Здесь ожидаем, что g[0] = coeff при x^deg (старший), g[deg] = coeff при x^0 (младший)
    static int[,] BuildCyclicGeneratorMatrix(int[] g, int n, int k)
    {
        int deg = g.Length - 1; // 5
        int[,] G = new int[k, n];
        for (int i = 0; i < k; i++)
        {
            for (int j = 0; j < g.Length; j++)
            {
                if (i + j < n)
                {
                    // g[j] соответствует смещению j от старшего бита текущей позиции
                    G[i, i + j] = g[j];
                }
            }
        }
        return G;
    }

    // --- Приведение к канонической форме [I_k | P] через XOR ---
    static int[,] ConvertToSystematicCanonical(int[,] G, int k, int n, int maxPrintRows)
    {
        int[,] Gsys = new int[k, n];
        List<int>[] rowUsed = new List<int>[k];

        for (int i = 0; i < k; i++) rowUsed[i] = new List<int>();

        int[,] tempG = (int[,])G.Clone();

        for (int col = 0; col < k; col++)
        {
            int pivotRow = -1;
            for (int row = col; row < k; row++)
                if (tempG[row, col] == 1) { pivotRow = row; break; }

            if (pivotRow != col && pivotRow != -1)
            {
                for (int j = 0; j < n; j++)
                {
                    int t = tempG[col, j];
                    tempG[col, j] = tempG[pivotRow, j];
                    tempG[pivotRow, j] = t;
                }
            }

            if (pivotRow == -1)
            {
                // Нет опорного элемента в этом столбце — это необычно для корректно выбранного g,
                // но мы продолжаем (можно добавить обработку ошибок).
                continue;
            }

            for (int row = 0; row < k; row++)
            {
                if (row != col && tempG[row, col] == 1)
                {
                    for (int j = 0; j < n; j++)
                        tempG[row, j] ^= tempG[col, j];
                    rowUsed[row].Add(col);
                }
            }
            rowUsed[col].Add(col);
        }

        for (int i = 0; i < k; i++)
        {
            for (int j = 0; j < n; j++) Gsys[i, j] = tempG[i, j];
            if (i < maxPrintRows)
                Console.WriteLine($"Каноническая строка {i} = XOR исходных строк: {string.Join(", ", rowUsed[i])}");
        }

        return Gsys;
    }

    static int[,] BuildParityCheckFromP(int[,] P, int k, int r)
    {
        int[,] H = new int[r, k + r];
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < k; j++)
                H[i, j] = P[j, i];
            H[i, k + i] = 1;
        }
        return H;
    }

    static int[,] ExtractSubmatrix(int[,] A, int r1, int r2, int c1, int c2)
    {
        int rows = r2 - r1 + 1;
        int cols = c2 - c1 + 1;
        int[,] B = new int[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                B[i, j] = A[r1 + i, c1 + j];
        return B;
    }

    static void PrintBinaryMatrix(int[,] M, int rowsToShow, int colsToShow)
    {
        for (int i = 0; i < rowsToShow; i++)
        {
            for (int j = 0; j < colsToShow; j++)
                Console.Write(M[i, j]);
            Console.WriteLine();
        }
    }

    // --- Кодирование: умножение на x^r и деление по g(X) ---
    static int[] EncodeWithPolynomial(int[] message, int n, int k, int r, int[] g)
    {
        // message: старший-бит-слева (message[0] -- x^{k-1})
        int[] Xk = new int[k];
        for (int i = 0; i < k; i++) Xk[i] = message[i];

        // 1️⃣ Умножаем на x^r → добавляем r нулей справа (полином с длиной n)
        int[] shifted = new int[n];
        for (int i = 0; i < k; i++)
            shifted[i] = Xk[i]; // старшие биты слева
        Console.WriteLine("Xk*X^r: " + string.Join("", shifted));

        // 2️⃣ Остаток деления
        int[] remainder = PolyMod(shifted, g, r);
        Console.WriteLine("2 Остаток (R) после деления на g(X): " + string.Join("", remainder));

        // 3️⃣ Итоговое кодовое слово Xn = Xk || R
        int[] codeword = new int[n];
        for (int i = 0; i < k; i++)
            codeword[i] = Xk[i];
        for (int i = 0; i < r; i++)
            codeword[k + i] = remainder[i];

        return codeword;
    }

    // Деление полиномов по модулю 2, ожидаем g[0] = старший коэффициент
    // Возвращаем остаток длины r (старший-бит-слева), дополняем нулями слева при необходимости
    static int[] PolyMod(int[] dividend, int[] g, int r)
    {
        int[] temp = (int[])dividend.Clone();
        int n = temp.Length;
        int degG = g.Length - 1; // степень g

        // Выполняем классическое побитное деление: выравниваем старший коэффициент g с текущим старшим 1
        for (int i = 0; i <= n - degG - 1; i++)
        {
            if (temp[i] == 1)
            {
                for (int j = 0; j <= degG; j++)
                    temp[i + j] ^= g[j]; // g[0] — старший
            }
        }

        // Остаток = последние r битов (старший слева)
        int[] rem = new int[r];
        int start = n - r;
        for (int i = 0; i < r; i++)
        {
            rem[i] = (start + i < n) ? temp[start + i] : 0;
        }

        // Гарантия: rem.Length == r, ведущие нули уже присутствуют, если степень остатка < r
        return rem;
    }
}
