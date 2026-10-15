using System;
using System.Collections.Generic;

namespace LR_2
{
    public interface IStaff
    {
        List<JobVacancy> GetJobVacancies();
        List<Employee> GetEmployees();
        List<JobTitle> GetJobTitles();
        int AddJobTitle(JobTitle jobTitle);
        string PrintJobVacancies();
        bool DelJobTitle(int id);
        void OpenJobVacancy(JobVacancy vacancy);
        bool CloseJobVacancy(int id);
        Employee Recruit(JobVacancy vacancy, Person person);
        bool Dismiss(int employeeId, Reason reason);
    }
}
