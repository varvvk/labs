using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace LR2
{
    public partial class Form2 : Form
    {
        public Processor processor = new Processor();
        public Form2()
        {
            InitializeComponent();
        }

        private void saveButtonf2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(manufactComboBox1.SelectedItem?.ToString()))
            {
                MessageBox.Show("Пожалуйста, выберите производителя процессора.");
                return;
            }

            if (string.IsNullOrEmpty(seriesTextBox1.Text))
            {
                MessageBox.Show("Пожалуйста, укажите серию процессора.");
                return;
            }

            if (string.IsNullOrEmpty(modelTextBox2.Text))
            {
                MessageBox.Show("Пожалуйста, укажите модель процессора.");
                return;
            }

            if (coresNumericUpDown1.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите количество ядер процессора.");
                return;
            }

            if (frequencyNumeric2.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите частоту процессора.");
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Пожалуйста, выберите архитектуру процессора (32 или 64 бита).");
                return;
            }

            if (L1numericUpDown3.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите размер кэша L1.");
                return;
            }

            if (L2numericUpDown4.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите размер кэша L2.");
                return;
            }

            if (L3numericUpDown5.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите размер кэша L3.");
                return;
            }

            processor.Manufacturer = manufactComboBox1.SelectedItem.ToString();
            processor.Series = seriesTextBox1.Text;
            processor.Model = modelTextBox2.Text;
            processor.Cores = (int)coresNumericUpDown1.Value;
            processor.Frequency = (double)frequencyNumeric2.Value;
            processor.Architecture = radioButton1.Checked ? 32 : 64;
            processor.CacheL1 = (int)L1numericUpDown3.Value;
            processor.CacheL2 = (int)L2numericUpDown4.Value;
            processor.CacheL3 = (int)L3numericUpDown5.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
