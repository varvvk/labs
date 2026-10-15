using System.Windows;

namespace MedicalCenter_ADO
{
    /// <summary>
    /// Логика взаимодействия для PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        public int PatientID { get; set; }
        public string FullName { get => txtFullName.Text.Trim(); set => txtFullName.Text = value; }
        public DateTime BirthDate { get => dateBirth.SelectedDate ?? DateTime.MinValue; set => dateBirth.SelectedDate = value; }

        public PatientWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || !dateBirth.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
