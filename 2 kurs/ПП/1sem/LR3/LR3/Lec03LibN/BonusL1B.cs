using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL1B : IBonus
    {
        public float cost1hour { get; set; }
        public float X { get; set; }

        public BonusL1B(float cost1hour, float x)
        {
            this.cost1hour = cost1hour;
            X = x;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour * X;
        }
    }
}
