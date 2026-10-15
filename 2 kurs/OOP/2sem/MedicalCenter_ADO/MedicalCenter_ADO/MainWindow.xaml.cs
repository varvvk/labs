using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace MedicalCenter_ADO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;
        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadPatients();
            LoadDoctors();
            LoadAppointments();
        }

        // --- Patients ---

        private void LoadPatients()
        {
            PatientsDataGrid.ItemsSource = dbHelper.GetAll("Patients").DefaultView;
        }

        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new PatientWindow();
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    using var uow = new UnitOfWork(dbHelper.ConnectionString);
                    var success = uow.Patients.Insert(new Patient
                    {
                        FullName = wnd.FullName,
                        BirthDate = wnd.BirthDate
                    });
                    uow.Commit();
                    if (success) LoadPatients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении пациента: " + ex.Message);
                }
            }
        }

        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента для редактирования.");
                return;
            }

            DataRowView row = (DataRowView)PatientsDataGrid.SelectedItem;
            var wnd = new PatientWindow
            {
                PatientID = (int)row["PatientID"],
                FullName = (string)row["FullName"],
                BirthDate = (DateTime)row["BirthDate"]
            };

            if (wnd.ShowDialog() == true)
            {
                try
                {
                    using var uow = new UnitOfWork(dbHelper.ConnectionString);
                    var success = uow.Patients.Update(new Patient
                    {
                        PatientID = wnd.PatientID,
                        FullName = wnd.FullName,
                        BirthDate = wnd.BirthDate
                    });
                    uow.Commit();
                    if (success) LoadPatients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обновлении пациента: " + ex.Message);
                }
            }
        }

        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента для удаления.");
                return;
            }

            DataRowView row = (DataRowView)PatientsDataGrid.SelectedItem;
            int id = (int)row["PatientID"];
            if (MessageBox.Show("Удалить пациента?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using var uow = new UnitOfWork(dbHelper.ConnectionString);
                    var success = uow.Patients.Delete(id);
                    uow.Commit();
                    if (success) LoadPatients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении пациента: " + ex.Message);
                }
            }
        }


        private void SearchPatient_Click(object sender, RoutedEventArgs e)
        {
            string search = PatientSearchBox.Text.Trim();
            if (string.IsNullOrEmpty(search))
            {
                LoadPatients();
                return;
            }

            DataTable dt = dbHelper.ExecuteStoredProcedure("GetAppointmentsByPatientName", new SqlParameter[] {
                new SqlParameter("@Name", search)
            });


            AppointmentsDataGrid.ItemsSource = dt.DefaultView;
            MainTabControl.SelectedIndex = 2;
        }

        // --- Doctors ---

        private void LoadDoctors()
        {
            DoctorsDataGrid.ItemsSource = dbHelper.GetAll("Doctors").DefaultView;
        }


        // --- Appointments ---

        private void LoadAppointments()
        {
            string query = @"SELECT a.AppointmentID, p.FullName AS Patient, d.FullName AS Doctor, a.AppointmentDate, a.Notes 
                             FROM Appointments a
                             JOIN Patients p ON a.PatientID = p.PatientID
                             JOIN Doctors d ON a.DoctorID = d.DoctorID";
            using (var conn = new SqlConnection(dbHelper.ConnectionString))
            {
                var dt = new DataTable();
                var adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
                AppointmentsDataGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}