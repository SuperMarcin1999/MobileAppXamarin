using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileAppXamarin.Views;
using Xamarin.Forms;

namespace MobileAppXamarin.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public ICommand RegisterCommand {get; } 
        public ICommand LoginCommand {get; }

        public MainPageViewModel()
        {
            RegisterCommand = new Command(RegisterExecute);
            LoginCommand = new Command(LoginExecute);
        }

        private void LoginExecute()
        {
            App.Current.MainPage = GetMainPage();
            App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private void RegisterExecute()
        {
            App.Current.MainPage = GetMainPage();
            App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static Page GetMainPage()
        {
            return new NavigationPage(new MainPage());
        }
    }
}
