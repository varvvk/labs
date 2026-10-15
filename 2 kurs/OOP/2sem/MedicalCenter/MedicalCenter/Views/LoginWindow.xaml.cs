using MedicalCenter.ViewModels;
using System.Windows;

namespace MedicalCenter.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var clientRepo = new ClientRepository(new Context());
            var adminRepo = new AdminRepository(new Context());

            var viewModel = new LoginViewModel(clientRepo, adminRepo);

            DataContext = viewModel;

            PasswordBox.PasswordChanged += (s, e) =>
            {
                viewModel.GetPassword = () => PasswordBox.Password;
            };
        }
    }
}
