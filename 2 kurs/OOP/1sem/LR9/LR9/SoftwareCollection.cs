using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LR9
{
    public class SoftwareCollection<T> : IList<T>
    {
        public SortedList<int, T> Elements;

        public SoftwareCollection() { Elements = new SortedList<int, T>(); }

        public void Add(int key, T item)
        {

            if (Elements.ContainsKey(key))
            {
                Console.WriteLine($"Key {key} already exists. Element not added.");
            }
            else
            {
                Elements.Add(key, item);
                Console.WriteLine($"Element \"{item}\" added with key {key}.");
            }
        }

        public void Remove(int key)
        {
            if (Elements.ContainsKey(key))
            {
                Elements.Remove(key);
                Console.WriteLine($"Element with key {key} removed");
            }
            else
            {
                Console.WriteLine($"Key {key} not found.");
            }
        }

        public void Find(int key)
        {
            if (Elements.ContainsKey(key))
            {
                Console.WriteLine($"Item with key {key} found: {Elements[key]}");
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        public void DisplayAll()
        {
            Console.WriteLine("Displaying all items in the collection:");
            foreach (var element in Elements)
            {
                Console.WriteLine(element);
            }
        }
    }
}
