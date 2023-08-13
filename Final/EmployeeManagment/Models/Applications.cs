using Npgsql;
using System.Text;
using System.Security.Cryptography;


namespace EmployeeManagment.Models
{
    public class Applications
    {
        // CRUD operations related to Employees

        public Response AddEmployee(NpgsqlConnection con, Employee employee)
        {
            Response response = new Response();
            try
            {
                string query = "INSERT INTO employees (firstName, lastName, age, gender,position, departmentname, salary, employmentdate, vacationUsed, vacationeligibilityperiod, degree, fieldofstudy) VALUES (@FirstName, @LastName, @Age, @Gender, @Position, @DepartmentName, @Salary, @EmploymentDate, @VacationUsed, @VacationEligibility, @Degree, @FieldOfStudy) RETURNING id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                // Add all necessary parameters
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender.ToString());
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@DepartmentName", employee.DepartmentName);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@EmploymentDate", employee.EmploymentDate);
                cmd.Parameters.AddWithValue("@VacationUsed", employee.VacationUsed);
                cmd.Parameters.AddWithValue("@VacationEligibility", employee.VacationEligibilityPeriod);
                cmd.Parameters.AddWithValue("@Degree", employee.Degree);
                cmd.Parameters.AddWithValue("@FieldOfStudy", employee.FieldOfStudy);

                con.Open();

                int id = (int)cmd.ExecuteScalar();
                response.StatusCode = 200;
                response.Message = "Employee added successfully";
                response.Employee = employee;
                response.Employee.Id = id; // Set the returned Id to the employee object
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while adding the employee: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public Response UpdateEmployee(NpgsqlConnection con, Employee employee)
        {
            Response response = new Response();
            try
            {
                string query = "UPDATE employees SET firstName=@FirstName, lastName=@LastName, age=@Age, gender=@Gender, position=@Position, departmentname=@DepartmentName, salary=@Salary, employmentdate=@EmploymentDate, VacationUsed=@VacationUsed, vacationeligibilityperiod=@VacationEligibility, Degree=@Degree, FieldOfStudy=@FieldOfStudy WHERE id=@Id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender.ToString());
                cmd.Parameters.AddWithValue("@Position", employee.Position);
                cmd.Parameters.AddWithValue("@DepartmentName", employee.DepartmentName);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@EmploymentDate", employee.EmploymentDate);
                cmd.Parameters.AddWithValue("@VacationUsed", employee.VacationUsed);
                cmd.Parameters.AddWithValue("@VacationEligibility", employee.VacationEligibilityPeriod);
                cmd.Parameters.AddWithValue("@Degree", employee.Degree);
                cmd.Parameters.AddWithValue("@FieldOfStudy", employee.FieldOfStudy);
                
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Employee updated successfully";
                }
                else
                {
                    response.StatusCode = 404; // Not Found
                    response.Message = "Employee not found";
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while updating the employee: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return response;
        }


        public Response DeleteEmployee(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            try
            {
                string query = "DELETE FROM employees WHERE Id=@Id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Employee deleted successfully";
                }
                else
                {
                    response.StatusCode = 404; // Not Found
                    response.Message = "Employee not found";
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while deleting the employee: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return response;
        }


        public Response GetEmployee(NpgsqlConnection con, int id)
        {
            Response response = new Response();
            try
            {
                string query = "SELECT * FROM employees WHERE Id=@Id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString(reader.GetOrdinal("Gender"))),
                        Position = reader.GetString(reader.GetOrdinal("Position")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                        EmploymentDate = reader.GetDateTime(reader.GetOrdinal("EmploymentDate")),
                        VacationUsed = reader.GetInt32(reader.GetOrdinal("VacationUsed")),
                        VacationEligibilityPeriod = reader.GetInt32(reader.GetOrdinal("VacationEligibilityperiod")),
                        Degree = reader.GetString(reader.GetOrdinal("Degree")),
                        FieldOfStudy = reader.GetString(reader.GetOrdinal("FieldOfStudy")),
                        
                    };


                

                    response.StatusCode = 200;
                    response.Message = "Employee retrieved successfully";
                    response.Employee = employee;
                }
                else
                {
                    response.StatusCode = 404; // Not Found
                    response.Message = "Employee not found";
                }
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500;
                response.Message = "An error occurred while retrieving the employee: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public Response GetEmployees(NpgsqlConnection con)
        {
            Response response = new Response();

            try
            {
                string query = "select * from employees";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                con.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                List<Employee> employees = new List<Employee>();    
                while (reader.Read())
                {
                    Employee employee = new Employee()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString(reader.GetOrdinal("Gender"))),
                        Position = reader.GetString(reader.GetOrdinal("Position")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                        EmploymentDate = reader.GetDateTime(reader.GetOrdinal("EmploymentDate")),
                        VacationUsed = reader.GetInt32(reader.GetOrdinal("VacationUsed")),
                        VacationEligibilityPeriod = reader.GetInt32(reader.GetOrdinal("VacationEligibilityperiod")),
                        Degree = reader.GetString(reader.GetOrdinal("Degree")),
                        FieldOfStudy = reader.GetString(reader.GetOrdinal("FieldOfStudy")),

                    };
                    employees.Add(employee);
                }
                response.StatusCode = 200;
                response.Message = "Employees retrieved successfully";
                response.Employees = employees;

            }
            catch (NpgsqlException ex)
            {

            }
            return response;
        }


        // Authentication related to User

        public List<User> GetUsers(NpgsqlConnection con)
        {
            List<User> users = new List<User>();

            try
            {
                string query = "SELECT Id, Username, Role FROM users"; 
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                con.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Role = reader.GetString(2)
                    });
                }
            }
            catch (NpgsqlException ex)
            {
                // Log the exception (consider using a logging library for better logging)
                Console.WriteLine("An error occurred while fetching users: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return users;
        }


        public Response AddUser(NpgsqlConnection con, User user)
        {
            Response response = new Response();

            try
            {
                string hashedPassword = HashPassword(user.PasswordHash); // Hashing password before storing
                string query = "INSERT INTO users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                con.Open();
                cmd.ExecuteNonQuery();

                response.StatusCode = 200;
                response.Message = "User added successfully";
            }
            catch (NpgsqlException ex)
            {
                response.StatusCode = 500; // Internal Server Error
                response.Message = "An error occurred while adding the user: " + ex.Message;
            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public User AuthenticateUser(NpgsqlConnection con, string username, string password)
        {
            try
            {
                // Fetch user details from the database where username matches
                string query = "SELECT Id, Username, PasswordHash, Role FROM users WHERE Username = @Username";
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader["PasswordHash"].ToString();

                    // If the password matches the stored hash
                    if (VerifyPassword(password, storedHash))
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = storedHash,
                            Role = reader["Role"].ToString()
                        };

                        return user;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                // Handle the exception, perhaps log it or rethrow.
            }
            finally
            {
                con.Close();
            }

            // If authentication fails or any other issue, return null.
            return null;
        }


        // Helper methods for password hashing and verification
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string providedPassword, string storedHash)
        {
            string hashedPassword = HashPassword(providedPassword);
            return hashedPassword.Equals(storedHash);
        }

    }
}
