using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
{
    public class Exam : Trial
    {
        public Exam(string name, DateTime date, int duration) : base(name, date, duration) { }

        public override string ToString()
        {
            return base.ToString() + " (Type: Exam)";
        }

        public override bool DoClone()
        {
            Console.WriteLine("Cloning Exam");
            return true;
        }

        public override void CalculateEndTime()
        {
            base.CalculateEndTime();
            Console.WriteLine("Экзамен окончен");
        }
    }
}
