using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class Question : Trial
    {
        public Question(string name, DateTime date, int duration) : base(name, date, duration) { }

        public override string ToString()
        {
            return base.ToString() + " (Type: Question)";
        }

        public override bool DoClone()
        {
            Console.WriteLine("Cloning Question");
            return true;
        }

        public override void CalculateEndTime()
        {
            base.CalculateEndTime();
            Console.WriteLine("Answer each question thoughtfully within the allocated time.");
        }
    }
}
