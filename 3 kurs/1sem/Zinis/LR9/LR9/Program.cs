using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanCoder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Текст для сжатия
            string message = "kazakovavarvara".ToLower();

            // a) Вероятности на основе "алфавита" — статистика из полного ФИО (как в примере с польским/латинским алфавитом)
            string alphabetSource = "kazakovavarvara".ToLower(); // Полное ФИО в латинской транслитерации
            Console.WriteLine("a) Кодирование Хаффмана с вероятностями на основе алфавита из ЛР2");
            var freqA = CalculateFrequencies(alphabetSource);
            var huffmanA = BuildHuffmanTree(freqA);
            var codesA = GetHuffmanCodes(huffmanA);
            DisplayTable(codesA, freqA, alphabetSource.Length, "Вероятность (по алфавиту)");
            string encodedA = Encode(message, codesA);
            string decodedA = Decode(encodedA, huffmanA);
            Console.WriteLine("Закодированное сообщение: " + encodedA);
            Console.WriteLine("Декодированное: " + decodedA);
            int originalBits = message.Length * 8; // ASCII: 8 бит на символ
            int compressedBitsA = encodedA.Length;
            Console.WriteLine($"Оригинал (ASCII): {originalBits} бит");
            Console.WriteLine($"Сжато (Хаффман по алфавиту): {compressedBitsA} бит");
            Console.WriteLine($"Коэффициент сжатия: {(double)compressedBitsA / originalBits * 100:F2}% от ASCII\n");

            // b) Динамическое кодирование — вероятности по самому сообщению
            Console.WriteLine("b) Динамическое кодирование Хаффмана (вероятности по самому сообщению):");
            var freqB = CalculateFrequencies(message);
            var huffmanB = BuildHuffmanTree(freqB);
            var codesB = GetHuffmanCodes(huffmanB);
            DisplayTable(codesB, freqB, message.Length, "Вероятность (динамическая)");
            string encodedB = Encode(message, codesB);
            string decodedB = Decode(encodedB, huffmanB);
            Console.WriteLine("Закодированное сообщение: " + encodedB);
            Console.WriteLine("Декодированное: " + decodedB);
            int compressedBitsB = encodedB.Length;
            Console.WriteLine($"Оригинал (ASCII): {originalBits} бит");
            Console.WriteLine($"Сжато (динамический Хаффман): {compressedBitsB} бит");
            Console.WriteLine($"Коэффициент сжатия: {(double)compressedBitsB / originalBits * 100:F2}% от ASCII\n");

            // Дополнительный пример: Динамическое кодирование Шеннона-Фано (вероятности по самому сообщению)
            Console.WriteLine("Динамическое кодирование Шеннона-Фано (вероятности по самому сообщению):");
            var shannonCodes = GetShannonFanoCodes(freqB);
            DisplayTableShannon(shannonCodes, freqB, message.Length, "Вероятность (динамическая)");
            string encodedShannon = EncodeShannon(message, shannonCodes);
            string decodedShannon = DecodeShannon(encodedShannon, shannonCodes);
            Console.WriteLine("Закодированное сообщение: " + encodedShannon);
            Console.WriteLine("Декодированное: " + decodedShannon);
            int compressedBitsShannon = encodedShannon.Length;
            Console.WriteLine($"Оригинал (ASCII): {originalBits} бит");
            Console.WriteLine($"Сжато: {compressedBitsShannon} бит");
            Console.WriteLine($"Коэффициент сжатия: {(double)compressedBitsShannon / originalBits * 100:F2}% от ASCII");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static Dictionary<char, int> CalculateFrequencies(string text)
        {
            var count = new Dictionary<char, int>();
            foreach (char c in text)
            {
                if (count.ContainsKey(c))
                    count[c]++;
                else
                    count[c] = 1;
            }
            return count;
        }

        class Node
        {
            public char? Symbol { get; set; }
            public int Frequency { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        static Node BuildHuffmanTree(Dictionary<char, int> frequencies)
        {
            if (frequencies.Count == 0) return null;
            var nodes = frequencies.Select(kv => new Node { Symbol = kv.Key, Frequency = kv.Value }).ToList();
            while (nodes.Count > 1)
            {
                // Сортировка по частоте, затем по символу для стабильности
                nodes = nodes.OrderBy(n => n.Frequency)
                             .ThenBy(n => n.Symbol ?? char.MaxValue)
                             .ToList();
                var left = nodes[0];
                var right = nodes[1];
                var parent = new Node
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Add(parent);
            }
            return nodes.Count == 1 ? nodes[0] : null;
        }

        static Dictionary<char, string> GetHuffmanCodes(Node root)
        {
            var codes = new Dictionary<char, string>();
            if (root == null) return codes;
            AssignCodes(root, "", codes);
            return codes;
        }

        static void AssignCodes(Node node, string code, Dictionary<char, string> codes)
        {
            if (node == null) return;
            if (node.Symbol.HasValue)
            {
                // Если код пустой (единственный символ), присваиваем "0" по соглашению
                codes[node.Symbol.Value] = string.IsNullOrEmpty(code) ? "0" : code;
                return;
            }
            AssignCodes(node.Left, code + "0", codes);
            AssignCodes(node.Right, code + "1", codes);
        }

        static string Encode(string message, Dictionary<char, string> codes)
        {
            var sb = new StringBuilder();
            foreach (char c in message)
            {
                if (!codes.TryGetValue(c, out string code))
                {
                    throw new Exception($"Символ '{c}' отсутствует в таблице кодов");
                }
                sb.Append(code);
            }
            return sb.ToString();
        }

        static string Decode(string encoded, Node root)
        {
            if (root == null || string.IsNullOrEmpty(encoded)) return string.Empty;
            var sb = new StringBuilder();
            var current = root;
            foreach (char bit in encoded)
            {
                current = bit == '0' ? current.Left : current.Right;
                if (current.Symbol.HasValue)
                {
                    sb.Append(current.Symbol.Value);
                    current = root;
                }
            }
            return sb.ToString();
        }

        static void DisplayTable(Dictionary<char, string> codes, Dictionary<char, int> freq, int totalLength, string probTitle)
        {
            Console.WriteLine("Символ\t" + probTitle + "\tКод Хаффмана");
            var sorted = freq.OrderByDescending(kv => (double)kv.Value / totalLength)
                             .ThenBy(kv => kv.Key);
            foreach (var kv in sorted)
            {
                double probability = (double)kv.Value / totalLength;
                string code = codes.ContainsKey(kv.Key) ? codes[kv.Key] : "—";
                Console.WriteLine($"{kv.Key}\t\t{probability:F4}\t\t{code}");
            }
            Console.WriteLine();
        }

        // Методы для Шеннона-Фано

        static Dictionary<char, string> GetShannonFanoCodes(Dictionary<char, int> frequencies)
        {
            var codes = new Dictionary<char, string>();

            // Сортируем символы по убыванию частоты
            var sortedSymbols = frequencies.OrderByDescending(kv => kv.Value)
                                           .ThenBy(kv => kv.Key)
                                           .Select(kv => kv.Key).ToList();

            ShannonFanoRecursive(sortedSymbols, 0, sortedSymbols.Count - 1, frequencies, codes, "");
            return codes;
        }

        static void ShannonFanoRecursive(List<char> symbols, int start, int end, Dictionary<char, int> frequencies, Dictionary<char, string> codes, string currentCode)
        {
            if (start > end) return;

            if (start == end)
            {
                codes[symbols[start]] = string.IsNullOrEmpty(currentCode) ? "0" : currentCode;
                return;
            }

            int totalFreq = 0;
            for (int i = start; i <= end; i++)
            {
                totalFreq += frequencies[symbols[i]];
            }

            int leftFreq = 0;
            int split = start;

            for (int i = start; i < end; i++)
            {
                leftFreq += frequencies[symbols[i]];
                if (leftFreq >= (totalFreq / 2))
                {
                    split = i;
                    break;
                }
            }

            // Присваиваем 0 левой группе
            ShannonFanoRecursive(symbols, start, split, frequencies, codes, currentCode + "0");

            // Присваиваем 1 правой группе
            ShannonFanoRecursive(symbols, split + 1, end, frequencies, codes, currentCode + "1");
        }

        static string EncodeShannon(string message, Dictionary<char, string> codes)
        {
            var sb = new StringBuilder();
            foreach (char c in message)
            {
                if (!codes.TryGetValue(c, out string code))
                {
                    throw new Exception($"Символ '{c}' отсутствует в таблице кодов");
                }
                sb.Append(code);
            }
            return sb.ToString();
        }

        static string DecodeShannon(string encoded, Dictionary<char, string> codes)
        {
            var sb = new StringBuilder();
            string current = "";
            foreach (char bit in encoded)
            {
                current += bit;
                foreach (var kv in codes)
                {
                    if (kv.Value == current)
                    {
                        sb.Append(kv.Key);
                        current = "";
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(current))
            {
                throw new Exception("Ошибка декодирования: неполный код");
            }
            return sb.ToString();
        }

        static void DisplayTableShannon(Dictionary<char, string> codes, Dictionary<char, int> freq, int totalLength, string probTitle)
        {
            Console.WriteLine("Символ\t" + probTitle + "\tКод Шеннона-Фано");
            var sorted = freq.OrderByDescending(kv => (double)kv.Value / totalLength)
                             .ThenBy(kv => kv.Key);
            foreach (var kv in sorted)
            {
                double probability = (double)kv.Value / totalLength;
                string code = codes.ContainsKey(kv.Key) ? codes[kv.Key] : "—";
                Console.WriteLine($"{kv.Key}\t\t{probability:F4}\t\t{code}");
            }
            Console.WriteLine();
        }
    }
}