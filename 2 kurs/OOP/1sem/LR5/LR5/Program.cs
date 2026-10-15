using LR5;
using LR6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6
{
    internal class Program
    {
        public enum TrialStatus
        {
            Planned,
            InProgress,
            Completed
        }
        public struct Student
        {
            public string FirstName;
            public string LastName;
            public int Course;

            public Student(string firstName, string lastName, int course)
            {
                FirstName = firstName;
                LastName = lastName;
                Course = course;
            }

            public void StudentInfo()
            {
                Console.WriteLine($"{FirstName} {LastName} - {Course} course");
            }
        }
        static void Main(string[] args)
        {
            
            Session session = new Session();
            Test test = new Test("Math Test", DateTime.Now, 90);
            
            try
            {
                session.AddTrial(test);
                session.AddTrial(test);
            }
            catch (DuplicateTrialException ex)
            {
                LogError(ex);
            }

            try
            {
                session.RemoveTrial("None Test");
            }
            catch (TrialNotFoundException ex)
            {
                LogError(ex);
            }

            try
            {
                FinalExam finalExam6 = new FinalExam("Final Exam 1", DateTime.Now, 180);
                FinalExam finalExam66 = new FinalExam("Final Exam 2", DateTime.Now, 200);

                session.AddTrial(finalExam6);
                session.AddTrial(finalExam66);
            }
            catch (FinalExamsException ex)
            {
                LogError(ex);
            }

            try
            {
                session.AddTrial(test);
            }
            catch (DuplicateTrialException ex)
            {
                LogError(ex);
                throw;
            }

            try
            {
                Session session2 = new Session();
                Trial trial2 = null;
                session2.AddTrial(trial2);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"[ошибка]");
            }
            finally
            {
                Console.WriteLine("Обработка завершена.");
            }


            void LogError( Exception ex)
            {
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
                Console.WriteLine();
            }
        }
    }
}
