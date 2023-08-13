namespace EmployeeManagment.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Employee Employee { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
