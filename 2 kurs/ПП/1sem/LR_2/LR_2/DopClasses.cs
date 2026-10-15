using System;


namespace LR_2
{
    public class JobVacancy
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class JobTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public class Reason
    {
        public string Description { get; set; }
    }
}
