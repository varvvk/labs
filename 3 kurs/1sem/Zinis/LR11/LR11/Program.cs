using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArithmeticCodingLab11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string msg1 = "песнетворчество";
            string msg2 = "электрифицированный";
            string concatenated = msg1 + msg2;

            Console.WriteLine($"Сообщение 1:              {msg1} (длина: {msg1.Length})");
            Console.WriteLine($"Сообщение 2:  {msg2} (длина: {msg2.Length})");
            Console.WriteLine($"Составное сообщение:      {concatenated} (длина: {concatenated.Length})\n");

            ProcessMessage(msg1, "Сообщение 1");
            ProcessMessage(concatenated, "Составное сообщение");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ProcessMessage(string message, string title)
        {
            Console.WriteLine($"─── {title} ────────────────────────────────────────────────");

            var probabilities = CalculateProbabilities(message);
            var sortedSymbols = probabilities.Keys.OrderBy(c => c).ToList();
            var cumProbs = CalculateCumulativeProbabilities(sortedSymbols, probabilities);

            Console.WriteLine("Вероятности символов:");
            foreach (var sym in sortedSymbols)
            {
                Console.WriteLine($"{sym,2} → {probabilities[sym]:F6}");
            }
            Console.WriteLine();

            decimal code = Encode(message, sortedSymbols, cumProbs, probabilities);
            Console.WriteLine($"Арифметический код: {code:F24}");

            string decoded = Decode(code, message.Length, sortedSymbols, cumProbs, probabilities);

            Console.WriteLine($"Раскодированное сообщение: {decoded}");
            Console.WriteLine(message == decoded
                ? "РАСКОДИРОВАНИЕ УСПЕШНО"
                : $"ОШИБКА! Ожидалось: {message}\nПолучено:   {decoded}");
            Console.WriteLine();

            // Добавленный функционал: Оценка переполнения
            EvaluateOverflowRisk(message, probabilities);

            // Добавленный функционал: Сравнение с вероятностными алгоритмами (Хаффман)
            CompareWithHuffman(message, code);
        }

        static Dictionary<char, decimal> CalculateProbabilities(string text)
        {
            var counts = text.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            decimal total = text.Length;
            return counts.ToDictionary(kv => kv.Key, kv => (decimal)kv.Value / total);
        }

        static Dictionary<char, decimal> CalculateCumulativeProbabilities(List<char> symbols, Dictionary<char, decimal> probs)
        {
            var cum = new Dictionary<char, decimal>();
            decimal sum = 0m;
            foreach (var sym in symbols)
            {
                cum[sym] = sum;
                sum += probs[sym];
            }
            return cum;
        }

        static decimal Encode(string message, List<char> symbols, Dictionary<char, decimal> cumProbs, Dictionary<char, decimal> probs)
        {
            decimal low = 0m;
            decimal high = 1m;

            Console.WriteLine("Шаги кодирования:");
            Console.WriteLine($"Начальный интервал: low = {low:F6}, high = {high:F6}");

            int step = 1;
            foreach (char c in message)
            {
                decimal range = high - low;
                if (range == 0) break;

                decimal symLow = cumProbs[c];
                decimal symHigh = symLow + probs[c];

                decimal newLow = low + range * symLow;
                decimal newHigh = low + range * symHigh;

                Console.WriteLine($"Шаг {step}: Символ '{c}'");
                Console.WriteLine($"  Текущий интервал: low = {low:F24}, high = {high:F24}");
                Console.WriteLine($"  Границы символа: symLow = {symLow:F24}, symHigh = {symHigh:F24}");
                Console.WriteLine($"  Новый интервал: low = {newLow:F24}, high = {newHigh:F24}\n");

                low = newLow;
                high = newHigh;
                step++;
            }

            return (low + high) / 2m;
        }

        static string Decode(decimal code, int originalLength, List<char> symbols, Dictionary<char, decimal> cumProbs, Dictionary<char, decimal> probs)
        {
            StringBuilder result = new StringBuilder();
            decimal low = 0m;
            decimal high = 1m;

            Console.WriteLine("Шаги декодирования:");
            Console.WriteLine($"Начальный интервал: low = {low:F6}, high = {high:F6}");
            Console.WriteLine($"Код: {code:F24}\n");

            for (int i = 0; i < originalLength; i++)
            {
                decimal range = high - low;
                if (range == 0m)
                {
                    result.Append(symbols.Last());
                    continue;
                }

                decimal scaled = (code - low) / range;

                char chosen = symbols.Last(); // По умолчанию, если scaled == 1.0

                decimal symLow = 0m;
                decimal symHigh = 0m;

                foreach (char sym in symbols)
                {
                    symLow = cumProbs[sym];
                    symHigh = symLow + probs[sym];

                    if (symLow <= scaled && scaled < symHigh)
                    {
                        chosen = sym;
                        break;
                    }
                }

                Console.WriteLine($"Шаг {i + 1}:");
                Console.WriteLine($"  Текущий интервал: low = {low:F24}, high = {high:F24}, range = {range:F24}");
                Console.WriteLine($"  Масштабированный код: {scaled:F24}");
                Console.WriteLine($"  Выбран символ: '{chosen}' (интервал: {symLow:F24} - {symHigh:F24})");

                decimal newLow = low + range * symLow;
                decimal newHigh = low + range * symHigh;

                Console.WriteLine($"  Новый интервал: low = {newLow:F24}, high = {newHigh:F24}\n");

                low = newLow;
                high = newHigh;

                result.Append(chosen);
            }

            return result.ToString();
        }

        // Новый функционал: Оценка возможности переполнения
        static void EvaluateOverflowRisk(string message, Dictionary<char, decimal> probs)
        {
            // Оценка минимального range после всех шагов (приближённо)
            decimal minRange = 1m;
            foreach (char c in message)
            {
                minRange *= probs[c];
            }

            Console.WriteLine("Оценка переполнения:");
            Console.WriteLine($"Минимальный возможный range: {minRange:E} (если все вероятности умножить)");
            if (minRange > Decimal.MinValue && minRange < 1e-28m)
            {
                Console.WriteLine("Риск переполнения ВЫСОКИЙ");
            }
            else if (minRange < 1e-10m)
            {
                Console.WriteLine("Риск переполнения СРЕДНИЙ");
            }
            else
            {
                Console.WriteLine("Риск переполнения НИЗКИЙ");
            }
            Console.WriteLine();
        }

        // Новый функционал: Сравнение с вероятностными алгоритмами (на примере Хаффмана)
        static void CompareWithHuffman(string message, decimal arithCode)
        {
            // 1. Энтропия Шеннона (в битах на символ)
            double entropy = 0.0;
            var probs = CalculateProbabilities(message);
            foreach (var p in probs.Values)
            {
                if (p > 0)
                {
                    double pDouble = (double)p;
                    entropy -= pDouble * Math.Log2(pDouble);
                }
            }

            // 2. Теоретический минимум (предел Шеннона)
            double theoreticalMinBits = entropy * message.Length;

            // 3. Оценка для арифметического кодирования
            // Нам нужно знать, сколько бит требуется для представления числа code с достаточной точностью
            // Примерная формула: -log2(final_range) + немного запаса
            // Но проще: количество бит = ceil(log2(1 / minRange))
            decimal minRange = 1m;
            foreach (char c in message)
            {
                minRange *= probs[c];
            }
            double arithBitsApprox;
            if (minRange > 0)
            {
                arithBitsApprox = -Math.Log2((double)minRange);
            }
            else
            {
                arithBitsApprox = theoreticalMinBits; // fallback
            }
            // + небольшой запас на представление (обычно +1–3 бита)
            double arithBits = Math.Ceiling(arithBitsApprox) + 2;

            // 4. Оценка для Хаффмана
            // Средняя длина кода ≈ энтропия + overhead
            // Overhead ≈ log2(число символов) * 2 (на дерево) + 8–16 бит (заголовок)
            int alphabetSize = probs.Count;
            double huffmanOverhead = Math.Ceiling(Math.Log2(alphabetSize)) * 2 + 16; // грубая оценка
            double huffmanBits = Math.Ceiling(entropy * message.Length) + huffmanOverhead;

            // 5. Вывод сравнения
            Console.WriteLine("Сравнение с методом Хаффмана:");
            Console.WriteLine($"Энтропия Шеннона:               {entropy:F4} бит/символ");
            Console.WriteLine($"Теоретический минимум (H × n):   {theoreticalMinBits:F2} бит");
            Console.WriteLine();
            Console.WriteLine($"Арифметическое кодирование:     ≈ {arithBits:F1} бит (очень близко к пределу)");
            Console.WriteLine($"Хаффман (оценка):                ≈ {huffmanBits:F1} бит (включая доп расходы на дерево)");
            Console.WriteLine();
        }
    }
}