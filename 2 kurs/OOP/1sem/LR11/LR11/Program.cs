using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            House house = new House();
            Software soft1 = new Software("Antivirus", "1.0.0");
            SoftwareCollection<Software> softwareCol = new SoftwareCollection<Software>();
            softwareCol.Add(1, soft1 );
            string str = "Hello";


            Console.WriteLine($"Имя сборки: {Reflector.GetAssemblyName(typeof(House))}");
            Console.WriteLine($"Публ конструкторы: {Reflector.HasPublicConstructors(typeof(House))}");
            Console.WriteLine($"Публ методы : {string.Join(", ", Reflector.GetPublicMethods(typeof(House)))}");
            Console.WriteLine($"Свойства и поля : {string.Join(", ", Reflector.GetFieldAndProperties(typeof(House)))}");
            Console.WriteLine($"Интерфейсы : {string.Join(", ", Reflector.GetInterfaces(typeof(House)))}");
            Reflector.GetMethodsByParameterType(typeof(House), typeof(int));
            Console.WriteLine();

            Reflector.Invoke(house, "ToString");

            File.WriteAllLines("Arguments.txt", new[] { "house" });

            Reflector.Invoke(house, "Equals", "Arguments.txt");



            Console.WriteLine($"Имя сборки: {Reflector.GetAssemblyName(typeof(SoftwareCollection<>))}");
            Console.WriteLine($"Публ конструкторы: {Reflector.HasPublicConstructors(typeof(SoftwareCollection<>))}");
            Console.WriteLine($"Публ методы : {string.Join(", ", Reflector.GetPublicMethods(typeof(SoftwareCollection<>)))}");
            Console.WriteLine($"Свойства и поля : {string.Join(", ", Reflector.GetFieldAndProperties(typeof(SoftwareCollection<>)))}");
            Console.WriteLine($"Интерфейсы: {string.Join(", ", Reflector.GetInterfaces(typeof(SoftwareCollection<>)))}");
            Reflector.GetMethodsByParameterType(typeof(SoftwareCollection<>), typeof(int));
            Console.WriteLine();

            Reflector.Invoke(softwareCol, "DisplayAll");

            File.WriteAllLines("Arguments2.txt", new[] { "1" });

            Reflector.Invoke(softwareCol, "Find", "Arguments2.txt");
            Reflector.Invoke(softwareCol, "Find", null);




            Console.WriteLine(" \n String ");
            Console.WriteLine($"Имя сборки : {Reflector.GetAssemblyName(typeof(string))}");
            Console.WriteLine($"Публ конструкторы: {Reflector.HasPublicConstructors(typeof(string))}");
            Console.WriteLine($"Публ методы: {string.Join(", ", Reflector.GetPublicMethods(typeof(string)))}");
            Console.WriteLine($"Поля и свойства: {string.Join(", ", Reflector.GetFieldAndProperties(typeof(string)))}");
            Console.WriteLine($"Интерфейсы : {string.Join(", ", Reflector.GetInterfaces(typeof(string)))}");
            Reflector.GetMethodsByParameterType(typeof(string), typeof(int));
            Console.WriteLine();

            Reflector.Invoke(str, "GetAssemblyNane");
            Console.WriteLine();

            House house2 = Reflector.Create<House>();

            if (house2 != null)
            {
                Console.WriteLine(house2.Street);
            }

            SoftwareCollection<Software> collection = Reflector.Create<SoftwareCollection<Software>>();
            collection.Add(1, soft1);
            collection.DisplayAll();


        }
    }
}
