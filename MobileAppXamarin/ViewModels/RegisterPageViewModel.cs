using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileAppXamarin.Models;
using Xamarin.Forms;

namespace MobileAppXamarin.ViewModels
{
    internal class RegisterPageViewModel : INotifyPropertyChanged
    {
        public static readonly string PORT_CODE = "5262";

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                RegisterCommand.ChangeCanExecute();
            }
        }

        private bool _isEnable;
        public bool IsEnable {
            get => _isEnable;
            set
            {
                _isEnable = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnable)));
                RegisterCommand.ChangeCanExecute();
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                RegisterCommand.ChangeCanExecute();
            }
        }
        private string _surname;

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                RegisterCommand.ChangeCanExecute();
            }
        }
        
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RegisterCommand.ChangeCanExecute();
            }
        }
        
        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                RegisterCommand.ChangeCanExecute();
            }
        }
        
        private string _departmentCode;
        public string DepartmentCode
        {
            get => _departmentCode;
            set
            {
                _departmentCode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DepartmentCode)));
                RegisterCommand.ChangeCanExecute();
            }
        }

        public ICommand ImAnEmployeeCommand { get; }
        public Command RegisterCommand { get; }

        public RegisterPageViewModel()
        {
            ImAnEmployeeCommand = new Command<bool>(ImAnEmployeeExecute);
            RegisterCommand = new Command(CreateAnEmployeeExecute, Validation);
            DepartmentCode = "NOT AVAILABLE";

            Name = "Marcin";
            Surname = "Knapik";
            MiddleName = "Szymon";
            Email = "knapik-m@o2.pl";
            PhoneNumber = "42134124";
            DepartmentCode = "3";
        }

        private void ImAnEmployeeExecute(bool switchValue)
        {
            if (switchValue)
            {
                DepartmentCode = "";
                IsEnable = true;
            }
            else
            {
                DepartmentCode = "NOT AVAILABLE";
                IsEnable = false;
            }
        }

        private async void CreateAnEmployeeExecute()
        {
            if (!IsValidEmail(Email))
            {
                 await App.Current.MainPage.DisplayAlert("Błąd pola email",
                    "Wpisz proszę poprawny email, jeśli wpisałeś poprawny a dalej widzisz błąd, skonstaktuj się z obsługą klienta", "Ok");
            }

            Employee employee;
            try
            {
                if (IsEnable)
                {
                    employee = new Employee(Name, MiddleName, Surname, Email, PhoneNumber, int.Parse(DepartmentCode));
                    await SaveEmployeeToApi(employee);
                    await App.Current.MainPage.DisplayAlert("Powodzenie", "Udało się przesłać twój formularz!", "OK");
                }
                else
                {
                    employee = new Employee(Name, MiddleName, Surname, Email, PhoneNumber);
                    await SavePersonToApi(employee);
                    await App.Current.MainPage.DisplayAlert("Powodzenie", "Udało się przesłać twój formularz!", "OK");
                }
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Powodzenie", ex.Message, "OK");
            }
        }

        private async Task SaveEmployeeToApi(Employee employee)
        {
            HttpClient httpClient = new HttpClient();

            var requestContent = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"http://10.0.2.2:{PORT_CODE}/Person/SaveEmployee", requestContent);
            await App.Current.MainPage.DisplayAlert("Kod odpowiedzi serwera", response.ToString(), "ok");
            response.EnsureSuccessStatusCode();
            
        }

        private async Task SavePersonToApi(Employee employee)
        {
            HttpClient httpClient = new HttpClient();

            var requestContent = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"http://10.0.2.2:{PORT_CODE}/Person/SavePerson", requestContent);
            await App.Current.MainPage.DisplayAlert("Kod odpowiedzi serwera", response.ToString(), "ok");
            response.EnsureSuccessStatusCode();

        }
        private bool Validation()
        {
            if (IsEnable) // Selected employee option.
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(MiddleName) || string.IsNullOrWhiteSpace(Surname) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(DepartmentCode)
                   )
                {
                    return false;
                }
            }
            else // Not an employee
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(MiddleName) || string.IsNullOrWhiteSpace(Surname) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    return false;
                }
            }
            if (Name.Length > 100 || Name.Length == 0)
                return false;
            if (MiddleName.Length > 100 || MiddleName.Length == 0)
                return false;
            if (Surname.Length > 100 || Surname.Length == 0)
                return false;
            if (Email.Length > 100 || Email.Length == 0)
                return false;
            if (PhoneNumber.Length > 15 || PhoneNumber.Length == 0)
                return false;
            if (IsEnable) //For selected employee option
            {
                if (DepartmentCode.Length > 5 || DepartmentCode.Length == 0)
                    return false;
            }

            return true; //Button enable
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
