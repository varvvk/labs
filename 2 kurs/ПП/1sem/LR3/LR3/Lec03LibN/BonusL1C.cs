using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class BonusL1C : IBonus
    {
        public float cost1hour { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public BonusL1C(float cost1hour, float x, float y)
        {
            this.cost1hour = cost1hour;
            X = x;
            Y = y;
        }

        public float calc(float number_hours)
        {
            return number_hours * cost1hour * X + Y;
        }
    }
}
