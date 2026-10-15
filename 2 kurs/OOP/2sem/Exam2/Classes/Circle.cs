using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Circle
    {
        private double R;

        public Circle(double R)
        { this.R = R; }

        public double Lenth()
        {
            return 2*Math.PI*R;
        }

        public double Square()
        {
            return Math.PI*R*R;
        }
    }
}
