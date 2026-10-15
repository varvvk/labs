using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace LR_2
{
    public class Faculty : Organization, IStaff
    {
        protected List<Department> departments = new List<Department>();


        public Faculty() {  }

        public Faculty(Faculty other) 
        {
            Name = other.Name;
            ShortName = other.ShortName;
            Address = other.Address;
            TimeStamp = other.TimeStamp;
        }

        public Faculty(string name, string shortName, string address) 
        {
            Name = name;
            ShortName = shortName;
            Address = address;
            TimeStamp = DateTime.Now;
        }

        public int AddDepartment(Department department)
        {
            departments.Add(department);
            return departments.Count;
        }
        public bool DelDepartment(int index)
        {
            if (index >= 0 && index < departments.Count)
            {
                departments.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool UpdDepartment(Department department)
        {
            for (int i = 0; i < departments.Count; i++)
            {
                if (departments[i].Id == department.Id)
                {
                    departments[i] = department;
                    return true;
                }
            }
            return false;
        }

        private bool VerDepartment(int index)
        {
            return index >= 0 && index < departments.Count;
        }

        public List<Department> GetDepartments() => departments;

        public void PrintInfo()
        {
            Console.WriteLine($"Faculty: {Name} ({ShortName}), Address: {Address}, Created on: {TimeStamp} \n");
        }

        public void SetName(string newName)
        {
            Name = newName;
        }

    }
}
