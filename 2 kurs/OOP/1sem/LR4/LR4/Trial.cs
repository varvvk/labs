using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4
{
    public abstract class Trial
    {
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
}
