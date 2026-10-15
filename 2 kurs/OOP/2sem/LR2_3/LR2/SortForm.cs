using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace LR2
{
    public partial class SortForm : Form
    {
        private List<Computer> computers;
        public SortForm(List<Computer> computers)
        {
            InitializeComponent();
            this.computers = computers;
            DisplayComputers(computers);
        }

        private void DisplayComputers(List<Computer> list)
        {
            richTextBox1.Clear();
            foreach (var comp in list)
            {
                richTextBox1.AppendText(
                    $"Тип: {comp.Type}\n" +
                    $"RAM: {comp.RamSize} {comp.RamType}\n" +
                    $"Диск: {comp.DiskSize} {comp.DiskType}\n" +
                    $"Дата: {comp.GetDate.ToShortDateString()}\n" +
                    $"CPU: {comp.Cpu.Manufacturer} {comp.Cpu.Model} ({comp.Cpu.Cores} ядер, {comp.Cpu.Frequency} ГГц)\n" +
                    $"GPU: {comp.Gpu.Manufacturer} {comp.Gpu.Model} ({comp.Gpu.MemorySize} ГБ)\n" +
                    $"Цена: {comp.Price:C}\n" +
                    "----------------------\n"
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sort1 = comboBoxSort1.SelectedItem?.ToString();
            var sort2 = comboBoxSort2.SelectedItem?.ToString();

            bool isDescending = radioButtonDescending.Checked;

            if (!string.IsNullOrEmpty(sort1) && !string.IsNullOrEmpty(sort2) && sort1 != sort2)
            {
                computers = SortByTwoCriteria(computers, sort1, sort2, isDescending);
            }
            else if (!string.IsNullOrEmpty(sort1))
            {
                computers = SortBySingleCriteria(computers, sort1, isDescending);
            }

            DisplayComputers(computers);
        }
        private List<Computer> SortBySingleCriteria(List<Computer> list, string criteria, bool isDescending)
        {
            var sortedList = isDescending
                ? list.OrderByDescending(c => GetSortingValue(c, criteria)).ToList()
                : list.OrderBy(c => GetSortingValue(c, criteria)).ToList();

            return sortedList;
        }

        private List<Computer> SortByTwoCriteria(List<Computer> list, string firstCriteria, string secondCriteria, bool isDescending)
        {
            var sortedList = isDescending
                ? list.OrderByDescending(c => GetSortingValue(c, firstCriteria))
                      .ThenByDescending(c => GetSortingValue(c, secondCriteria))
                      .ToList()
                : list.OrderBy(c => GetSortingValue(c, firstCriteria))
                      .ThenBy(c => GetSortingValue(c, secondCriteria))
                      .ToList();

            return sortedList;
        }

        private IComparable GetSortingValue(Computer c, string criteria)
        {
            return criteria switch
            {
                "Цена" => c.Price,
                "ОЗУ" => c.RamSize,
                "Частота CPU" => c.Cpu.Frequency,
                "Объем Диска" => c.DiskSize,
                "Тип"=> c.Type,
                _ => 0 // Значение по умолчанию (не используется в сортировке)
            };
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (computers == null || !computers.Any())
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON файлы (*.json)|*.json";
                saveFileDialog.Title = "Сохранить результаты сортировки";
                saveFileDialog.FileName = "SortedResults.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(computers, Formatting.Indented);
                        File.WriteAllText(saveFileDialog.FileName, json);
                        MessageBox.Show("Результаты сортировки сохранены успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
