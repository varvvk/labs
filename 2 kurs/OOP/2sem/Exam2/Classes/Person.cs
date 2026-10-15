using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Person
    {
        public int Age;
        public string Name;

        public Person(int age, string name)
        {
            Age = age;
            Name = name;
        }

        public void IsAdult()
        {
            if (Age >= 18)
                Console.WriteLine("Adult");
            else
                Console.WriteLine("Not Adult");
        }
    }
}
