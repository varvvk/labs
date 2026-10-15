using MedicalCenter.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MedicalCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var context = new Context())
            {
                context.Database.EnsureDeleted();   // Удаляет БД
                context.Database.EnsureCreated();   // Создаёт заново
                DbInitializer.Initialize(context);  // Заполняет начальными данными
            }

            //var loginWindow = new LoginWindow();
            //loginWindow.Show();
        }

    }

}
