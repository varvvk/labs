using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
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
            Console.WriteLine("Ура! Финальный экзамен окончен");
        }
    }
}
