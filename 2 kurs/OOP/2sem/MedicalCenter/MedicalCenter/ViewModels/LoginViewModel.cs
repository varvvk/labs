using System;
using System.Windows;
using System.Windows.Input;
using MedicalCenter.Models;
using MedicalCenter.Repositories;

namespace MedicalCenter.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAdminRepository _adminRepository;

        private string _selectedUserType = "Client";
        private string _login;

        public string SelectedUserType
        {
            get => _selectedUserType;
            set
            {
                if (SetProperty(ref _selectedUserType, value))
                {
                    OnPropertyChanged(nameof(UserNameLabel));
                }
            }
        }

        public string UserNameLabel => SelectedUserType == "Admin" ? "Логин" : "Номер телефона";

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        /// <summary>
        /// Делегат для получения пароля из PasswordBox
        /// </summary>
        public Func<string> GetPassword { get; set; } = () => string.Empty;

        public ICommand LoginCommand { get; }

        public object LoggedInUser { get; private set; }

        public LoginViewModel(IClientRepository clientRepository, IAdminRepository adminRepository)
        {
            _clientRepository = clientRepository;
            _adminRepository = adminRepository;

            LoginCommand = new RelayCommand(_ => ExecuteLogin());
        }

        private void ExecuteLogin()
        {
            string password = GetPassword();


            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedUserType == "Admin")
            {
                var admin = _adminRepository.GetByLogin(Login, password);
                if (admin != null)
                {
                    LoggedInUser = admin;
                    MessageBox.Show($"Вход выполнен как администратор: {admin.Name}");
                    CloseWindow();
                    return;
                }
            }
            else if (SelectedUserType == "Client")
            {
                var client = _clientRepository.GetByLogin(Login, password);
                if (client != null)
                {
                    LoggedInUser = client;
                    MessageBox.Show($"Вход выполнен как клиент: {client.Name}");
                    CloseWindow();
                    return;
                }
            }

            MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = true;  // Это даст true в ShowDialog()
                    window.Close();
                    break;
                }
            }
        }
    }
}
