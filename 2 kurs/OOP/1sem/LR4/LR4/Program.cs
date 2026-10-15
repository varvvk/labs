using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test("Math Test", DateTime.Now, 90);
            Console.WriteLine(test);
            Console.WriteLine();
            Question question = new Question("Sample Question", DateTime.Now, 15);
            Exam exam = new Exam("Midterm Exam", DateTime.Now, 120);
            FinalExam finalExam = new FinalExam("Final Exam", DateTime.Now, 180);

            List<Trial> trials = new List<Trial> { test, question, exam, finalExam };
            foreach (Trial trial in trials)
            {
                trial.CalculateEndTime();
            }
            Console.WriteLine();

            foreach (var trial in trials)
            {
                Console.WriteLine(trial);

                if (trial is FinalExam)
                {
                    Console.WriteLine("This is a Final Exam.");
                }
                else if (trial is Exam)
                {
                    Console.WriteLine("This is an Exam.");
                }
                else if (trial is Test)
                {
                    Console.WriteLine("This is a Test.");
                }
                else if (trial is Question)
                {
                    Console.WriteLine("This is a Question.");
                }
            }
            Console.WriteLine();

            ICloneable cloneableTrial = test as ICloneable;

            if (cloneableTrial != null)
            {
                Console.WriteLine("trial is derived from ICloneable");
                cloneableTrial.DoClone();
            }
            else
            {
                Console.WriteLine("trial is not derived from ICloneable");
            }
            Console.WriteLine();

            Printer printer = new Printer();

            foreach (Trial trial in trials)
            {
                printer.IAmPrinting(trial);
            }
        }
    }
}
