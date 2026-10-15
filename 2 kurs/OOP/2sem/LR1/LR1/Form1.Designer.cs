namespace LR1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.weightBox = new System.Windows.Forms.TextBox();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.ageBox = new System.Windows.Forms.TextBox();
            this.GoalWeightBox = new System.Windows.Forms.TextBox();
            this.GoalComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CalculateBtn = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.isMaleButton = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // weightBox
            // 
            this.weightBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.weightBox.Location = new System.Drawing.Point(34, 209);
            this.weightBox.Name = "weightBox";
            this.weightBox.Size = new System.Drawing.Size(150, 35);
            this.weightBox.TabIndex = 2;
            this.weightBox.Text = "Вес";
            this.weightBox.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // heightBox
            // 
            this.heightBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.heightBox.Location = new System.Drawing.Point(34, 264);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(150, 35);
            this.heightBox.TabIndex = 3;
            this.heightBox.Text = "Рост";
            this.heightBox.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // ageBox
            // 
            this.ageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ageBox.Location = new System.Drawing.Point(34, 322);
            this.ageBox.Name = "ageBox";
            this.ageBox.Size = new System.Drawing.Size(150, 35);
            this.ageBox.TabIndex = 4;
            this.ageBox.Text = "Возраст";
            this.ageBox.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // GoalWeightBox
            // 
            this.GoalWeightBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.GoalWeightBox.Location = new System.Drawing.Point(716, 209);
            this.GoalWeightBox.Name = "GoalWeightBox";
            this.GoalWeightBox.Size = new System.Drawing.Size(250, 35);
            this.GoalWeightBox.TabIndex = 6;
            this.GoalWeightBox.Text = "Желаемый вес";
            this.GoalWeightBox.Enter += new System.EventHandler(this.TextBox_Enter);
            // 
            // GoalComboBox
            // 
            this.GoalComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.GoalComboBox.FormattingEnabled = true;
            this.GoalComboBox.Items.AddRange(new object[] {
            "Снижение веса",
            "Поддержание веса",
            "Увеличение веса"});
            this.GoalComboBox.Location = new System.Drawing.Point(716, 135);
            this.GoalComboBox.Name = "GoalComboBox";
            this.GoalComboBox.Size = new System.Drawing.Size(250, 37);
            this.GoalComboBox.TabIndex = 8;
            this.GoalComboBox.Text = "Снижение веса";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(325, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 37);
            this.label1.TabIndex = 9;
            this.label1.Text = "Калькулятор калорий";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(28, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Введите данные";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(28, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 32);
            this.label3.TabIndex = 11;
            this.label3.Text = "Пол";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(710, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 32);
            this.label4.TabIndex = 12;
            this.label4.Text = "Выберите цель";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(718, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(230, 29);
            this.label5.TabIndex = 13;
            this.label5.Text = "До какого периода";
            // 
            // CalculateBtn
            // 
            this.CalculateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CalculateBtn.Location = new System.Drawing.Point(399, 314);
            this.CalculateBtn.Name = "CalculateBtn";
            this.CalculateBtn.Size = new System.Drawing.Size(180, 50);
            this.CalculateBtn.TabIndex = 14;
            this.CalculateBtn.Text = "Рассчитать";
            this.CalculateBtn.UseVisualStyleBackColor = true;
            this.CalculateBtn.Click += new System.EventHandler(this.CalculateBtn_Click);
            // 
            // outputBox
            // 
            this.outputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.outputBox.Location = new System.Drawing.Point(251, 400);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.Size = new System.Drawing.Size(460, 250);
            this.outputBox.TabIndex = 15;
            // 
            // isMaleButton
            // 
            this.isMaleButton.AutoSize = true;
            this.isMaleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.isMaleButton.Location = new System.Drawing.Point(130, 135);
            this.isMaleButton.Name = "isMaleButton";
            this.isMaleButton.Size = new System.Drawing.Size(140, 33);
            this.isMaleButton.TabIndex = 16;
            this.isMaleButton.TabStop = true;
            this.isMaleButton.Text = "Мужской";
            this.isMaleButton.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioButton2.Location = new System.Drawing.Point(293, 135);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(142, 33);
            this.radioButton2.TabIndex = 17;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Женский";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker1.Location = new System.Drawing.Point(726, 327);
            this.dateTimePicker1.MinDate = new System.DateTime(2025, 2, 27, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1006, 702);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.isMaleButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.CalculateBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GoalComboBox);
            this.Controls.Add(this.GoalWeightBox);
            this.Controls.Add(this.ageBox);
            this.Controls.Add(this.heightBox);
            this.Controls.Add(this.weightBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox weightBox;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.TextBox ageBox;
        private System.Windows.Forms.TextBox GoalWeightBox;
        private System.Windows.Forms.ComboBox GoalComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button CalculateBtn;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.RadioButton isMaleButton;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}

