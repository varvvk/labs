using System;
using System.Collections.Generic;

namespace LR3
{
    public class Set
    {
        public List<int> elements;

        public Set(List<int> enterElements)
        {
            elements = new List<int>();
            foreach (int element in enterElements)
            {
                if (!elements.Contains(element))
                {
                    elements.Add(element);
                }
            }

            Production production = new Production(1001, "Acme Corporation");
            Developer dev = new Developer("Ivan Ivanov", 1001, "Software Development");
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= elements.Count)
                {
                    Console.WriteLine("Неверный индекс");
                    return -1;
                }
                return elements[index];
            }
        }

        public static Set operator ++(Set set)
        {
            Random rand = new Random();
            int newElement;
            do
            {
                newElement = rand.Next(1, 100);
            } while (set.elements.Contains(newElement));
            set.elements.Add(newElement);
            return set;
        }

        public static Set operator +(Set a, Set b)
        {
            var resultElements = new List<int>(a.elements);
            foreach (int element in b.elements)
            {
                if (!resultElements.Contains(element))
                {
                    resultElements.Add(element);
                }
            }
            return new Set(resultElements);
        }

        public static bool operator <=(Set a, Set b)
        {
            foreach (int element in a.elements)
            {
                if (!b.elements.Contains(element))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator >=(Set a, Set b)
        {
            return b <= a;
        }

        public static implicit operator int(Set set)
        {
            return set.elements.Count;
        }

        public static int operator %(Set set, int index)
        {
            if (index < 0 || index >= set.elements.Count)
                throw new IndexOutOfRangeException("Index is out of range");
            return set.elements[index];
        }

        public void DisplayElements()
        {
            Console.WriteLine("Elements in the Set: ");
            foreach (int element in elements)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
        public override string ToString()
        {
            return $"{{ {string.Join(", ", elements)} }}";
        }

        public class Production
        {
            public int Id { get; private set; }
            public string OrganizationName { get; private set; }

            public Production(int id, string organizationName)
            {
                Id = id;
                OrganizationName = organizationName;
            }
        }

        public class Developer
        {
            public string Fullname { get; set; }
            public int Id { get; set; }
            public string Department { get; set; }

            public Developer(string fullname, int id, string department)
            {
                Fullname = fullname;
                Id = id;
                Department = department;
            }
        }
    }

    public static class StatisticOperation
    {
        public static int Sum(Set set)
        {
            int sum = 0;
            foreach (int element in set.elements)
            {
                sum += element;
            }
            return sum;
        }

        public static int Range(Set set)
        {
            if (set.elements.Count == 0)
                return 0;

            int max = set.elements[0];
            int min = set.elements[0];

            foreach (int element in set.elements)
            {
                if (element > max) max = element;
                if (element < min) min = element;
            }

            return max - min;
        }

        public static int CountElements(Set set)
        {
            return set.elements.Count;
        }

        public static string Encrypt(this string input)
        {
            char[] characters = input.ToCharArray();
            Array.Reverse(characters);
            return new string(characters);
        }

        public static bool IsOrdered(this Set set)
        {
            for (int i = 1; i < set.elements.Count; i++)
            {
                if (set.elements[i - 1] > set.elements[i])
                    return false;
            }
            return true;
        }
    }
}
