using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL3B : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float X { get; set; }

        public BonusL3B(float cost1hour, float a, float b, float x)
        {
            this.cost1hour = cost1hour;
            A = a;
            B = b;
            X = x;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * (cost1hour + B) * X;
        }
    }
}
