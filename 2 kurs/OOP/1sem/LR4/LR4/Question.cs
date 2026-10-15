using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
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
            Console.WriteLine("Вопросы закончились");
        }
    }
}
