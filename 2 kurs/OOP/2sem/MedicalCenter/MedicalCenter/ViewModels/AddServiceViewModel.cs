using System.Windows.Input;
using MedicalCenter.Models;
using MedicalCenter.Repositories;
using System.Windows;

namespace MedicalCenter.ViewModels
{
    public class AddServiceViewModel : ViewModelBase
    {
        private readonly IServiceRepository _repository = new ServiceRepository(); // желательно через DI

        private string _title;
        private string _doctor;
        private string _category;
        private string _description;
        private decimal _price;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Doctor { get => _doctor; set => SetProperty(ref _doctor, value); }
        public string Category { get => _category; set => SetProperty(ref _category, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public decimal Price { get => _price; set => SetProperty(ref _price, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand ClearFieldsCommand { get; }


        private readonly MainViewModel _mainViewModel;
        public AddServiceViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());

            ClearFieldsCommand = new RelayCommand(_ => ClearFields());
        }


        public Service CreatedService { get; private set; }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Category))
            {
                MessageBox.Show("Пожалуйста, выберите категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var service = new Service
            {
                Title = Title,
                Doctor = Doctor,
                Category = Category,
                Description = Description,
                Price = Price
            };

            _mainViewModel.AddService(service);
            CreatedService = service;

            CloseWindow();
        }



        private void Cancel()
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void ClearFields()
        {
            Title = string.Empty;
            Doctor = string.Empty;
            Category = null;
            Description = string.Empty;
            Price = 0;
        }

    }
}
