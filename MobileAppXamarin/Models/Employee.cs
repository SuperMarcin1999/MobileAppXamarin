using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppXamarin.Models
{
    internal class Employee
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }

        public Employee(string name, string password, string surname, string email, string phoneNumber, int? departmentId = null)
        {
            Name = name;
            Password = password;
            SurName = surname;
            Email = email;
            PhoneNumber = phoneNumber;
            DepartmentId = departmentId;
        }
    }
}
