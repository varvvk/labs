using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LR2
{
    public partial class Form1 : Form
    {
        private List<Computer> computers = new List<Computer>();
        private Computer currentComputer = new Computer();

        private string lastAction = "Ожидает действия";

        public Form1()
        {
            InitializeComponent();
            UpdateStatusStrip();
        }

        private void CPUinfoButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                currentComputer.Cpu = form2.processor;
                MessageBox.Show("Данные процессора обновлены.");
            }

            lastAction = "Добавлена информация о процессоре";
            UpdateStatusStrip();
        }

        private void videoCardButton1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            if (form3.ShowDialog() == DialogResult.OK)
            {
                currentComputer.Gpu = form3.videoCard;
                MessageBox.Show("Данные видеокарты обновлены.");
            }

            lastAction = " Добавлена информация о видеокарте";
            UpdateStatusStrip();
        }

        #region
        //private bool ValidateForm()
        //{
        //    if (string.IsNullOrEmpty(typeComboBox1.SelectedItem?.ToString()))
        //    {
        //        MessageBox.Show("Пожалуйста, выберите тип компьютера.");
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(ramTypeTextBox1.Text))
        //    {
        //        MessageBox.Show("Пожалуйста, укажите тип памяти.");
        //        return false;
        //    }

        //    if (ramSizeListBox1.SelectedItem == null)
        //    {
        //        MessageBox.Show("Пожалуйста, выберите размер оперативной памяти.");
        //        return false;
        //    }

        //    if (string.IsNullOrEmpty(diskTypeComboBox.SelectedItem?.ToString()))
        //    {
        //        MessageBox.Show("Пожалуйста, выберите тип диска.");
        //        return false;
        //    }

        //    if (diskSizeNumeric.Value == 0)
        //    {
        //        MessageBox.Show("Пожалуйста, укажите размер диска.");
        //        return false;
        //    }

        //    if (currentComputer.Cpu == null)
        //    {
        //        MessageBox.Show("Пожалуйста, выберите процессор.");
        //        return false;
        //    }

        //    if (currentComputer.Gpu == null)
        //    {
        //        MessageBox.Show("Пожалуйста, выберите видеокарту.");
        //        return false;
        //    }

        //    return true;
        //}
        #endregion

        public void CollectDataFromForm()
        {
            currentComputer.Type = typeComboBox1.SelectedItem?.ToString();
            currentComputer.RamType = ramTypeTextBox1.Text;
            currentComputer.RamSize = Convert.ToInt32(ramSizeListBox1.SelectedItem?.ToString());
            currentComputer.DiskType = diskTypeComboBox.SelectedItem?.ToString();
            currentComputer.DiskSize = (int)diskSizeNumeric.Value;
            currentComputer.GetDate = dateTimePicker1.Value;
        }

        private void priceButton1_Click(object sender, EventArgs e)
        {
            CollectDataFromForm();

            if (!ValidateComputer(currentComputer))
                return;

            decimal basePrice = 500;
            basePrice += currentComputer.RamSize * 10;
            basePrice += currentComputer.DiskSize * 5;
            basePrice += currentComputer.Cpu.Cores * 50;
            basePrice += currentComputer.Gpu.MemorySize * 20;
            currentComputer.Price = basePrice;

            priceOutput.Text = $"Стоимость: {currentComputer.Price}$";

            lastAction = "Рассчитана стоимость";
            UpdateStatusStrip();
        }

        private void saveButton1_Click(object sender, EventArgs e)
        {
            CollectDataFromForm();

            if (!ValidateComputer(currentComputer))
                return;

            computers.Add(currentComputer);

            string jsonData = JsonConvert.SerializeObject(computers, Formatting.Indented);
            string filePath = "computerData.json";
            File.WriteAllText(filePath, jsonData);
            MessageBox.Show("Данные успешно сохранены в файл.");

            currentComputer = new Computer();

            lastAction = "Данные сохранены в файл";
            UpdateStatusStrip();
        }

        private void LoadDataFromJson(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    computers = JsonConvert.DeserializeObject<List<Computer>>(jsonData) ?? new List<Computer>();
                }
                else
                {
                    computers = new List<Computer>();
                }
                MessageBox.Show($"Загружено {computers.Count} компьютеров!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
                computers = new List<Computer>();
            }
        }

        private void showButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files|*.json";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                LoadDataFromJson(filePath);

                Form4 form4 = new Form4(computers);
                form4.Show();
            }

            lastAction = "Данные выгружены из файла";
            UpdateStatusStrip();
        }


        //lab4
        private void searchItem_Click(object sender, EventArgs e)
        {
            SearchResultsForm searchForm = new SearchResultsForm(computers); 
            searchForm.Show();

            lastAction = "Выполнен поиск";
            UpdateStatusStrip();
        }

        private void sortItem_Click(object sender, EventArgs e)
        {
            SortForm sortForm = new SortForm(computers);
            sortForm.Show(); 

            lastAction = "Выполнена сортировка";
            UpdateStatusStrip();
        }

        private void aboutItem_Click(object sender, EventArgs e)
        {
            string version = "1.0.0";
            string developer = "Казакова В. В."; 

            MessageBox.Show($"Версия: {version}\nРазработчик: {developer}",
                            "О программе",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private bool ValidateComputer(Computer computer)
        {
            var context = new ValidationContext(computer, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(computer, context, results, true))
            {
                string errorMessages = string.Join("\n", results.Select(r => r.ErrorMessage));
                MessageBox.Show($"Ошибка валидации:\n{errorMessages}");
                return false;
            }

            return true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SearchResultsForm searchForm = new SearchResultsForm(computers);
            searchForm.Show();

            lastAction = "Выполнен поиск";
            UpdateStatusStrip();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SortForm sortForm = new SortForm(computers);
            sortForm.Show();

            lastAction = "Выполнена сортировка";
            UpdateStatusStrip();
        }

        private void toolStripClear_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox && control != textBox1 && control != textBox2)
                {
                    (control as TextBox).Clear();
                }
                else if (control is ComboBox)
                {
                    (control as ComboBox).SelectedIndex = -1;
                }
                else if (control is RichTextBox)
                {
                    (control as RichTextBox).Clear();
                }
                else if (control is NumericUpDown)
                {
                    if (control.Name == "diskSizeNumeric")
                    {
                        (control as NumericUpDown).Value = 256;
                    }
                }
                else if (control is DateTimePicker)
                {
                    if (control.Name == "dateTimePicker1")
                    {
                        (control as DateTimePicker).Value = DateTime.Now;
                    }
                }
                else if (control is ListBox && control.Name == "ramSizeListBox1")
                {
                    (control as ListBox).ClearSelected();
                }
            }

            lastAction = "Выполнена очистка формы";
            UpdateStatusStrip();
        }

        private void toolStripDeleteLast_Click(object sender, EventArgs e)
        {
            string filePath = "computerData.json";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                List<Computer> computers = JsonConvert.DeserializeObject<List<Computer>>(json);

                if (computers != null && computers.Any())
                {
                    computers.RemoveAt(computers.Count - 1); 

                    string updatedJson = JsonConvert.SerializeObject(computers, Formatting.Indented);

                    File.WriteAllText(filePath, updatedJson);

                    MessageBox.Show("Последний компьютер был удален.");
                }
                else
                {
                    MessageBox.Show("Список компьютеров пуст.");
                }
            }
            else
            {
                MessageBox.Show("Файл не найден.");
            }

            lastAction = "Удален последний объект";
            UpdateStatusStrip();
        }

        private void UpdateStatusStrip()
        {
            // Обновляем количество объектов в списке
            toolStripStatusLabelCount.Text = $"Объекты: {computers.Count}";

            // Обновляем информацию о последнем действии
            toolStripStatusLabelAction.Text = $"Последнее действие: {lastAction}";

            // Обновляем текущую дату и время
            toolStripStatusLDate.Text = $"Дата и время: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        private void toolStripBack_Click(object sender, EventArgs e)
        {
            SearchResultsForm searchForm = new SearchResultsForm(computers);
            searchForm.Show();

            lastAction = "Выполнен поиск";
            UpdateStatusStrip();
        }

        private void toolStripForward_Click(object sender, EventArgs e)
        {
            SortForm sortForm = new SortForm(computers);
            sortForm.Show();

            lastAction = "Выполнена сортировка";
            UpdateStatusStrip();
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
        }
    }
}
