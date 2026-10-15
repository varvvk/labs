using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL2C : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public BonusL2C(float cost1hour, float a, float x, float y)
        {
            this.cost1hour = cost1hour;
            A = a;
            X = x;
            Y = y;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * cost1hour * X + Y;
        }
    }
}
