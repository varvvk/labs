using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR10
{
    internal class Program
    {
        public class Resident
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public int FlatNumber { get; set; }

            public Resident(string name, string street, int flatNumber)
            {
                Name = name;
                Street = street;
                FlatNumber = flatNumber;
            }
        }

        static void Main(string[] args)
        {
            //1

            string[] months =
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };

            var month = months.Where(n => n.Length == 8).Select(n => n);
            foreach (var n in month) { Console.WriteLine(n); }
            Console.WriteLine();

            var month2 = from n in months
                                        where n == "June" || n == "July" || n == "August" ||
                                              n == "December" || n == "January" || n == "February"
                                        select n;
           foreach(var n in month2) { Console.WriteLine(n); }
            Console.WriteLine();

            var month3 = months.OrderBy(n=>n).Select(n => n);
            foreach(var n in month3) { Console.WriteLine(n); }
            Console.WriteLine();

            var month4 = months.Where(n => n.Contains('u') & n.Length >= 4).Select(n => n);
            foreach( var n in month4) { Console.WriteLine(n); }
            Console.WriteLine();

            //2

            List<House> collectionHouses = new List<House>()
            {
                new House(101, 50, 3, 2, "Blue Street", "Brick", 10, 15),
                new House(102, 65, 5, 3, "Green Street", "Panel", 12,4),
                new House(103, 75, 8, 3, "Red Street", "Monolithic", 15, 6),
                new House(104, 40, 2, 1, "Yellow Street", "Brick", 8, 7),
                new House(105, 90, 10, 4, "Pink Street", "Panel", 20, 6),
                new House(106, 50, 3, 2, "Pink Street", "Brick", 10, 53),
                new House(107, 65, 2, 3, "Black Street", "Panel", 12, 23),
                new House(102, 75, 10, 3, "Blue Street", "Brick", 10, 15),
                new House(103, 40, 2, 1, "Blue Street", "Brick", 10, 15),
                new House(104, 40, 2, 1, "Blue Street", "Brick", 10, 15),
                new House(105, 40, 2, 1, "Blue Street", "Brick", 11, 25),
                new House(106, 40, 2, 1, "Blue Street", "Brick", 10, 15),
                new House(107, 40, 2, 1, "Blue Street", "Brick", 10, 15),
                new House(110, 90, 10, 4, "Brown Street", "Panel", 20, 1)
            };

            Console.WriteLine("\nFlats whith 3 rooms:");
            var house1 =
                collectionHouses.Where(n => n.Rooms.Equals(3))
                .Select(s => s);
            foreach(var x in house1) { Console.WriteLine($"Flat number: {x.FlatNumber}, Floor: {x.Floor}"); }

            Console.WriteLine("\nFirst 5 flats at Blue st. house 10");
            var house2 =
                collectionHouses.Where(n => n.Street.Equals("Blue Street") & n.HouseNumber == 15)
                .Take(5);
            foreach(var x in house2) { Console.WriteLine($"{x.FlatNumber}, {x.Street}, {x.HouseNumber}"); }
            Console.WriteLine() ;

            var house3 =
                collectionHouses.Where(p => p.Street == "Pink Street")
                .Count();
            Console.WriteLine($"Amount of flats on Pink st. : {house3} \n");

            Console.WriteLine("Flats between 2 and 10 floor");
            var house4 =
                collectionHouses.Where(n => n.Rooms == 3)
                .TakeWhile(t => t.Floor > 2 & t.Floor < 10);
            foreach(var x in house4) { Console.WriteLine($"Flat: {x.FlatNumber}, amount of rooms: {x.Rooms}, floor: {x.Floor}"); }
            Console.WriteLine();

            //3

            var selectedHouses = collectionHouses
                .Where(n => n.Area > 65)
                .OrderBy(t => t.Area)
                .Select(s => s)
                .GroupBy(g => g)
                .Where(a => a.All(b => b.Floor > 8))
                .Count();

            Console.WriteLine($"Количество этажей выше 8, на которых площадь квартир больше 65 {selectedHouses} \n");

            //4
            List<Resident> residents = new List<Resident>()
            {
                new Resident("Alice", "Blue Street", 101),
                new Resident("Lisa", "Blue Street", 102),
                new Resident("Eva", "Blue Street", 103),
                new Resident("Maksim", "Blue Street", 104),
                new Resident("Petr", "Blue Street", 105),
                new Resident("Mark", "Blue Street", 106),
                new Resident("Nastya", "Blue Street", 107),
            };

            var joinedCollection =
                from b in house2
                join
                c in residents on b.FlatNumber equals c.FlatNumber
                select new { FlatNumber = b.FlatNumber, Name = c.Name };

            foreach (var x in joinedCollection) {  Console.WriteLine($"Number of flat: {x.FlatNumber} and owner {x.Name}"); }


        }
    }
}
