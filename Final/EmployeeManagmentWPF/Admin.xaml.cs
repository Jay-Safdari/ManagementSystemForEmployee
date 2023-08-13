using Newtonsoft.Json;
using EmployeeManagmentWPF.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace EmployeeManagmentWPF
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private readonly HttpClient httpClient;

        public Admin()
        {
            InitializeComponent();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7207/api/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            LoadEmployees();
            LoadUsers();
        }

        private async void LoadEmployees()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("employees/getEmployees");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<Response>(jsonResponse);

                    if (apiResponse?.Employees != null && apiResponse.Employees.Count > 0)
                    {
                        dataGrid_tbl.ItemsSource = apiResponse.Employees;
                    }
                    else
                    {
                        MessageBox.Show("No data found or an error occurred while fetching the data.");
                    }
                }
                else
                {
                    MessageBox.Show($"Failed to fetch employees data. Server responded with {response.StatusCode}.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ClearTextField()
        {
            txtBox_fname.Text = string.Empty;
            txtBox_lname.Text = string.Empty;
            txtBox_age.Text = string.Empty;
            txtBox_gender.Text = string.Empty;
            txtBox_position.Text = string.Empty;
            txtBox_department.Text = string.Empty;
            txtBox_salary.Text = string.Empty;
            txtBox_degree.Text = string.Empty;
            txtBox_fieldStudy.Text = string.Empty;
            txtBox_VacationUsed.Text = string.Empty;
            txtBox_VacEligPeriod.Text = string.Empty;
            datePicker_employmentDate.SelectedDate = null;
        }

        private void dataGrid_tbl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dataGrid_tbl.SelectedItem is EmployeeManagmentWPF.Models.Employee selectedEmployee)
            {
                txtBox_fname.Text = selectedEmployee.FirstName;
                txtBox_lname.Text = selectedEmployee.LastName;
                txtBox_age.Text = selectedEmployee.Age.ToString();
                txtBox_gender.Text = selectedEmployee.Gender.ToString();
                txtBox_position.Text = selectedEmployee.Position;
                txtBox_department.Text = selectedEmployee.DepartmentName;
                txtBox_salary.Text = selectedEmployee.Salary.ToString("N2"); // Assuming you want two decimal points
                txtBox_degree.Text = selectedEmployee.Degree;
                txtBox_fieldStudy.Text = selectedEmployee.FieldOfStudy;
                txtBox_VacationUsed.Text = selectedEmployee.VacationUsed.ToString();
                txtBox_VacEligPeriod.Text = selectedEmployee.VacationEligibilityPeriod.ToString();
                datePicker_employmentDate.SelectedDate = selectedEmployee.EmploymentDate;
            }
        }

        private async void btn_addEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagmentWPF.Models.Employee employee = new EmployeeManagmentWPF.Models.Employee
            {
                FirstName = txtBox_fname.Text,
                LastName = txtBox_lname.Text,
                Age = int.Parse(txtBox_age.Text),
                Position = txtBox_position.Text,
                DepartmentName = txtBox_department.Text,
                Salary = decimal.Parse(txtBox_salary.Text),
                Degree = txtBox_degree.Text,
                FieldOfStudy = txtBox_fieldStudy.Text,
                VacationUsed = int.Parse(txtBox_VacationUsed.Text),
                VacationEligibilityPeriod = int.Parse(txtBox_VacEligPeriod.Text),
                EmploymentDate = datePicker_employmentDate.SelectedDate ?? DateTime.Now
            };

            if (Enum.TryParse(txtBox_gender.Text, out Gender genderValue))
            {
                employee.Gender = genderValue;
            }
            else
            {
                MessageBox.Show("Invalid gender input. Please enter a valid gender.");
                return;
            }

            var response = await httpClient.PostAsJsonAsync("employees/addEmployee", employee);

            MessageBox.Show(response.StatusCode.ToString());

            LoadEmployees();
            ClearTextField();
        }

        private async void btn_UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            // First, we ensure that an employee is selected from the data grid.
            if (dataGrid_tbl.SelectedItem is not EmployeeManagmentWPF.Models.Employee selectedEmployee)
            {
                MessageBox.Show("Please select an employee to update.");
                return;
            }

            // Update selected employee's data from input fields
            selectedEmployee.FirstName = txtBox_fname.Text;
            selectedEmployee.LastName = txtBox_lname.Text;
            selectedEmployee.Age = int.Parse(txtBox_age.Text);
            selectedEmployee.Position = txtBox_position.Text;
            selectedEmployee.DepartmentName = txtBox_department.Text;
            selectedEmployee.Salary = decimal.Parse(txtBox_salary.Text);
            selectedEmployee.Degree = txtBox_degree.Text;
            selectedEmployee.FieldOfStudy = txtBox_fieldStudy.Text;
            selectedEmployee.VacationUsed = int.Parse(txtBox_VacationUsed.Text);
            selectedEmployee.VacationEligibilityPeriod = int.Parse(txtBox_VacEligPeriod.Text);
            selectedEmployee.EmploymentDate = datePicker_employmentDate.SelectedDate ?? DateTime.Now;

            if (Enum.TryParse(txtBox_gender.Text, out Gender genderValue))
            {
                selectedEmployee.Gender = genderValue;
            }
            else
            {
                MessageBox.Show("Invalid gender input. Please enter a valid gender.");
                return;
            }

            try
            {
                // Assuming your API takes an ID in the URL to specify which employee to update.
                var response = await httpClient.PutAsJsonAsync($"employees/updateEmployee/{selectedEmployee.Id}", selectedEmployee);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Employee updated successfully.");
                    LoadEmployees();
                    ClearTextField();
                }
                else
                {
                    MessageBox.Show($"Failed to update employee. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void btn_deleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            // First, ensure that an employee is selected from the data grid.
            if (dataGrid_tbl.SelectedItem is not EmployeeManagmentWPF.Models.Employee selectedEmployee)
            {
                MessageBox.Show("Please select an employee to delete.");
                return;
            }

            // Optionally, you can add a confirmation prompt before deletion.
            var confirmation = MessageBox.Show($"Are you sure you want to delete {selectedEmployee.FirstName} {selectedEmployee.LastName}?", "Confirm Delete", MessageBoxButton.YesNo);
            if (confirmation == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                // Assuming your API takes an ID in the URL to specify which employee to delete.
                var response = await httpClient.DeleteAsync($"employees/deleteEmployee/{selectedEmployee.Id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Employee deleted successfully.");
                    LoadEmployees();
                    ClearTextField();
                }
                else
                {
                    MessageBox.Show($"Failed to delete employee. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void btn_searchEmployeeById_Click(object sender, RoutedEventArgs e)
        {
            int employeeId;

            // Validate the ID input
            if (!int.TryParse(txtBox_searchID.Text, out employeeId))
            {
                MessageBox.Show("Please enter a valid employee ID.");
                return;
            }

            try
            {
                var response = await httpClient.GetAsync($"employees/getEmployee/{employeeId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<EmployeeManagmentWPF.Models.Response>(responseContent);

                    if (apiResponse != null && apiResponse.Employee != null)
                    {
                        dataGrid_tbl.ItemsSource = new List<EmployeeManagmentWPF.Models.Employee> { apiResponse.Employee };
                    }
                    else
                    {
                        MessageBox.Show("Employee not found.");
                        dataGrid_tbl.ItemsSource = null;
                    }
                }
                else
                {
                    //MessageBox.Show("Error fetching the employee.");
                    MessageBox.Show($"Error fetching the employee. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void LoadUsers()
        {
            try
            {
                var response = await httpClient.GetAsync("users/getUsers");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<EmployeeManagmentWPF.Models.User>>(jsonString);

                    dataGrid_user.ItemsSource = users;
                }
                else
                {
                    MessageBox.Show("Error fetching users.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBox_username.Text) || string.IsNullOrEmpty(txtBox_password.Text) || string.IsNullOrEmpty(txtBox_role.Text))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            var user = new EmployeeManagmentWPF.Models.User
            {
                Username = txtBox_username.Text,
                PasswordHash = txtBox_password.Text,
                Role = txtBox_role.Text
            };

            try
            {
                var response = await httpClient.PostAsJsonAsync("users/addUser", user);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User added successfully");
                    LoadUsers();  // Refresh the user list
                    ClearUserFields(); // Clear the input fields
                }
                else
                {
                    MessageBox.Show("Error adding user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ClearUserFields()
        {
            txtBox_username.Clear();
            txtBox_password.Clear();
            txtBox_role.Clear();
        }
    }
}

