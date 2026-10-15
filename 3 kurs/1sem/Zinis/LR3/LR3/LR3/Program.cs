using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace EntropyCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Исходные данные
            string polishMessage = "Kazakova Varvara";
            string base64Message = "S2F6YWtvdmEgVmFydmFyYQ==";

            Console.WriteLine("=== АНАЛИЗ ПОЛЬСКОГО ТЕКСТА И BASE64 ===");
            Console.WriteLine($"Польский текст: {polishMessage}");
            Console.WriteLine($"Base64: {base64Message}");
            Console.WriteLine();

            // Анализ польского текста
            double H_pol_shannon = CalculateMessageEntropy(polishMessage);
            double H_pol_hartley = CalculateHartleyEntropy(polishMessage);
            double R_pol = CalculateRedundancy(H_pol_hartley, H_pol_shannon);

            // Анализ Base64 текста
            double H_base64_shannon = CalculateMessageEntropy(base64Message);
            double H_base64_hartley = CalculateHartleyEntropy(base64Message);
            double R_base64 = CalculateRedundancy(H_base64_hartley, H_base64_shannon);

            // Вывод результатов
            Console.WriteLine("РЕЗУЛЬТАТЫ:");
            Console.WriteLine("Польский текст:");
            Console.WriteLine($"  Энтропия Шеннона: {H_pol_shannon:F4} бит/символ");
            Console.WriteLine($"  Энтропия Хартли: {H_pol_hartley:F4} бит/символ");
            Console.WriteLine($"  Избыточность: {R_pol:F2}%");
            Console.WriteLine();

            Console.WriteLine("Base64 текст:");
            Console.WriteLine($"  Энтропия Шеннона: {H_base64_shannon:F4} бит/символ");
            Console.WriteLine($"  Энтропия Хартли: {H_base64_hartley:F4} бит/символ");
            Console.WriteLine($"  Избыточность: {R_base64:F2}%");
            Console.WriteLine();

            // Частотный анализ
            var polishFreq = CalculateSymbolFrequencies(polishMessage);
            var base64Freq = CalculateSymbolFrequencies(base64Message);

            WriteFrequenciesToCsv("polish_frequencies.csv", polishFreq, "Польский алфавит - Kazakova Varvara");
            WriteFrequenciesToCsv("base64_frequencies.csv", base64Freq, "Base64 алфавит - S2F6YWtvdmEgVmFydmFyYQ==");

            Console.WriteLine("Частоты символов записаны в файлы:");
            Console.WriteLine("  - polish_frequencies.csv");
            Console.WriteLine("  - base64_frequencies.csv");
            Console.WriteLine();

            // Задание 3: XOR операции в двоичном виде
            Console.WriteLine("=== ЗАДАНИЕ 3: XOR ОПЕРАЦИИ В ДВОИЧНОМ ВИДЕ ===");

            string surname = "Kazakova";
            string name = "Varvara";

            Console.WriteLine($"Фамилия (a): {surname}");
            Console.WriteLine($"Имя (b): {name}");
            Console.WriteLine();

            // 1) XOR в кодах ASCII с двоичным представлением
            Console.WriteLine("1) XOR в кодах ASCII (двоичное представление):");
            Console.WriteLine("==============================================");

            byte[] asciiA = StringToAscii(surname);
            byte[] asciiB = StringToAscii(name);

            // Показываем первые 3 символа в двоичном виде
            for (int i = 0; i < Math.Min(3, Math.Min(asciiA.Length, asciiB.Length)); i++)
            {
                char charA = (char)asciiA[i];
                char charB = (char)asciiB[i];

                string binaryA = ByteToBinary(asciiA[i]);
                string binaryB = ByteToBinary(asciiB[i]);
                string binaryXor = ByteToBinary((byte)(asciiA[i] ^ asciiB[i]));
                string binaryDoubleXor = ByteToBinary((byte)((asciiA[i] ^ asciiB[i]) ^ asciiB[i]));
            }

            // Полный XOR для всех символов ASCII
            Console.WriteLine("Полный XOR буферов ASCII:");
            byte[] xorResult = XorBuffers(asciiA, asciiB);
            byte[] doubleXorResult = XorBuffers(xorResult, asciiB);

            Console.WriteLine($"a:       {BytesToBinary(asciiA)}");
            Console.WriteLine($"b:       {BytesToBinary(asciiB)}");
            Console.WriteLine($"a XOR b: {BytesToBinary(xorResult)}");
            Console.WriteLine($"a XOR b XOR b: {BytesToBinary(doubleXorResult)}");
            Console.WriteLine($"Текст: '{AsciiToString(doubleXorResult)}'");
            Console.WriteLine();

            // 2) XOR в Base64 с двоичным представлением
            Console.WriteLine("2) XOR в Base64 (двоичное представление):");
            Console.WriteLine("=========================================");

            string base64A = StringToBase64(surname);
            string base64B = StringToBase64(name);

            byte[] base64BytesA = Base64ToBytes(base64A);
            byte[] base64BytesB = Base64ToBytes(base64B);

            Console.WriteLine($"a (Base64): {base64A}");
            Console.WriteLine($"b (Base64): {base64B}");

            // Показываем первые 3 байта в двоичном виде
            for (int i = 0; i < Math.Min(3, Math.Min(base64BytesA.Length, base64BytesB.Length)); i++)
            {
                string binaryA = ByteToBinary(base64BytesA[i]);
                string binaryB = ByteToBinary(base64BytesB[i]);
                string binaryXor = ByteToBinary((byte)(base64BytesA[i] ^ base64BytesB[i]));
                string binaryDoubleXor = ByteToBinary((byte)((base64BytesA[i] ^ base64BytesB[i]) ^ base64BytesB[i]));
            }

            // Полный XOR для Base64 в двоичном формате
            Console.WriteLine("Полный XOR буферов Base64:");
            byte[] base64Xor = XorBuffers(base64BytesA, base64BytesB);
            byte[] base64DoubleXor = XorBuffers(base64Xor, base64BytesB);

            Console.WriteLine($"a:       {BytesToBinary(base64BytesA)}");
            Console.WriteLine($"b:       {BytesToBinary(base64BytesB)}");
            Console.WriteLine($"a XOR b: {BytesToBinary(base64Xor)}");
            Console.WriteLine($"a XOR b XOR b: {BytesToBinary(base64DoubleXor)}");

            Console.WriteLine($"a XOR b (Base64): {BytesToBase64(base64Xor)}");
            Console.WriteLine($"a XOR b XOR b (Base64): {BytesToBase64(base64DoubleXor)}");
            Console.WriteLine($"Текст: '{Base64ToString(BytesToBase64(base64DoubleXor))}'");
            Console.WriteLine();

            // Демонстрация свойства a XOR b XOR b = a
            Console.WriteLine("3) Доказательство свойства a XOR b XOR b = a:");
            Console.WriteLine("=============================================");
            Console.WriteLine("Математическое свойство:");
            Console.WriteLine("  (a XOR b) XOR b = a XOR (b XOR b) = a XOR 0 = a");
            Console.WriteLine();
            Console.WriteLine("Практическая проверка:");
            Console.WriteLine($"  Исходный a: '{surname}'");
            Console.WriteLine($"  После a XOR b XOR b (ASCII): '{AsciiToString(doubleXorResult)}'");
            Console.WriteLine($"  После a XOR b XOR b (Base64): '{Base64ToString(BytesToBase64(base64DoubleXor))}'");

            Console.ReadKey();
        }

        // Преобразование байта в двоичную строку (8 бит)
        static string ByteToBinary(byte b)
        {
            char[] bits = new char[8];
            for (int i = 0; i < 8; i++)
            {
                bits[7 - i] = ((b >> i) & 1) == 1 ? '1' : '0';
            }
            return new string(bits);
        }

        // Преобразование массива байтов в двоичную строку
        static string BytesToBinary(byte[] bytes)
        {
            StringBuilder binary = new StringBuilder();
            foreach (byte b in bytes)
            {
                binary.Append(ByteToBinary(b) + " ");
            }
            return binary.ToString().Trim();
        }

        // Функция для XOR двух буферов (без использования стандартных функций XOR)
        static byte[] XorBuffers(byte[] a, byte[] b)
        {
            int maxLength = Math.Max(a.Length, b.Length);
            byte[] result = new byte[maxLength];
            byte[] paddedA = PadArray(a, maxLength);
            byte[] paddedB = PadArray(b, maxLength);

            for (int i = 0; i < maxLength; i++)
            {
                // Ручная реализация XOR побитово
                byte xorByte = 0;
                for (int bit = 0; bit < 8; bit++)
                {
                    bool bitA = ((paddedA[i] >> bit) & 1) == 1;
                    bool bitB = ((paddedB[i] >> bit) & 1) == 1;
                    bool xorBit = (bitA && !bitB) || (!bitA && bitB);

                    if (xorBit)
                    {
                        xorByte |= (byte)(1 << bit);
                    }
                }
                result[i] = xorByte;
            }

            return result;
        }

        // Дополнение массива нулями до нужной длины
        static byte[] PadArray(byte[] array, int length)
        {
            if (array.Length >= length) return array;

            byte[] padded = new byte[length];
            Array.Copy(array, padded, array.Length);
            for (int i = array.Length; i < length; i++)
            {
                padded[i] = 0;
            }
            return padded;
        }

        // Преобразование строки в ASCII байты
        static byte[] StringToAscii(string text)
        {
            byte[] bytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                bytes[i] = (byte)text[i];
            }
            return bytes;
        }

        // Преобразование ASCII байтов в строку
        static string AsciiToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = (char)bytes[i];
            }
            return new string(chars).TrimEnd('\0');
        }

        // Преобразование строки в Base64
        static string StringToBase64(string text)
        {
            byte[] bytes = StringToAscii(text);
            return Convert.ToBase64String(bytes);
        }

        // Преобразование Base64 в байты
        static byte[] Base64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        // Преобразование Base64 в строку
        static string Base64ToString(string base64)
        {
            byte[] bytes = Base64ToBytes(base64);
            return AsciiToString(bytes);
        }

        // Преобразование байтов в Base64
        static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        // Остальные функции остаются без изменений
        static Dictionary<char, double> CalculateSymbolFrequencies(string message)
        {
            var count = new Dictionary<char, int>();
            foreach (char c in message)
            {
                if (count.ContainsKey(c))
                    count[c]++;
                else
                    count[c] = 1;
            }
            int total = message.Length;
            var freq = new Dictionary<char, double>();
            foreach (var kv in count)
            {
                freq[kv.Key] = (double)kv.Value / total * 100;
            }
            return freq;
        }

        static double CalculateMessageEntropy(string message)
        {
            var count = new Dictionary<char, int>();
            foreach (char c in message)
            {
                if (count.ContainsKey(c))
                    count[c]++;
                else
                    count[c] = 1;
            }
            int total = message.Length;
            double entropy = 0;
            foreach (var kv in count)
            {
                double p = (double)kv.Value / total;
                entropy -= p * Math.Log(p, 2);
            }
            return entropy;
        }

        static double CalculateHartleyEntropy(string message)
        {
            var uniqueChars = new HashSet<char>(message);
            return Math.Log(uniqueChars.Count, 2);
        }

        static double CalculateRedundancy(double hartley, double shannon)
        {
            return ((hartley - shannon) / hartley) * 100;
        }

        static void WriteFrequenciesToCsv(string fileName, Dictionary<char, double> freq, string title)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.WriteLine($"{title}");
                writer.WriteLine("Symbol;Frequency (%);Count");
                foreach (var kv in freq.OrderByDescending(x => x.Value))
                {
                    string symbol = kv.Key.ToString();
                    if (char.IsWhiteSpace(kv.Key))
                        symbol = "Space";
                    else if (kv.Key == '=')
                        symbol = "Padding (=)";

                    writer.WriteLine($"{symbol};{kv.Value:F2};{freq.Values.Sum() * kv.Value / 100:F0}");
                }
            }
        }
    }
}