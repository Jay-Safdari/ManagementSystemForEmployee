using System;

namespace EmployeeManagmentWPF.Models
{
    public enum Gender
    {
        Male,
        Female,
        NonBinary,
        NotSpecified
    }

    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Position { get; set; }
        public string DepartmentName { get; set; }
        public decimal Salary { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int VacationUsed { get; set; }
        public int VacationEligibilityPeriod { get; set; } // This can be in days or months
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
    }
}
