using System;

namespace task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 5;
            byte b = 255;
            float c = 43.27f;
            decimal dc = 334.8m;
            double d = 11.09253;
            char ch = 'a';
            string str = "Hello, World!";
            sbyte sb = -128;
            uint ui = 42U;
            long l = 1234567890L;
            ulong ul = 1234567890UL;
            short sh = -32000;
            ushort ush = 65000;

            Console.WriteLine("Int: " + a);
            Console.WriteLine("Byte: " + b);
            Console.WriteLine("Float: " + c);
            Console.WriteLine("Decimal: " + dc);
            Console.WriteLine("Double: " + d);
            Console.WriteLine("Char: " + ch);
            Console.WriteLine("String: " + str);

            Console.Write("Введите переменную любого типа: ");
            string x = Console.ReadLine();
            Console.WriteLine("Вы ввели: " + x);

            //Неявное приведение
            int a2 = 10;
            double b2 = a2;

            float c2 = 9.8F;
            double d2 = c2;

            byte e = 100;
            int f = e;

            short g = 1000;
            long h = g; 

            int num = 2147483647;
            long bigNum = num;

            //Явное приведение
            double x2 = 10.99;
            int y2 = (int)x2; 

            long bigNum2 = 100000L;
            int intNum = (int)bigNum2;

            float f2 = 3.5F;
            short s2 = (short)f2;

            decimal dec = 123.456M;
            double doub = (double)dec;

            float fl = 12.3838f;
            int fl1 = (int)fl;

            //Convert
            string str2 = "32";
            int age = Convert.ToInt32(str2);

            //упаковка распаковка
            int num3 = 123;     
            object boxed = num3;
            int unboxed = (int)boxed;
            Console.WriteLine($"Упакованное значение: {boxed}, Распакованное значение: {unboxed}");

            //неявно типизирован.
            var int4 = 10;
            var str4 = "Hello";

            //e
            int? val = null;
            Nullable<int> val2 = 5;
            Console.WriteLine("Значения null:");
            Console.WriteLine(val);
            Console.WriteLine(val2);

            var myVar = 5;
            //var myVar = "Hello";
        }
    }
}
