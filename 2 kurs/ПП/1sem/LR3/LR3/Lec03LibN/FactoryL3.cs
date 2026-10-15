using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.Lec03LibN
{
    internal class FactoryL3 : IFactory
    {
        public float A { get; set; }
        public float B { get; set; }

        public FactoryL3(float a, float b)
        {
            A = a;
            B = b;
        }

        public IBonus getA(float cost1hour)
        {
            return new BonusL3A(cost1hour, A, B);
        }

        public IBonus getB(float cost1hour, float x)
        {
            return new BonusL3B(cost1hour, A, B, x);
        }

        public IBonus getC(float cost1hour, float x, float y)
        {
            return new BonusL3C(cost1hour, A, B, x, y);
        }
    }
}
