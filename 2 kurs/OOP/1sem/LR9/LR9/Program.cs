using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LR9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SoftwareCollection<Software> softwareCollection = new SoftwareCollection<Software>();

            Software soft1 = new Software("Antivirus", "1.0.0");
            Software soft2 = new Software("Google Chrome", "1.1.1");

            softwareCollection.Add(1, soft1);
            softwareCollection.Add(2, soft2);
            Console.WriteLine();
            softwareCollection.DisplayAll();
            softwareCollection.Remove(1);
            softwareCollection.DisplayAll();
            softwareCollection.Find(2);
            Console.WriteLine();

            //2

            SortedList<int, char> sortedList = new SortedList<int, char>();

            sortedList.Add(1, 'A');
            sortedList.Add(2, 'C');
            sortedList.Add(3, 'B');

            Console.WriteLine("Исходная коллекция SortedList:");
            foreach (var item in sortedList)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
            }

            int n = 2;
            for (int i = 0; i < n; i++)
            {
                sortedList.RemoveAt(0);
            }

            Console.WriteLine("\nКоллекция после удаления элементов:");
            foreach (var item in sortedList)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
            }

            sortedList.Add(6, 'F');
            sortedList[7] = 'G'; 

            Console.WriteLine("\nКоллекция после добавления элементов:");
            foreach (var item in sortedList)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
            }


            Dictionary<int, char> dictionary = new Dictionary<int, char>();

            foreach (var item in sortedList)
            {
                dictionary[item.Key] = item.Value;
            }

            Console.WriteLine("\nКоллекция Dictionary:");
            foreach (var item in dictionary)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значение: {item.Value}");
            }

            char searchValue = 'F';
            if (dictionary.ContainsValue(searchValue))
            {
                Console.WriteLine($"\nЗначение '{searchValue}' найдено в коллекции.");
            }
            else
            {
                Console.WriteLine($"\nЗначение '{searchValue}' не найдено в коллекции.");
            }

            Console.WriteLine();

            //3

            ObservableCollection<Software> softwareCollection2 = new ObservableCollection<Software>();

            softwareCollection2.CollectionChanged += OnCollectionChanged;
            
            softwareCollection2.Add(new Software("Visual Studio", "2022"));
            softwareCollection2.Add(new Software("IntelliJ IDEA", "2023"));
            softwareCollection2.Add(new Software("Eclipse", "4.22"));

            softwareCollection2.RemoveAt(0); 
            softwareCollection2.Remove(softwareCollection2[0]);

            Console.WriteLine("Содержимое коллекции:");
            foreach (var item in softwareCollection2)
            {
                Console.WriteLine(item);
            }
        }

        private static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Software software in e.NewItems)
                {
                    Console.WriteLine($"Добавлен элемент: {software}");
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Software software in e.OldItems)
                {
                    Console.WriteLine($"Удалён элемент: {software}");
                }
            }
        }
    }
}
