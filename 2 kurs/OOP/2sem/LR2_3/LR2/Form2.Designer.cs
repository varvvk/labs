namespace LR2
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.manufactComboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.seriesTextBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.modelTextBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.coresNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.frequencyNumeric2 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.L3numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.L2numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.L1numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.saveButtonf2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.coresNumericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L3numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L2numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.L1numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(300, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Процессор";
            // 
            // manufactComboBox1
            // 
            this.manufactComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.manufactComboBox1.FormattingEnabled = true;
            this.manufactComboBox1.Items.AddRange(new object[] {
            "Intel",
            "AMD",
            "ARM",
            "Qualcomm"});
            this.manufactComboBox1.Location = new System.Drawing.Point(41, 139);
            this.manufactComboBox1.Name = "manufactComboBox1";
            this.manufactComboBox1.Size = new System.Drawing.Size(149, 37);
            this.manufactComboBox1.TabIndex = 1;
            this.manufactComboBox1.Text = "Intel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(36, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Производитель";
            // 
            // seriesTextBox1
            // 
            this.seriesTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.seriesTextBox1.Location = new System.Drawing.Point(41, 253);
            this.seriesTextBox1.Name = "seriesTextBox1";
            this.seriesTextBox1.Size = new System.Drawing.Size(191, 35);
            this.seriesTextBox1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(38, 210);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(87, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "Серия";
            // 
            // modelTextBox2
            // 
            this.modelTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.modelTextBox2.Location = new System.Drawing.Point(41, 380);
            this.modelTextBox2.Name = "modelTextBox2";
            this.modelTextBox2.Size = new System.Drawing.Size(191, 35);
            this.modelTextBox2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(36, 332);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "Модель";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(321, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(214, 29);
            this.label5.TabIndex = 7;
            this.label5.Text = "Количество ядер";
            // 
            // coresNumericUpDown1
            // 
            this.coresNumericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.coresNumericUpDown1.Location = new System.Drawing.Point(326, 139);
            this.coresNumericUpDown1.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.coresNumericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.coresNumericUpDown1.Name = "coresNumericUpDown1";
            this.coresNumericUpDown1.Size = new System.Drawing.Size(120, 35);
            this.coresNumericUpDown1.TabIndex = 8;
            this.coresNumericUpDown1.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(321, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 29);
            this.label6.TabIndex = 9;
            this.label6.Text = "Частота, ГГц";
            // 
            // frequencyNumeric2
            // 
            this.frequencyNumeric2.DecimalPlaces = 1;
            this.frequencyNumeric2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.frequencyNumeric2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.frequencyNumeric2.Location = new System.Drawing.Point(326, 253);
            this.frequencyNumeric2.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.frequencyNumeric2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequencyNumeric2.Name = "frequencyNumeric2";
            this.frequencyNumeric2.Size = new System.Drawing.Size(120, 35);
            this.frequencyNumeric2.TabIndex = 10;
            this.frequencyNumeric2.Value = new decimal(new int[] {
            31,
            0,
            0,
            65536});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(321, 332);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(314, 29);
            this.label7.TabIndex = 11;
            this.label7.Text = "Разрядность архитектуры";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioButton1.Location = new System.Drawing.Point(347, 380);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(111, 33);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "32 бит";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioButton2.Location = new System.Drawing.Point(498, 380);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(111, 33);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "64 бит";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label9.Location = new System.Drawing.Point(6, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 29);
            this.label9.TabIndex = 15;
            this.label9.Text = "L1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label10.Location = new System.Drawing.Point(6, 140);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 29);
            this.label10.TabIndex = 16;
            this.label10.Text = "L2";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label11.Location = new System.Drawing.Point(6, 222);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 29);
            this.label11.TabIndex = 17;
            this.label11.Text = "L3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.L3numericUpDown5);
            this.groupBox1.Controls.Add(this.L2numericUpDown4);
            this.groupBox1.Controls.Add(this.L1numericUpDown3);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.groupBox1.Location = new System.Drawing.Point(675, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 323);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Размер кэша (КВ)";
            // 
            // L3numericUpDown5
            // 
            this.L3numericUpDown5.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.L3numericUpDown5.Location = new System.Drawing.Point(122, 220);
            this.L3numericUpDown5.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.L3numericUpDown5.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.L3numericUpDown5.Name = "L3numericUpDown5";
            this.L3numericUpDown5.Size = new System.Drawing.Size(120, 35);
            this.L3numericUpDown5.TabIndex = 20;
            this.L3numericUpDown5.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // L2numericUpDown4
            // 
            this.L2numericUpDown4.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.L2numericUpDown4.Location = new System.Drawing.Point(122, 138);
            this.L2numericUpDown4.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.L2numericUpDown4.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.L2numericUpDown4.Name = "L2numericUpDown4";
            this.L2numericUpDown4.Size = new System.Drawing.Size(120, 35);
            this.L2numericUpDown4.TabIndex = 19;
            this.L2numericUpDown4.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // L1numericUpDown3
            // 
            this.L1numericUpDown3.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.L1numericUpDown3.Location = new System.Drawing.Point(122, 55);
            this.L1numericUpDown3.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.L1numericUpDown3.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.L1numericUpDown3.Name = "L1numericUpDown3";
            this.L1numericUpDown3.Size = new System.Drawing.Size(120, 35);
            this.L1numericUpDown3.TabIndex = 18;
            this.L1numericUpDown3.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // saveButtonf2
            // 
            this.saveButtonf2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.saveButtonf2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.saveButtonf2.Location = new System.Drawing.Point(419, 463);
            this.saveButtonf2.Name = "saveButtonf2";
            this.saveButtonf2.Size = new System.Drawing.Size(190, 54);
            this.saveButtonf2.TabIndex = 19;
            this.saveButtonf2.Text = "Сохранить";
            this.saveButtonf2.UseVisualStyleBackColor = false;
            this.saveButtonf2.Click += new System.EventHandler(this.saveButtonf2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 549);
            this.Controls.Add(this.saveButtonf2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.frequencyNumeric2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.coresNumericUpDown1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.modelTextBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.seriesTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.manufactComboBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.coresNumericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.L3numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L2numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.L1numericUpDown3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox manufactComboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox seriesTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox modelTextBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown coresNumericUpDown1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown frequencyNumeric2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown L1numericUpDown3;
        private System.Windows.Forms.NumericUpDown L3numericUpDown5;
        private System.Windows.Forms.NumericUpDown L2numericUpDown4;
        private System.Windows.Forms.Button saveButtonf2;
    }
}