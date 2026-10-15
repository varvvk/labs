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

namespace _7_2
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

        private void Background_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? style = (Background.SelectedItem as ComboBoxItem)?.Content.ToString();

            switch (style)
            {
                case "Gray":
                    TextBlock.Background = Brushes.Gray;
                    break;
                case "Pink":
                    TextBlock.Background = Brushes.Pink;
                    break;
                case "Light Blue":
                    TextBlock.Background = Brushes.LightBlue;
                    break;
                default:
                    break;
            }
        }

        private void Font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? fontName = (Font.SelectedItem as ComboBoxItem)?.Content.ToString();
            TextBlock.FontFamily = new FontFamily(fontName);
        }
    }
}