namespace LR2
{
    partial class SortForm
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSort1 = new System.Windows.Forms.ComboBox();
            this.comboBoxSort2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonDescending2 = new System.Windows.Forms.RadioButton();
            this.radioButtonDescending = new System.Windows.Forms.RadioButton();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.richTextBox1.Location = new System.Drawing.Point(24, 51);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(450, 594);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(633, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сортировать по";
            // 
            // comboBoxSort1
            // 
            this.comboBoxSort1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxSort1.FormattingEnabled = true;
            this.comboBoxSort1.Items.AddRange(new object[] {
            "Цена",
            "ОЗУ",
            "Частота CPU",
            "Объем Диска",
            "Тип"});
            this.comboBoxSort1.Location = new System.Drawing.Point(624, 129);
            this.comboBoxSort1.Name = "comboBoxSort1";
            this.comboBoxSort1.Size = new System.Drawing.Size(215, 37);
            this.comboBoxSort1.TabIndex = 2;
            this.comboBoxSort1.Text = "Цена";
            // 
            // comboBoxSort2
            // 
            this.comboBoxSort2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.comboBoxSort2.FormattingEnabled = true;
            this.comboBoxSort2.Items.AddRange(new object[] {
            "Цена",
            "ОЗУ",
            "Частота CPU",
            "Объем Диска"});
            this.comboBoxSort2.Location = new System.Drawing.Point(624, 230);
            this.comboBoxSort2.Name = "comboBoxSort2";
            this.comboBoxSort2.Size = new System.Drawing.Size(215, 37);
            this.comboBoxSort2.TabIndex = 3;
            this.comboBoxSort2.Text = "Цена";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.button1.Location = new System.Drawing.Point(640, 475);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 54);
            this.button1.TabIndex = 4;
            this.button1.Text = "Сортировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButtonDescending2
            // 
            this.radioButtonDescending2.AutoSize = true;
            this.radioButtonDescending2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioButtonDescending2.Location = new System.Drawing.Point(517, 335);
            this.radioButtonDescending2.Name = "radioButtonDescending2";
            this.radioButtonDescending2.Size = new System.Drawing.Size(227, 33);
            this.radioButtonDescending2.TabIndex = 5;
            this.radioButtonDescending2.TabStop = true;
            this.radioButtonDescending2.Text = "По возрастанию";
            this.radioButtonDescending2.UseVisualStyleBackColor = true;
            // 
            // radioButtonDescending
            // 
            this.radioButtonDescending.AutoSize = true;
            this.radioButtonDescending.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioButtonDescending.Location = new System.Drawing.Point(735, 402);
            this.radioButtonDescending.Name = "radioButtonDescending";
            this.radioButtonDescending.Size = new System.Drawing.Size(191, 33);
            this.radioButtonDescending.TabIndex = 6;
            this.radioButtonDescending.TabStop = true;
            this.radioButtonDescending.Text = "По убыванию";
            this.radioButtonDescending.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonSave.Location = new System.Drawing.Point(532, 590);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(182, 55);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 713);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.radioButtonDescending);
            this.Controls.Add(this.radioButtonDescending2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxSort2);
            this.Controls.Add(this.comboBoxSort1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "SortForm";
            this.Text = "SortForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSort1;
        private System.Windows.Forms.ComboBox comboBoxSort2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonDescending2;
        private System.Windows.Forms.RadioButton radioButtonDescending;
        private System.Windows.Forms.Button buttonSave;
    }
}