using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileAppXamarin.Models;
using Xamarin.Forms;

namespace MobileAppXamarin.ViewModels
{
    internal class LoginPageViewModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                LoginCommand.ChangeCanExecute();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                LoginCommand.ChangeCanExecute();
            }
        }
        public bool IsEmployee { get; set; }

        public Command LoginCommand { get; }
        public ICommand ImAnEmployeeCommand { get; }


        public LoginPageViewModel()
        {
            LoginCommand = new Command(LoginCommandExecute, Validation);
            ImAnEmployeeCommand = new Command<bool>(ImAnEmployeeExecute);
        }

        private void ImAnEmployeeExecute(bool switchValue)
        {
            IsEmployee = switchValue;
        }

        private void LoginCommandExecute()
        {
            EmployeeLogin employeeLogin = new EmployeeLogin{Name = this.Name, Password = this.Password};
            SendLoginRequestToApi(employeeLogin);
        }

        private async Task SendLoginRequestToApi(EmployeeLogin human)
        {
            HttpClient httpClient = new HttpClient();

            var requestContent =
                new StringContent(JsonSerializer.Serialize(human), Encoding.UTF8, "application/json");

            if (IsEmployee)
            {
                var response =
                    await httpClient.PostAsync($"http://10.0.2.2:{RegisterPageViewModel.PORT_CODE}/Person/LoginEmployee", requestContent);
                await App.Current.MainPage.DisplayAlert("Kod odpowiedzi serwera", response.ToString(), "ok");
                response.EnsureSuccessStatusCode();
            }
            else
            {
                var response =
                    await httpClient.PostAsync($"http://10.0.2.2:{RegisterPageViewModel.PORT_CODE}/Person/LoginPerson", requestContent);
                await App.Current.MainPage.DisplayAlert("Kod odpowiedzi serwera", response.ToString(), "ok");
                response.EnsureSuccessStatusCode();
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            return true;
        }
    }
}
