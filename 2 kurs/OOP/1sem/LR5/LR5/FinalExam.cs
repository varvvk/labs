using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    
    sealed class FinalExam : Exam
    {
        public FinalExam(string name, DateTime date, int duration) : base(name, date, duration) { }

        public override string ToString()
        {
            return base.ToString() + " (Type: FinalExam)";
        }

        public override void CalculateEndTime()
        {
            base.CalculateEndTime();
            Console.WriteLine("This is the final exam, so manage your time carefully.");
        }
    }
}
