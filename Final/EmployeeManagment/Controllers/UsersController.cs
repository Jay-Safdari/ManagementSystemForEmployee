using Microsoft.AspNetCore.Mvc;
using Npgsql;
using EmployeeManagment.Models;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Applications _applications;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _applications = new Applications();
        }

        // Method to Get All Users
        [HttpGet]
        [Route("getUsers")]
        public IEnumerable<User> GetUsers()
        {
            using var con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            return _applications.GetUsers(con);
        }

        // Method to Add User
        [HttpPost]
        [Route("addUser")]
        public Response AddUser(User user)
        {
            using var con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            return _applications.AddUser(con, user);
        }

        // Method for Authentication
        [HttpPost]
        [Route("authenticate")]
        public ActionResult<AuthenticationResponse> Authenticate(UserCredentials userCredentials)
        {
            using var con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            var user = _applications.AuthenticateUser(con, userCredentials.Username, userCredentials.Password);
            if (user != null)
            {
                return Ok(new AuthenticationResponse
                {
                    IsAuthenticated = true,
                    Message = "Authentication successful!",
                    Role = user.Role
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponse
                {
                    IsAuthenticated = false,
                    Message = "Invalid username or password.",
                    Role = null
                });
            }
        }
    }

    // Helper class to capture authentication parameters
    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

