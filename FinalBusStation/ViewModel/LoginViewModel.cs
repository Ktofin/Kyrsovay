using FinalBusStation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using FinalBusStation.View;

namespace FinalBusStation.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private readonly AuthModel _authModel;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            _authModel = new AuthModel();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            OpenRegisterCommand = new RelayCommand(ExecuteOpenRegister); // Инициализация команды
        }

        private void ExecuteLogin(object parameter)
        {
            var user = _authModel.Authenticate(Username, Password);
            if (user != null)
            {
                if (user.role == "Administrator")
                {
                    var adminWindow = new AdminView();
                    adminWindow.Show();
                }
                else
                {
                    UserSession.UserId = user.user_id;
                    PassengerView passengerView = new PassengerView();
                    passengerView.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteOpenRegister(object parameter)
        {
            // Открытие окна регистрации
            RegistrationView registerView = new RegistrationView();
            registerView.Show();
           
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
