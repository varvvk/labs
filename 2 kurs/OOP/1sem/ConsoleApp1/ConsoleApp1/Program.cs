using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public delegate double MyDelegate(double weight, string destination);

    public class DeliveryService
    {
        public event MyDelegate Calculation;

        public double Standart(double weight, string destination) { Console.WriteLine("st"); return weight * 10; }
        public double Faster(double weight, string destination) { Console.WriteLine("fst"); return weight * 20; }

        public void Start(double weight, string destination)
        {
            Console.WriteLine($"{weight} + {destination}");
            Calculation?.Invoke(weight, destination);
        }
        
    }

    public class Program
    {
        static void Main(string[] args)
        {
            DeliveryService deliveryService = new DeliveryService();

            deliveryService.Calculation += deliveryService.Standart;
            deliveryService.Calculation += deliveryService.Faster;

            deliveryService.Start(4.5, "minsk");


        }
    }
}
