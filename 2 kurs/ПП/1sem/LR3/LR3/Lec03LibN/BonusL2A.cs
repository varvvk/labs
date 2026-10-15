using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL2A : IBonus
    {
        public float cost1hour { get; set; }
        public float A { get; set; }

        public BonusL2A(float cost1hour, float a)
        {
            this.cost1hour = cost1hour;
            A = a;
        }

        public float calc(float number_hours)
        {
            return (number_hours + A) * cost1hour;
        }
    }
}
