using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL3C : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public BonusL3C(float cost1hour, float a, float b, float x, float y)
        {
            this.cost1hour = cost1hour;
            A = a;
            B = b;
            X = x;
            Y = y;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * (cost1hour + B) * X + Y;
        }
    }
}
