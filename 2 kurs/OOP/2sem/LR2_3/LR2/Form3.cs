using System;
using System.Windows.Forms;

namespace LR2
{
    public partial class Form3 : Form
    {
        public VideoCard videoCard =  new VideoCard();
        public Form3()
        {
            InitializeComponent();
        }

        private void saveButtonf3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(manufacturer.Text))
            {
                MessageBox.Show("Пожалуйста, укажите производителя видеокарты.");
                return;
            }

            if (string.IsNullOrEmpty(series.Text))
            {
                MessageBox.Show("Пожалуйста, укажите серию видеокарты.");
                return;
            }

            if (string.IsNullOrEmpty(model.Text))
            {
                MessageBox.Show("Пожалуйста, укажите модель видеокарты.");
                return;
            }

            if (frequency.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите частоту видеокарты.");
                return;
            }

            if (numericUpDown2.Value == 0)
            {
                MessageBox.Show("Пожалуйста, укажите размер памяти видеокарты.");
                return;
            }

            // Если все поля заполнены, сохраняем данные
            videoCard.Manufacturer = manufacturer.Text;
            videoCard.Series = series.Text;
            videoCard.Model = model.Text;
            videoCard.Frequency = (double)frequency.Value;
            videoCard.MemorySize = (int)numericUpDown2.Value;

            // Закрываем форму с результатом OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


    }
}
