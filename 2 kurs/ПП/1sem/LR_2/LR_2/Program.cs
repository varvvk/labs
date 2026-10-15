using System;
using System.Collections.Generic;

namespace LR_2
{
    class Program
    {
        static void Main(string[] args)
        {
            University university = new University("Tech University", "TU", "123 University Ave.");
            university.PrintInfo();

            Faculty faculty = new Faculty("Engineering Faculty", "EF", "Engineering Block");
            university.AddFaculty(faculty);
            faculty.PrintInfo();

            Faculty faculty2 = new Faculty("PrintTechnology Faculty", "EF", "Engineering Block");
            university.AddFaculty(faculty2);

            Department department = new Department { Id = 1, Name = "Computer Science" };
            faculty.AddDepartment(department);
            Console.WriteLine($"Добавлен департамент: {department.Name}");

            JobTitle professor = new JobTitle { Id = 1, Title = "Professor" };
            university.AddJobTitle(professor);
            Console.WriteLine($"Добавлена должность: {professor.Title}");

            JobVacancy vacancy = new JobVacancy { Id = 1, Title = "Computer Science Professor" };
            university.OpenJobVacancy(vacancy);
            Console.WriteLine("Открытые вакансии:");
            Console.WriteLine(university.PrintJobVacancies());

            Person candidate = new Person { Name = "Alice Johnson" };
            Employee newEmployee = university.Recruit(vacancy, candidate);
            Console.WriteLine($"Новый сотрудник: {newEmployee.Name} нанят на должность {vacancy.Title}");

            Reason dismissalReason = new Reason { Description = "End of contract" };
            university.Dismiss(newEmployee.Id, dismissalReason);

            bool closeVacancyResult = university.CloseJobVacancy(vacancy.Id);
            Console.WriteLine(closeVacancyResult ? "Вакансия закрыта" : "Ошибка закрытия вакансии");

            Console.WriteLine();
            university.PrintInfo();

            Console.WriteLine("\nУдаление факультета:");
            bool deleteFacultyResult = university.DelFaculty(0); 
            Console.WriteLine(deleteFacultyResult ? "Факультет удалён\n" : "Ошибка удаления факультета\n");

            university.PrintInfo();
            Console.WriteLine();

            bool updateFaculty2Result = university.UpdFaculty(faculty2);
            Console.WriteLine(updateFaculty2Result ? "Факультет обновлён\n" : "Ошибка обновления факультета\n");

            university.PrintInfo();
        }
    }
}
