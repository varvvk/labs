using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL3A : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }
        public float B { get; set; }

        public BonusL3A(float cost1hour, float a, float b)
        {
            this.cost1hour = cost1hour;
            A = a;
            B = b;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * (cost1hour + B);
        }
    }
}
