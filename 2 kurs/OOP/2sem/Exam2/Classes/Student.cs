using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Student
    {
        private string Name;
        private int Age;
        private List<int> Grades;

        public Student(string name, int age, List<int> grades)
        {
            Name = name;
            Age = age;
            Grades = grades;
        }

        public double CalculateAverage()
        {
            return Grades.Average(); 
        }
    }
}
