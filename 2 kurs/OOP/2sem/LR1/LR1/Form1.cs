using System;
using System.Windows.Forms;

namespace LR1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void CalculateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(weightBox.Text) ||
                    string.IsNullOrWhiteSpace(heightBox.Text) ||
                    string.IsNullOrWhiteSpace(ageBox.Text) ||
                    string.IsNullOrWhiteSpace(GoalWeightBox.Text) ||
                    GoalComboBox.SelectedItem == null ||
                    (!isMaleButton.Checked && !radioButton2.Checked))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(weightBox.Text, out int weight) ||
                    !double.TryParse(heightBox.Text, out double height) ||
                    !int.TryParse(ageBox.Text, out int age) ||
                    !int.TryParse(GoalWeightBox.Text, out int goalWeight))
                {
                    MessageBox.Show("Введите корректные числовые значения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isMale = isMaleButton.Checked;

                string selectedGoal = GoalComboBox.SelectedItem.ToString();

                if ((selectedGoal == "Снижение веса" && goalWeight >= weight) ||
                    (selectedGoal == "Увеличение веса" && goalWeight <= weight))
                {
                    MessageBox.Show("Цель и желаемый вес противоречат друг другу. Пожалуйста, уточните данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime selectedDate = dateTimePicker1.Value;
                DateTime currentDate = DateTime.Now;

                int days = (selectedDate - currentDate).Days;

                if (days <= 0)
                {
                    MessageBox.Show("Дата достижения цели должна быть в будущем!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int weightDifference = Math.Abs(goalWeight - weight);

                Calculator calculator = new Calculator(weight, height, age, isMale, days, weightDifference);

                string bmiInterpretation = calculator.InterpretBodyMassIndex();
                double calorieNorm = calculator.CalorieNormCalc();
                double calorieDeficit = calculator.CalorieCalc();


                string output = $"{bmiInterpretation}\n";

                if (selectedGoal == "Поддержание веса")
                {
                    output += $"Рекомендуемое количество калорий в день: {calorieNorm:F0} ккал.";
                }
                else if (selectedGoal == "Снижение веса" || selectedGoal == "Увеличение веса")
                {
                    if (calorieDeficit > 500)
                    {
                        output += "Достижение цели невозможно или небезопасно при таких условиях.";
                    }
                    else
                    {
                        double recommendedCalories = (selectedGoal == "Снижение веса")
                            ? calorieNorm - calorieDeficit
                            : calorieNorm + calorieDeficit;

                        output += $"Рекомендуемое количество калорий в день: {recommendedCalories:F0} ккал.";
                    }
                }

                outputBox.Text = output;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Ошибка: Попытка деления на ноль. Проверьте данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Clear();
            }
        }

    }
}
