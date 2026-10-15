using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            (int, string, char, string, ulong) tuple = (42, "Hello", 'A', "World", 123456789);

            Console.WriteLine("Кортеж целиком: " + tuple);
            Console.WriteLine($"1 элемент: {tuple.Item1}, 3 элемент: {tuple.Item3}");

            (int num, string str1, char ch, string str2, ulong num2) = tuple;
            Console.WriteLine($"Распакованный кортеж: {num}, {str1}, {ch}, {str2}, {num2}");

            (int numPart, _, char chPart, _, _) = tuple;
            Console.WriteLine($"Частичная распаковка: num: {numPart}, ch: {chPart}");

            var tuple2 = (42, "World", 'A', "Hello", 123456789UL);
            bool areEqual = (tuple == tuple2);
            Console.WriteLine($"Кортежи равны: {areEqual}");

        }
    }
}
