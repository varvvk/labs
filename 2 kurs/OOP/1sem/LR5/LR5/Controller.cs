using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    public class Controller
    {
        private Session session;

        public Controller(Session session)
        {
            this.session = session;
        }

        public int GetQuantityBySubject()
        {
            int counter = 0;
            foreach (var trial in session.GetTrials())
            {
                if (trial is Exam || trial is FinalExam)
                {
                    Exam exam = trial as Exam;
                    if (exam != null )
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        public int CountAllTrials()
        {
            return session.GetTrials().Count;
        }

        public int CountTestsByDuration(int duration)
        {
            int counter = 0;
            foreach (var trial in session.GetTrials())
            {
                if (trial is Test)
                {
                    Test test = trial as Test;
                    if (test != null && test.Duration == duration)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
    }
}
