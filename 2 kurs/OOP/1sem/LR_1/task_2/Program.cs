using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string a = "My name is Varya";
            string b = "My name is Nastya";
            bool c = (a == b);
            Console.WriteLine("Совпадают ли строки a и b? " + c );
            int cMethod = string.Compare(a, b);
            Console.WriteLine("Совпадают ли строки a и b? (С помощью метода) " + cMethod);

            string str1 = "Это тестовая строка";
            string str2 = "Это тоже тестовая";
            string str3 = "И это тестовая строка";

            string concat = string.Concat(str1 + " " + str2);
            Console.WriteLine("Concatenation: " + concat);

            string copy = string.Copy(str1);
            Console.WriteLine("Copied string: " + copy);

            string substr = str3.Substring(6, 8);
            Console.WriteLine("Substring: " + substr);

            string[] splts = str1.Split(' ');
            foreach (var splt in splts)
            {
                Console.WriteLine("Split: " + splt);
            }

            string insert = str2.Insert(17, " строка");
            Console.WriteLine("Insert: " + insert);

            string remove = str3.Remove(6, 8);
            Console.WriteLine("Remove: " + remove);

            Console.WriteLine($" Пример интерполирования: {str2}");

            //null empty
            string s = "";
            string s1 = null;
            bool s2 = string.IsNullOrEmpty(s);
            bool s3 = string.IsNullOrEmpty(s1);
            Console.WriteLine($"Первая и вторая строки являются пустыми или со значение null? {s2} {s3}");

            string d = "Hello";
            string e = null;
            string e2 = null;
            string concat2 = string.Concat(d + e);
            Console.WriteLine("Concatenation: " + concat2);
            bool f = (d == e);
            bool f2 = (e2 == e);
            Console.WriteLine("Совпадают ли строки \"Hello\" и null? " + f);
            Console.WriteLine("Совпадают ли строки null и null? " + f2);

            //StringBuilder
            StringBuilder sb = new StringBuilder("Hello, World!");
            Console.WriteLine(sb);

            sb.Remove(6, 7);
            Console.WriteLine("After removal: " + sb);
            sb.Insert(0, "Start: ");
            sb.Append("Varya");
            Console.WriteLine("After insertions: " + sb);
        }
    }
}
