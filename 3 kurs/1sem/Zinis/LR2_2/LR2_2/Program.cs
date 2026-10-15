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
            string nameMessage = "КазаковаВарвараВадимовна";
            string polishMessage = "KazakowaWarwaraWadimowna";
            string kyrgyzMessage = "КазаковаВарвараВадимовна";

            double H_pol_msg = CalculateMessageEntropy(polishMessage);
            double H_kyr_msg = CalculateMessageEntropy(kyrgyzMessage);

            Console.WriteLine("а) Энтропия сообщения на основе польского алфавита: " + H_pol_msg.ToString("F4") + " бит/символ");
            Console.WriteLine("Энтропия сообщения на основе кыргызского алфавита: " + H_kyr_msg.ToString("F4") + " бит/символ");

            var polishFreq = CalculateSymbolFrequencies(polishMessage);
            var kyrgyzFreq = CalculateSymbolFrequencies(kyrgyzMessage);
            WriteFrequenciesToCsv("polish_frequencies.csv", polishFreq, "Польский алфавит");
            WriteFrequenciesToCsv("kyrgyz_frequencies.csv", kyrgyzFreq, "Кыргызский алфавит");
            Console.WriteLine("Частоты символов записаны в файлы polish_frequencies.csv и kyrgyz_frequencies.csv.");

            var encoding = Encoding.GetEncoding(1251);
            byte[] bytes = encoding.GetBytes(nameMessage);
            string binaryStr = string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            double H_bin = CalculateBinaryEntropy(binaryStr);

            Console.WriteLine("\nб) Двоичный код сообщения \"КазаковаВарвараВадимовна\": " + binaryStr);
            Console.WriteLine("Энтропия бинарного сообщения: " + H_bin.ToString("F4") + " бит/бит");

            int L = nameMessage.Length;
            double I_pol = L * H_pol_msg;
            double I_kyr = L * H_kyr_msg;
            int n_bits = bytes.Length * 8;
            double I_bin = n_bits * H_bin;

            Console.WriteLine("\nв) Количество информации в сообщении \"КазаковаВарвараВадимовна\":");
            Console.WriteLine("На основе польского алфавита: " + I_pol.ToString("F4") + " бит");
            Console.WriteLine("На основе кыргызского алфавита: " + I_kyr.ToString("F4") + " бит");
            Console.WriteLine("В двоичном представлении: " + I_bin.ToString("F4") + " бит");

            // Подсчет для вероятностей ошибки для всех алфавитов
            int unique_pol = polishFreq.Count;
            double logA_pol = Math.Log(unique_pol, 2);
            double n_bits_pol = L * logA_pol;

            int unique_kyr = kyrgyzFreq.Count;
            double logA_kyr = Math.Log(unique_kyr, 2);
            double n_bits_kyr = L * logA_kyr;

            double[] probabilities = { 0.1, 0.5 };
            Console.WriteLine("\nг) Количество информации с вероятностью ошибки передачи бита:");

            Console.WriteLine("Для польского алфавита:");
            foreach (double p in probabilities)
            {
                double h_p = BinaryEntropyFunction(p);
                double C = 1 - h_p;
                double I_g_pol = n_bits_pol * C;
                Console.WriteLine($"Для p={p}: " + I_g_pol.ToString("F4") + " бит");
            }
            Console.WriteLine($"Для p=1.0: 0,0000 бит (Невозможно получить информацию)");

            Console.WriteLine("Для кыргызского алфавита:");
            foreach (double p in probabilities)
            {
                double h_p = BinaryEntropyFunction(p);
                double C = 1 - h_p;
                double I_g_kyr = n_bits_kyr * C;
                Console.WriteLine($"Для p={p}: " + I_g_kyr.ToString("F4") + " бит");
            }
            Console.WriteLine($"Для p=1.0: 0,0000 бит (Невозможно получить информацию)");

            double[] probabilities2 = { 0.1, 0.5, 1.0 };
            Console.WriteLine("Для двоичного представления:");
            foreach (double p in probabilities2)
            {
                double h_p = BinaryEntropyFunction(p);
                double C = 1 - h_p;
                double I_g = n_bits * C;
                Console.WriteLine($"Для p={p}: " + I_g.ToString("F4") + " бит");
            }

            Console.ReadKey();
        }

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

        static double CalculateBinaryEntropy(string binaryStr)
        {
            int count0 = binaryStr.Count(c => c == '0');
            int count1 = binaryStr.Count(c => c == '1');
            int total = binaryStr.Length;
            double p0 = (double)count0 / total;
            double p1 = (double)count1 / total;
            double entropy = 0;
            if (p0 > 0) entropy -= p0 * Math.Log(p0, 2);
            if (p1 > 0) entropy -= p1 * Math.Log(p1, 2);
            return entropy;
        }

        static double BinaryEntropyFunction(double p)
        {
            if (p == 0 || p == 1) return 0;
            return -p * Math.Log(p, 2) - (1 - p) * Math.Log(1 - p, 2);
        }

        static void WriteFrequenciesToCsv(string fileName, Dictionary<char, double> freq, string title)
        {
            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.WriteLine($"{title}");
                writer.WriteLine("Letter;Frequency (%)");
                foreach (var kv in freq.OrderBy(x => x.Key))
                {
                    writer.WriteLine($"{kv.Key};{kv.Value:F2}");
                }
            }
        }
    }
}