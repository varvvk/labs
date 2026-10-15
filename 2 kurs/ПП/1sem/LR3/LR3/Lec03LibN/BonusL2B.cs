using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL2B : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }
        public float X { get; set; }

        public BonusL2B(float cost1hour, float a, float x)
        {
            this.cost1hour = cost1hour;
            A = a;
            X = x;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * cost1hour * X;
        }
    }
}
