using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeManagmentWPF
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        private readonly HttpClient httpClient;
        public Employee()
        {
            InitializeComponent();

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7207/api/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        private async void btn_searchEmployeeById_Click_1(object sender, RoutedEventArgs e)
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
    }
}
