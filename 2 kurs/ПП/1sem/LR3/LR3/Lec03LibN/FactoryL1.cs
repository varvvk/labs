using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class FactoryL1 : IFactory
    {
        public IBonus getA(float cost1hour)
        {
            return new BonusL1A(cost1hour);
        }

        public IBonus getB(float cost1hour, float x)
        {
            return new BonusL1B(cost1hour, x);
        }

        public IBonus getC(float cost1hour, float x, float y)
        {
            return new BonusL1C(cost1hour, x, y);
        }
    }
}
