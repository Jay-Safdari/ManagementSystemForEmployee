using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using EmployeeManagmentWPF.Models;



namespace EmployeeManagmentWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient;
        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7207/api/")
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void btn_ShowPass_Click(object sender, RoutedEventArgs e)
        {
            string passWord = pswBox_password.Password;
            txtBox_showPassword.Text = passWord;
        }

        private async void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            txtBox_showPassword.Clear();
            var userCredentials = new UserCredentials
            {
                Username = txtBox_username.Text,
                Password = pswBox_password.Password
            };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("users/authenticate", userCredentials);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

                if (authResponse.IsAuthenticated)
                {
                    if (authResponse.Role == "Admin")
                    {
                        Admin adminWindow = new Admin();
                        adminWindow.Show();
                    }
                    else if (authResponse.Role == "Employee")
                    {
                        Employee employeeWindow = new Employee();
                        employeeWindow.Show();
                    }
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Error connecting to the server.");
            }

        }

        public class UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
