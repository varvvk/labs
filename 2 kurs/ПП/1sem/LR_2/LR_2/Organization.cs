using System;
using System.Collections.Generic;

namespace LR_2
{
    public class Organization : IStaff
    {
        public int Id
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string ShortName
        {
            get;
            protected set;
        }

        public string Address
        {
            get;
            protected set;
        }

        public DateTime TimeStamp
        {
            get;
            protected set;
        }

        public Organization() 
        {
            TimeStamp = DateTime.Now;
        }
        public Organization(Organization other) 
        {
            Id = other.Id;
            Name = other.Name;
            ShortName = other.ShortName;
            Address = other.Address;
            TimeStamp = other.TimeStamp;
        }
        public Organization(string name, string shortName, string address) 
        {
            Name = name;
            ShortName = shortName;
            Address = address;
            TimeStamp = DateTime.Now;
        }

        public void printInfo() 
        {
            Console.WriteLine($"Organization: {Name} ({ShortName}), Address: {Address}, Created on: {TimeStamp} \n");
        }

        private List<Employee> employees = new List<Employee>();
        private List<JobVacancy> jobVacancies = new List<JobVacancy>();
        private List<JobTitle> jobTitles = new List<JobTitle>();

        public List<JobTitle> GetJobTitles()
        {
            return jobTitles;
        }
        public List<JobVacancy> GetJobVacancies()
        {
            return jobVacancies;
        }
        public List<Employee> GetEmployees()
        {
            return employees;
        }

        public int AddJobTitle(JobTitle jobTitle)
        {
            jobTitles.Add(jobTitle);
            return jobTitle.Id;
        }

        public string PrintJobVacancies()
        {
            string result = "";
            foreach (var vacancy in jobVacancies)
            {
                result += $"Vacancy ID: {vacancy.Id}, Title: {vacancy.Title}\n";
            }
            return result;
        }

        public bool DelJobTitle(int id)
        {
            var jobTitle = jobTitles.Find(jt => jt.Id == id);
            if (jobTitle != null)
            {
                jobTitles.Remove(jobTitle);
                return true;
            }
            return false;
        }

        public void OpenJobVacancy(JobVacancy vacancy)
        {
            jobVacancies.Add(vacancy);
        }

        public bool CloseJobVacancy(int id)
        {
            var vacancy = jobVacancies.Find(v => v.Id == id);
            if (vacancy != null)
            {
                jobVacancies.Remove(vacancy);
                return true;
            }
            return false;
        }

        public Employee Recruit(JobVacancy vacancy, Person person)
        {
            var jobVacancy = jobVacancies.Find(jv => jv.Id == vacancy.Id);
            if (jobVacancy == null)
            {
                throw new InvalidOperationException("Vacancy does not exist");
            }

            Employee newEmployee = new Employee
            {
                Id = employees.Count + 1,
                Name = person.Name
            };

            employees.Add(newEmployee);
            return newEmployee;
        }

        public bool Dismiss(int employeeId, Reason reason)
        {
            var employee = employees.Find(e => e.Id == employeeId);
            if (employee != null)
            {
                Console.WriteLine($"Employee {employee.Name} is dismissed. Reason: {reason.Description}");

                employees.Remove(employee);
                return true;
            }
            return false;
        }
    }
}
