using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Rectangle
    {
        public int x;
        public int y;

        public Rectangle(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void GetArea(Rectangle rec)
        {
            Console.WriteLine( $"Area: {x*y}");
        }

        public void GetPerimeter(Rectangle rec)
        {
            Console.WriteLine($"Perimeter: {(x + y) * 2}");
        }
    }
}
