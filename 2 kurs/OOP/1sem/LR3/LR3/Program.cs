using System;
using System.Collections.Generic;

namespace LR3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Set setA = new Set(new List<int> { 1, 2, 2, 3 });
            setA.DisplayElements();

            Set setB = new Set(new List<int> { 7, 8, 3 });
            setB.DisplayElements();

            Set setC = new Set(new List<int> { 7, 8 });
            setC.DisplayElements();

            Console.WriteLine($"Множество A после ++: {++setA}");

            Set unionSet = setA + setB;
            Console.WriteLine($"\nОбъединение множеств A и B с использованием оператора +: {unionSet}");

            Console.WriteLine("\nПроверка, является ли множество C подмножеством множества B с использованием оператора <=: " + (setC <= setB));
            Console.WriteLine("\nПроверка, является ли множество C надмножеством множества B с использованием оператора >=: " + (setC >= setB));

            int powerOfSetA = setA;
            Console.WriteLine("\nМощность множества A (преобразование в int): " + powerOfSetA);

            int elementAtIndex2 = setA % 2;
            Console.WriteLine($"Элемент с указанной позиции: {elementAtIndex2}");

            Console.WriteLine("\nСумма элементов в множестве A: " + StatisticOperation.Sum(setA));
            Console.WriteLine("Разность (макс-мин) элементов в множестве A: " + StatisticOperation.Range(setA));
            Console.WriteLine("Количество элементов в множестве A: " + StatisticOperation.CountElements(setA));

            string originalText = "Привет, мир!";
            string encryptedText = originalText.Encrypt();
            Console.WriteLine("\nИсходный текст: " + originalText);
            Console.WriteLine("Зашифрованный текст: " + encryptedText);

            Console.WriteLine("\nПроверка, упорядочено ли множество A: " + setA.IsOrdered());
        }
    }
}