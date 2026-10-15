using MedicalCenter.Models;
using System.Windows;
using System.Windows.Input;

namespace MedicalCenter.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly Client _originalClient;
        private readonly Context _context;

        public ICommand CloseCommand { get; }
        public Action? CloseAction { get; set; } // Делегат вызывается из View


        private string _name;
        private string _email;
        private string _phone;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public ICommand SaveCommand { get; }

        public UserProfileViewModel(Client client)
        {
            _originalClient = client;
            _context = new Context(); // Или через DI, если используете

            Name = client.Name;
            Email = client.Email;
            Phone = client.Phone;

            SaveCommand = new RelayCommand(_ => SaveChanges());
            CloseCommand = new RelayCommand(_ =>
            {
                CloseAction?.Invoke();
            });

        }

        private void SaveChanges()
        {
            var client = _context.Clients.Find(_originalClient.Id);
            if (client != null)
            {
                client.Name = Name;
                client.Email = Email;
                client.Phone = Phone;

                _context.SaveChanges();

                MessageBox.Show("Данные обновлены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
