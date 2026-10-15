using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public partial class Test : Trial, ICloneable
    {
        public Test(string name, DateTime date, int duration) : base(name, date, duration) { }

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
            Console.WriteLine("Remember to finish all test questions within this allocated time.");
        }
    }
}
