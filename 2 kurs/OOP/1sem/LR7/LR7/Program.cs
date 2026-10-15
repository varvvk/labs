using LR7;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LR7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var setA = new CollectionType<double>(new List<double> { 1.89, 2, 34, 2.75, 3 });
            setA.DisplayElements();

            var setB = new CollectionType<int>(new List<int> { 7, 8, 2 });
            setB.DisplayElements();

            var setC = new CollectionType<int>(new List<int> { 7, 8});
            setC.DisplayElements();

            Console.WriteLine($"Collection A after ++: {++setA}");

            var unionSet = setB + setC;
            Console.WriteLine($"\nUnion of Collections B and C using + operator: {unionSet}");

            Console.WriteLine("\nПроверка, является ли множество C подмножеством множества B с использованием оператора <=: " + (setC <= setB));
            Console.WriteLine("\nПроверка, является ли множество C надмножеством множества B с использованием оператора >=: " + (setC >= setB));

            int powerOfSetA = setA;
            Console.WriteLine("\nPower of Collection A (converted to int): " + powerOfSetA);

            double elementAtIndex2 = setA % 2;
            Console.WriteLine($"Элемент под индексом: {elementAtIndex2}");

            Console.WriteLine("\nUsing Add, Remove, and View methods:");
            setB.Add(10);
            setB.Remove(7);
            setB.View();


            Console.WriteLine("\nСумма элементов в множестве A: " + StatisticOperation.Sum(setB));
            Console.WriteLine("Разность (макс-мин) элементов в множестве A: " + StatisticOperation.Range(setB));
            Console.WriteLine("Количество элементов в множестве A: " + StatisticOperation.CountElements(setB));

            string filePathJson = "data.json";
            setA.SaveToFile(filePathJson, "json");
            setA.LoadFromFile(filePathJson, "json");

            string filePathXml = "data.xml";
            setB.SaveToFile(filePathXml, "xml");
            setB.LoadFromFile(filePathXml, "xml");

            Console.WriteLine();

            GenericClass<Trial> trialCollection = new GenericClass<Trial>();

            Trial trial1 = new Trial("Trial 1", DateTime.Now, 30);
            Trial trial2 = new Trial("Trial 2", DateTime.Now.AddDays(1), 45);

            trialCollection.Add(trial1);
            trialCollection.Add(trial2);

            trialCollection.View();

            trialCollection.Remove(trial1);
            trialCollection.View();
        }
    }
}