using EmployeeManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("addEmployee")]
        public Response AddEmployee(Employee employee)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.AddEmployee(con, employee);

            return response;
        }

        [HttpPut]
        [Route("updateEmployee/{id}")]
        public Response UpdateEmployee(int id, Employee employee)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.UpdateEmployee(con, employee);

            return response;
        }

        [HttpDelete]
        [Route("deleteEmployee/{id}")]
        public Response DeleteEmployee(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.DeleteEmployee(con, id);

            return response;
        }

        [HttpGet]
        [Route("getEmployee/{id}")]
        public Response GetEmployee(int id)
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.GetEmployee(con, id);

            return response;
        }

        
        [HttpGet]
        [Route("getEmployees")]
        public Response GetEmployees()
        {
            NpgsqlConnection con = new NpgsqlConnection(_configuration.GetConnectionString("employeeConnection").ToString());
            Response response = new Response();
            Applications app = new Applications();
            response = app.GetEmployees(con);

            return response;
        }
        
    }
}
