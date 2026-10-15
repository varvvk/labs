using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace LR7
{
    public class CollectionType<T> : IInterface<T> where T:struct
    {
        public List<T> Elements;

        public CollectionType(List<T> enterElements)
        {
            Elements = new List<T>();
            foreach (T element in enterElements)
            {
                if (!Elements.Contains(element))
                {
                    Elements.Add(element);
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Elements.Count)
                {
                    Console.WriteLine("Неверный индекс");
                    return default;
                }
                return Elements[index];
            }
        }

        public static CollectionType<T> operator ++(CollectionType<T> collection)
        {
            if (typeof(T).IsValueType && (typeof(T) == typeof(int) || typeof(T) == typeof(double) || typeof(T) == typeof(float)))
            {
                Random rand = new Random();
                T newElement;

                do
                {
                    if (typeof(T) == typeof(int))
                        newElement = (T)(object)rand.Next(1, 100);
                    else if (typeof(T) == typeof(double))
                        newElement = (T)(object)(rand.NextDouble() * 100);
                    else if (typeof(T) == typeof(float))
                        newElement = (T)(object)((float)rand.NextDouble() * 100);
                    else
                        throw new InvalidOperationException("Unsupported type.");
                }
                while (collection.Elements.Contains(newElement));

                collection.Elements.Add(newElement);
            }
            else
            {
                throw new InvalidOperationException("The type T must be a numeric type.");
            }

            return collection;
        }
        
        public static CollectionType<T> operator +(CollectionType<T> a, CollectionType<T> b)
        {
            if (a.GetType() != b.GetType())
                throw new InvalidOperationException("Collections must be of the same type.");
            var resultElements = new List<T>(a.Elements);
            foreach (T element in b.Elements)
            {
                if (!resultElements.Contains(element))
                {
                    resultElements.Add(element);
                }
            }
            return new CollectionType<T>(resultElements);
        }

        public static bool operator <=(CollectionType<T> a, CollectionType<T> b)
        {
            foreach (T element in a.Elements)
            {
                if (!b.Elements.Contains(element))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator >=(CollectionType<T> a, CollectionType<T> b)
        {
            return b <= a;
        }

        public static implicit operator int(CollectionType<T> collection)
        {
            return collection.Elements.Count;
        }

        public static T operator %(CollectionType<T> collection, int index)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection[index]; 
        }

        public void DisplayElements()
        {
            Console.WriteLine("Elements in the Collection: ");
            foreach (T element in Elements)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }

        public override string ToString()
        {
            return $"{{ {string.Join(", ", Elements)} }}";
        }

        public void Add(T item)
        {
            try
            {
                Elements.Add(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Добавление элемента завершено.");
            }
        }

        public bool Remove(T item)
        {
            try
            {
                bool removed = Elements.Remove(item);
                if (!removed)
                {
                    throw new InvalidOperationException("Элемент не найден для удаления");
                }
                return true;
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Удаление элемента завершено.");
            }
            return false;
        }

        public void View()
        {
            try
            {
                if (Elements.Count == 0)
                {
                    throw new InvalidOperationException("Коллекция пуста");
                }

                Console.WriteLine("Элементы коллекции:");
                foreach (var item in Elements)
                {
                    Console.WriteLine(item);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Просмотр элементов завершен.");
            }
        }

        public void SaveToFile(string filePath, string format)
        {

            if (format.ToLower() == "json")
            {
                string json = JsonSerializer.Serialize(Elements);
                File.WriteAllText(filePath, json);
                Console.WriteLine("Данные сохранены в JSON файл.");
            }

            else if (format.ToLower() == "xml")
            {
                XmlSerializer xmlSerializer = new XmlSerializer(Elements.GetType());
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    xmlSerializer.Serialize(writer, Elements);
                }
                Console.WriteLine("Данные сохранены в XML файл.");
            }
            else
            {
                Console.WriteLine("Неизвестный формат.");
            }
        }

        public void LoadFromFile(string filePath, string format)
        {
            if (format.ToLower() == "json")
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    Elements = JsonSerializer.Deserialize<List<T>>(json);
                    Console.WriteLine("Данные из JSON файла успешно загружены:");
                    foreach (var element in Elements)
                    {
                        Console.WriteLine(element);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении JSON файла: {ex.Message}");
                }
            }
            else if (format.ToLower() == "xml")
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        Elements = (List<T>)xmlSerializer.Deserialize(reader);
                    }
                    Console.WriteLine("Данные из XML файла успешно загружены:");
                    foreach (var element in Elements)
                    {
                        Console.WriteLine(element);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении XML файла: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Неизвестный формат.");
            }
        }


    }

    public static class StatisticOperation
    {
        public static int Sum(CollectionType<int> set)
        {
            int sum = 0;
            foreach (int element in set.Elements)
            {
                sum += element;
            }
            return sum;
        }

        public static int Range(CollectionType<int> set)
        {
            if (set.Elements.Count == 0)
                return 0;

            int max = set.Elements[0];
            int min = set.Elements[0];

            foreach (int element in set.Elements)
            {
                if (element > max) max = element;
                if (element < min) min = element;
            }

            return max - min;
        }

        public static int CountElements(CollectionType<int> set)
        {
            return set.Elements.Count;
        }

        public static string Encrypt(this string input)
        {
            char[] characters = input.ToCharArray();
            Array.Reverse(characters);
            return new string(characters);
        }

        public static bool IsOrdered(this CollectionType<int> set)
        {
            for (int i = 1; i < set.Elements.Count; i++)
            {
                if (set.Elements[i - 1] > set.Elements[i])
                    return false;
            }
            return true;
        }
    }
}
