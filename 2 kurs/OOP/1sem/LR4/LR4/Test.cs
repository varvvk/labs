using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
{
    public class Test : Trial, ICloneable
    {
        public Test(string name, DateTime date, int duration) : base(name, date, duration) { }

        public override string ToString()
        {
            return base.ToString() + " (Type: Test)";
        }

        public override bool Equals(object obj)
        {
            if (obj is Test otherTest)
            {
                return Name == otherTest.Name && Date == otherTest.Date && Duration == otherTest.Duration;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Date, Duration);
        }

        public override bool DoClone()
        {
            Console.WriteLine("Cloning using Trial's DoClone method");
            return true;
        }

        bool ICloneable.DoClone()
        {
            Console.WriteLine("Cloning using ICloneable's DoClone method");
            return false;
        }

        public override void CalculateEndTime()
        {
            base.CalculateEndTime();
            Console.WriteLine("Тест завершен");
        }
    }
}
