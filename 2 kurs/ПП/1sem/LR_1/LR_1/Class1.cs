using System;

namespace LR_1
{
    internal class Student
    {
        private const string univ = "BSTU";
        public const string faculty = "IT";
        protected const int grade = 1;

        private int id;
        public string surname;
        public string name;
        protected string address;

        private string phone { get; set; }
        public string Address { 
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
        protected string status { get; set; }

        public Student()
        {
            surname = "Unknown";
            name = "Unknown";
            phone = "0";
            address = "Unknown";
            id = GenerateID();
            status = "None";
        }
        public Student(string surname, string name, string phone, string address, string status)
        {
            this.surname = surname;
            this.name = name;
            this.phone = phone;
            this.address = address;
            id = GenerateID();
            this.status = status;
        }
        public Student(Student other)
        {
            surname = other.surname;
            name = other.name;
            phone = other.phone;
            address = other.address;
            id = GenerateID();
            status = other.status;
        }

        private int GenerateID()
        {
            return new Random().Next(1000, 9999);
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Surname: {surname}, Name: {name}, Phone: {phone}, Address: {address}, Status: {status}, University: {univ}, Faculty: {faculty}, Grade: {grade}");
        }

        protected string ProtectedInfo()
        {
            return $" Адрес: {address}";
        }
        public void ShowProtectedInfo()
        {
            Console.WriteLine("Protected info: " + ProtectedInfo());
        }

        private string PrivateInfo()
        {
            return $"ID {surname}: {id}";
        }
        public void ShowPrivateInfo()
        {
            Console.WriteLine(PrivateInfo());
        }
    }
}
