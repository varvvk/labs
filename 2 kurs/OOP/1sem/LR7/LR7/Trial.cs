using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR7
{
    public class GenericClass<T> : IInterface<T>
    {
        private List<T> items;

        public GenericClass()
        {
            items = new List<T>();
        }
        public void Add(T item)
        {
            items.Add(item);
            Console.WriteLine("Item added.");
        }
        public bool Remove(T item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
                Console.WriteLine("Item removed.");
                return true;
            }
            else
            {
                Console.WriteLine("Item not found.");
                return false;
            }
        }
        public void View()
        {
            Console.WriteLine("Items in the collection:");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class Trial
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }

        public Trial(string name, DateTime date, int duration)
        {
            Name = name;
            Date = date;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"Trial: {Name}, Date: {Date.ToShortDateString()}, Duration: {Duration} minutes";
        }

        public virtual void CalculateEndTime()
        {
            DateTime endTime = Date.AddMinutes(Duration);
            Console.WriteLine($"The '{Name}' trial will end at {endTime:HH:mm}.");
        }
    }
}
