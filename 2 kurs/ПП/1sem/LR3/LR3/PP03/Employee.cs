using LR3.Lec03LibN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR3.PP03
{
    public class Employee
    {
        public IBonus bonus { get; private set; }

        public Employee(IBonus bonus)
        {
            this.bonus = bonus;
        }

        public float CalcBonus(float number_hours)
        {
            return bonus.calc(number_hours);
        }
    }
}
