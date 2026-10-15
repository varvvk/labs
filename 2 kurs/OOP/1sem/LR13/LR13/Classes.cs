using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Linq;

namespace LR13
{
    public abstract class Trial
    {
        [JsonIgnore]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }

        public Trial(string name, DateTime date, int duration)
        {
            Name = name;
            Date = date;
            Duration = duration;
        }

        public override string ToString()
        {
            return $"Trial: {Name}, Date: {Date.ToShortDateString()}, Duration: {Duration} minutes";
        }

        public abstract bool DoClone();

        public virtual void CalculateEndTime()
        {
            DateTime endTime = Date.AddMinutes(Duration);
            Console.WriteLine($"The '{Name}' trial will end at {endTime:HH:mm}.");
        }
    }
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
    [Serializable]
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
