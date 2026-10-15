using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Product
    {
        private string name;
        private double price;

        public Product(string name, double price)
        {
            this.name = name;
            this.price = price;
        }

        public void ApplyDiscount(double percent)
        {
            double discountAmount = price * (percent / 100);
            price -= discountAmount;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Товар: {name}, Цена: {price:F2} руб.");
        }
    }
}
