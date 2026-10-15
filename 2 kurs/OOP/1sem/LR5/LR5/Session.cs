using LR5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LR6
{
    public class Session
    {
        private List<Trial> trials;

        public Session()
        {
            trials = new List<Trial>();
        }

        public void AddTrial(Trial trial)
        {
            if (trials.Exists(t => t.Name == trial.Name))
            {
                throw new DuplicateTrialException($"Испытание с именем '{trial.Name}' уже существует в сессии.");
            }

            if (trial is FinalExam && trials.Exists(t => t is FinalExam))
            {
                throw new FinalExamsException("Сессия уже содержит финальный экзамен. Нельзя добавить больше одного.");
            }

            Debug.Assert(trial != null, "Попытка добавить null-объект в сессию.");

            if (trial == null)
            {
                throw new EmptyTrialException("Невозможно добавить пустой объект в сессию.");
            }

            trials.Add(trial);

        }

        public void RemoveTrial(string trialName)
        {
            Trial trial = trials.Find(t => t.Name == trialName);

            if (trial == null)
            {
                throw new TrialNotFoundException($"Испытание с именем '{trialName}' не найдено.");
            }

            trials.Remove(trial);
        }

        public void PrintTrials()
        {
            Console.WriteLine("\nTrials in the session:");
            if (trials.Count == 0)
            {
                Console.WriteLine("No trials in the session.");
                return;
            }

            foreach (Trial trial in trials)
            {
                Console.WriteLine(trial);
            }
        }

        public List<Trial> GetTrials()
        {
            return trials;
        }
    }
}
