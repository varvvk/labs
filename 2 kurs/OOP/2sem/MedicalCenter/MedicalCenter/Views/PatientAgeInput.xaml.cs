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
    /// Логика взаимодействия для PatientAgeInput.xaml
    /// </summary>
    public partial class PatientAgeInput : UserControl
    {
        public PatientAgeInput()
        {
            InitializeComponent();
        }

        // DependencyProperty с Validate и Coerce
        public static readonly DependencyProperty AgeProperty = DependencyProperty.Register(
            "Age",
            typeof(int),
            typeof(PatientAgeInput),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceAge),
            ValidateAge);

        public int Age
        {
            get => (int)GetValue(AgeProperty);
            set => SetValue(AgeProperty, value);
        }

        private static bool ValidateAge(object value)
        {
            int val = (int)value;
            if (val < 0 || val > 150)
            {
                MessageBox.Show($"Валидация не пройдена: значение {val} вне диапазона 0-150.", "Валидация");
                return false;
            }
            return true;
        }

        private static object CoerceAge(DependencyObject d, object baseValue)
        {
            int val = (int)baseValue;
            if (val < 0)
            {
                MessageBox.Show($"Коррекция: значение {val} исправлено на 0.", "Коррекция");
                return 0;
            }
            if (val > 120)
            {
                MessageBox.Show($"Коррекция: значение {val} исправлено на 120.", "Коррекция");
                return 120;
            }
            return val;
        }


        // Routed Events
        public static readonly RoutedEvent AgeSubmittedEvent = EventManager.RegisterRoutedEvent(
            "AgeSubmitted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PatientAgeInput));
        public event RoutedEventHandler AgeSubmitted
        {
            add => AddHandler(AgeSubmittedEvent, value);
            remove => RemoveHandler(AgeSubmittedEvent, value);
        }

        public static readonly RoutedEvent PreviewAgeSubmittedEvent = EventManager.RegisterRoutedEvent(
            "PreviewAgeSubmitted", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(PatientAgeInput));
        public event RoutedEventHandler PreviewAgeSubmitted
        {
            add => AddHandler(PreviewAgeSubmittedEvent, value);
            remove => RemoveHandler(PreviewAgeSubmittedEvent, value);
        }

        public static readonly RoutedEvent AgeCheckedEvent = EventManager.RegisterRoutedEvent(
            "AgeChecked", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(PatientAgeInput));
        public event RoutedEventHandler AgeChecked
        {
            add => AddHandler(AgeCheckedEvent, value);
            remove => RemoveHandler(AgeCheckedEvent, value);
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            // Запускаем события (как у тебя было)
            RaiseEvent(new RoutedEventArgs(PreviewAgeSubmittedEvent)); // Tunneling
            RaiseEvent(new RoutedEventArgs(AgeCheckedEvent)); // Direct
            RaiseEvent(new RoutedEventArgs(AgeSubmittedEvent)); // Bubbling

            // Показываем итоговое значение возраста на экране
            OutputTextBlock.Text = $"Возраст: {Age}";
        }

    }
}
