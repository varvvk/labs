using System;
using System.Collections.Generic;

namespace LR_2
{
    public class University : Organization, IStaff
    {
        protected List<Faculty> faculties = new List<Faculty>();

        public University() { }
        public University(University other) 
        {
            Name = other.Name;
            ShortName = other.ShortName;
            Address = other.Address;
            TimeStamp = other.TimeStamp;
        }
        public University(string name, string shortName, string address)
        {
            Name = name;
            ShortName = shortName;
            Address = address;
            TimeStamp = DateTime.Now;
        }

        public int AddFaculty(Faculty faculty)
        {
            faculties.Add(faculty);
            return faculties.Count;
        }

        public bool DelFaculty(int index)
        {
            if (index >= 0 && index < faculties.Count)
            {
                faculties.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool UpdFaculty(Faculty faculty)
        {
            for (int i = 0; i < faculties.Count; i++)
            {
                if (faculties[i].Id == faculty.Id)
                {
                    faculties[i].SetName("Новое название"); // Устанавливаем новое название через метод SetName
                    faculties[i] = faculty;
                    return true;
                }
            }
            return false;
        }

        private bool VerFaculty(int index)
        {
            return index >= 0 && index < faculties.Count;
        }

        public List<Faculty> GetFaculties()
        {
            return faculties;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"University: {Name} ({ShortName}), Address: {Address}, Created on: {TimeStamp}");

            if (faculties.Count > 0)
            {
                Console.WriteLine("Список факультетов:");
                foreach (var faculty in faculties)
                {
                    Console.WriteLine($"- {faculty.Name}");
                }
            }
            else
            {
                Console.WriteLine("Факультетов пока нет.");
            }
        }

    }
}

