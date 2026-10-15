using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Movie
    {
        private string Name;
        private string Director;
        private DateTime Year;

        public Movie(string name, string director, DateTime year) 
        { 
            Name = name;
            Director = director;
            Year = year;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Name: {Name}, year: {Year.Year}");
        }
    }
}
