using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;


namespace LR2
{
    public partial class SearchResultsForm : Form
    {
        private List<Computer> computers;
        public SearchResultsForm(List<Computer> computers)
        {
            InitializeComponent();
            this.computers = computers ?? new List<Computer>();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            List<Computer> results = GetFilteredResults();

            richTextBoxResults.Clear();

            if (results.Any())
            {
                foreach (var comp in results)
                {
                    richTextBoxResults.AppendText(FormatComputerInfo(comp) + "\r\n" + new string('-', 50) + "\r\n");
                }
            }
            else
            {
                richTextBoxResults.Text = "Совпадений не найдено.";
            }
        }

        private bool MatchesExactSearch(Computer comp, string searchText)
        {
            string lowerSearch = searchText.ToLower();

            return GetAllComputerValues(comp)
                .Any(value => value.Equals(lowerSearch, StringComparison.OrdinalIgnoreCase));
        }

        private bool MatchesPartialSearch(Computer comp, string searchText)
        {
            Regex regex = new Regex(Regex.Escape(searchText), RegexOptions.IgnoreCase);
            return GetAllComputerValues(comp).Any(value => regex.IsMatch(value));
        }

        private bool MatchesLetterPosition(Computer comp, string searchText)
        {
            if (searchText.Length < 2 || !char.IsLetter(searchText[0]) || !char.IsDigit(searchText[1]))
            {
                return false;
            }

            char letter = searchText[0]; // Буква
            int position = int.Parse(searchText[1].ToString()) - 1; // Позиция (минус 1, так как индексы с 0)

            Regex regex = new Regex($"^.{{{position}}}{letter}.*$", RegexOptions.IgnoreCase);

            return GetAllComputerValues(comp).Any(value => value.Length > position && regex.IsMatch(value));
        }

        private List<string> GetAllComputerValues(Computer comp)
        {
            return new List<string>
            {
                comp.Type, comp.RamSize.ToString(), comp.RamType,
                comp.DiskSize.ToString(), comp.DiskType, comp.GetDate.ToString("yyyy-MM-dd"),
                comp.Price.ToString(), comp.Cpu.Manufacturer, comp.Cpu.Series, comp.Cpu.Model,
                comp.Cpu.Cores.ToString(), comp.Cpu.Frequency.ToString(), comp.Cpu.Architecture.ToString(),
                comp.Cpu.CacheL1.ToString(), comp.Cpu.CacheL2.ToString(), comp.Cpu.CacheL3.ToString(),
                comp.Gpu.Manufacturer, comp.Gpu.Series, comp.Gpu.Model,
                comp.Gpu.Frequency.ToString(), comp.Gpu.MemorySize.ToString()
            };
        }

        private string FormatComputerInfo(Computer comp)
        {
            return $"Тип: {comp.Type}, RAM: {comp.RamSize}GB ({comp.RamType}), \r\n" +
                   $"Диск: {comp.DiskSize}GB ({comp.DiskType}), Дата получения: {comp.GetDate:yyyy-MM-dd}, \r\n" +
                   $"Цена: {comp.Price} руб, CPU: {comp.Cpu.Manufacturer} Серия: {comp.Cpu.Series} Модель: {comp.Cpu.Model} \r\n" +
                   $"Количество ядер: {comp.Cpu.Cores} Частота: {comp.Cpu.Frequency}GHz, GPU: Производитель: {comp.Gpu.Manufacturer} Серия: {comp.Gpu.Series} \r\n" +
                   $"Модель: {comp.Gpu.Model} Объем: {comp.Gpu.MemorySize}GB";
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (richTextBoxResults.Text == "Совпадений не найдено." || string.IsNullOrWhiteSpace(richTextBoxResults.Text))
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Computer> searchResults = GetFilteredResults();

            if (searchResults == null || !searchResults.Any())
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON файлы (*.json)|*.json";
                saveFileDialog.Title = "Сохранить результаты поиска";
                saveFileDialog.FileName = "SearchResults.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(searchResults, Formatting.Indented);
                        File.WriteAllText(saveFileDialog.FileName, json);
                        MessageBox.Show("Результаты поиска сохранены успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private List<Computer> GetFilteredResults()
        {
            string searchText = textBoxSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                return new List<Computer>();
            }

            bool isPartialMatch = checkBoxPartialMatch.Checked;
            bool isRangeFilter = checkBoxRange.Checked;
            bool isLetterMatch = checkBoxLetter.Checked;

            List<Computer> results;

            if (isLetterMatch)
            {
                results = computers.Where(comp => MatchesLetterPosition(comp, searchText)).ToList();
            }
            else if (isPartialMatch)
            {
                results = computers.Where(comp => MatchesPartialSearch(comp, searchText)).ToList();
            }
            else
            {
                results = computers.Where(comp => MatchesExactSearch(comp, searchText)).ToList();
            }

            if (isRangeFilter)
            {
                results = results.Where(comp => comp.Price >= 3000 && comp.Price <= 4000).ToList();
            }

            return results;
        }

    }
}
