using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class FactoryL2 : IFactory
    {
        public float A { get; set; }

        public FactoryL2(float a)
        {
            A = a;
        }

        public IBonus getA(float cost1hour)
        {
            return new BonusL2A(cost1hour, A);
        }

        public IBonus getB(float cost1hour, float x)
        {
            return new BonusL2B(cost1hour, A, x);
        }

        public IBonus getC(float cost1hour, float x, float y)
        {
            return new BonusL2C(cost1hour, A, x, y);
        }
    }
}
