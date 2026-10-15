using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] myArray = 
            {
                {0, 1, 2, 3 }, 
                {4,5,6,7 }, 
                {8,9,10,11 }
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++) 
                {
                    Console.Write($"\t{myArray[i,j]}");
                }
                Console.WriteLine();
            }

            //одномерный массив строк
            string[] myArray2 = { "Varya", "Nastya", "Masha", "Dasha" };
            for (int i = 0;i < 4; i++) 
            {
                Console.WriteLine(myArray2[i]);
            }
            Console.WriteLine($"Длина массива: {myArray2.Length}");

            Console.Write("Введите позицию элемента: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите новое значение: ");
            string newValue = Console.ReadLine();

            if (a >= 0 && a < myArray2.Length)
            {
                myArray2[a] = newValue;
            }
            else
            {
                Console.WriteLine("Ошибка: индекс вне диапазона массива.");
            }

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(myArray2[i]);
            }

            //ступечатый (не выровненный) массив 
            double[][] myArray3 =
            {
                new double[2],
                new double[3],
                new double[4]
            };

            for(int i = 0; i< myArray3.Length; i++)
            {
                for(int j = 0;j < myArray3[i].Length; j++)
                {
                    Console.WriteLine($"Введите элемент [{i}][{j}] массива: ");
                    myArray3[i][j] = Convert.ToDouble(Console.ReadLine());
                }
            }
            for (int i = 0; i < myArray3.Length; i++)
            {
                for (int j = 0; j < myArray3[i].Length; j++)
                {
                    Console.Write($"\t{myArray3[i][j]}");
                }
                Console.WriteLine();
            }

            //неявно типизированные переменные
            var numbers = new[] { 1, 2, 3, 4, 5 };
            var text = "Hello, World!";
        }
    }
}
