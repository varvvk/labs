using MedicalCenter.Models;
using MedicalCenter.Repositories;
using MedicalCenter.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MedicalCenter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object _loggedInUser;
        public object CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            IServiceRepository repository = new ServiceRepository(); // Make sure you have implemented this
            DataContext = new MainViewModel(repository);

            ApplyTheme("DefaultTheme.xaml");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                _loggedInUser = loginWindow.DataContext is LoginViewModel loginVm ? loginVm.LoggedInUser : null;
                CurrentUser = _loggedInUser;

                if (DataContext is MainViewModel mainVm)
                {
                    mainVm.InitializeUser(CurrentUser); // ← ВЫЗЫВАЕМ НА MainViewModel
                }
            }
        }



        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser is Client client)
            {
                var viewModel = new UserProfileViewModel(client);
                var profileControl = new UserProfileControl { DataContext = viewModel };

                viewModel.CloseAction = () =>
                {
                    MainGrid.Children.Remove(profileControl);
                };

                Grid.SetRow(profileControl, 2);
                Grid.SetColumnSpan(profileControl, 2);

                MainGrid.Children.Add(profileControl);
            }
            else
            {
                MessageBox.Show("Вы не вошли как клиент.");
            }
        }

        private void ServiceCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is ServiceViewModel serviceVm)
            {
                if (DataContext is MainViewModel mainVm && mainVm.ShowDetailsCommand.CanExecute(serviceVm))
                {
                    mainVm.ShowDetailsCommand.Execute(serviceVm);
                }
            }
        }



        private void SetLanguage(string lang)
        {
            try
            {
                ResourceDictionary dict = new ResourceDictionary();

                switch (lang)
                {
                    case "en-US":
                        dict.Source = new Uri("/Resources/English.xaml", UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("/Resources/Russian.xaml", UriKind.Relative);
                        break;
                }

                // Удалить предыдущий словарь
                var oldDict = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("StringResources"));

                if (oldDict != null)
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);

                // Добавить новый
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при смене языка: " + ex.Message);
            }
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage("en-US");
        }

        private void RussianButton_Click(object sender, RoutedEventArgs e)
        {
            SetLanguage("ru-RU");
        }


        private bool _isPinkTheme = false;
        private void ChangeThemeButton_Click(object sender, RoutedEventArgs e)
        {
            _isPinkTheme = !_isPinkTheme;

            string theme = _isPinkTheme ? "PinkTheme.xaml" : "DefaultTheme.xaml";
            ApplyTheme(theme);
        }

        private void ApplyTheme(string themeName)
        {
            try
            {
                var dict = new ResourceDictionary
                {
                    Source = new Uri($"/Themes/{themeName}", UriKind.Relative)
                };

                // Удаляем старые словари темы
                var oldDict = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme"));

                if (oldDict != null)
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);

                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при смене темы: {ex.Message}");
            }
        }


        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.UndoCommand.Execute(null);
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
                vm.RedoCommand.Execute(null);
        }


        private void PatientAgeInput_PreviewAgeSubmitted(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tunneling: Перед отправкой возраста (PreviewAgeSubmitted)");
        }

        private void PatientAgeInput_AgeChecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Direct: Возраст проверен (AgeChecked)");
        }

        private void PatientAgeInput_AgeSubmitted(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bubbling: Возраст отправлен (AgeSubmitted)");
        }

    }
}