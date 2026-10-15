using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR2
{
    public partial class Form4 : Form
    {
        private List<Computer> computers;

        public Form4(List<Computer> computers)
        {
            InitializeComponent();
            this.computers = computers ?? new List<Computer>(); // Если список пуст, создаем новый
            listBoxComputers.DataSource = this.computers;
            listBoxComputers.DisplayMember = "Type"; // Отображаем тип компьютера
        }

        private void listBoxComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxComputers.SelectedItem is Computer selectedComputer)
            {
                textBoxf4.Text = $"Тип: {selectedComputer.Type}\r\n" +
                                 $"Тип RAM: {selectedComputer.RamType}\r\n" +
                                 $"Размер RAM: {selectedComputer.RamSize} GB\r\n" +
                                 $"Тип диска: {selectedComputer.DiskType}\r\n" +
                                 $"Размер диска: {selectedComputer.DiskSize} GB\r\n" +
                                 $"Процессор: {selectedComputer.Cpu?.Model} ({selectedComputer.Cpu?.Cores} ядер)\r\n" +
                                 $"Видеокарта: {selectedComputer.Gpu?.Model} ({selectedComputer.Gpu?.MemorySize} GB VRAM)\r\n" +
                                 $"Дата: {selectedComputer.GetDate:dd.MM.yyyy}\r\n" +
                                 $"Цена: {selectedComputer.Price}$";
            }
            else
            {
                textBoxf4.Text = "Выберите компьютер для просмотра деталей.";
            }
        }
    }
}
