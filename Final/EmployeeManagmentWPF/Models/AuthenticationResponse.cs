namespace EmployeeManagmentWPF.Models
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }
}
