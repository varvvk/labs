using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Books
    {
        private string Name;
        private string Author;
        private string Description;

        public Books(string name, string author, string description) 
        {
            Name = name;
            Author = author;
            Description = description;
        }

        public void PrintBooks()
        {
            Console.WriteLine($"Name: {Name}, Author: {Author}, Description: {Description}");
        }
    }
}
