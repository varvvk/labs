using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL1A : IBonus
    {
        public float cost1hour { get; set; }

        public BonusL1A(float cost1hour)
        {
            this.cost1hour = cost1hour;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour;
        }
    }
}
