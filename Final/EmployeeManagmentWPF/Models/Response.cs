using System.Collections.Generic;
using System.Windows.Documents;

namespace EmployeeManagmentWPF.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Employee Employee { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
