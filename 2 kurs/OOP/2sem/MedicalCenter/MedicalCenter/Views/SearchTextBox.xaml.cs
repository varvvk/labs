using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalCenter.Controls
{
    /// <summary>
    /// Логика взаимодействия для SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        public SearchTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(SearchTextBox),
        new FrameworkPropertyMetadata(
            string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            null,
            CoerceText),
        ValidateText);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static bool ValidateText(object value)
        {
            var text = value as string ?? string.Empty;

            if (text.Length > 50)
            {
                MessageBox.Show("Превышен максимальный лимит в 50 символов.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private static object CoerceText(DependencyObject d, object baseValue)
        {
            var text = baseValue as string ?? string.Empty;
            return text.Length > 50 ? text.Substring(0, 50) : text;
        }
    }
}
