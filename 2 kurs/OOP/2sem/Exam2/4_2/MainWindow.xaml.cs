using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool updating;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Celsius_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (updating) return;
            updating = true;

            Fahrenheit.Text = int.TryParse(Celsius.Text, out int c) ? (c * 2 + 32).ToString() : "";

            updating = false;
        }

        private void Fahrenheit_TextChanged(object sender, TextChangedEventArgs e)
        {
            updating = true;

            Celsius.Text = int.TryParse(Fahrenheit.Text, out int c) ? ((c - 32)/2).ToString() : "";

            updating = false;
        }
    }
}