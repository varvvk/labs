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

namespace _6_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            double num1 = double.Parse(Input1.Text);
            int num2 = int.Parse(Input2.Text);
            double result = 0;

            string? operation = (ComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch(operation)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    result = Math.Round((num1 / num2),2);
                    break;
                default:
                    break;
            }

            Result.Text = result.ToString();
        }
    }
}