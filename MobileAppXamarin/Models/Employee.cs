using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppXamarin.Models
{
    internal class Employee
    {
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }

        public Employee(string name, string middleName, string surname, string email, string phoneNumber, int? departmentId = null)
        {
            Name = name;
            MiddleName = middleName;
            SurName = surname;
            Email = email;
            PhoneNumber = phoneNumber;
            DepartmentId = departmentId;
        }
    }
}
