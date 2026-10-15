namespace LR2
{
    partial class Form3
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.manufacturer = new System.Windows.Forms.TextBox();
            this.series = new System.Windows.Forms.TextBox();
            this.model = new System.Windows.Forms.TextBox();
            this.frequency = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.saveButtonf3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.frequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(293, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Видеокарта";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(35, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Производитель";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(35, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Серия";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(346, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Модель";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(346, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 29);
            this.label5.TabIndex = 4;
            this.label5.Text = "Частота (в МГц)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(639, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 29);
            this.label6.TabIndex = 5;
            this.label6.Text = "Объем памяти";
            // 
            // manufacturer
            // 
            this.manufacturer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.manufacturer.Location = new System.Drawing.Point(40, 149);
            this.manufacturer.Name = "manufacturer";
            this.manufacturer.Size = new System.Drawing.Size(230, 35);
            this.manufacturer.TabIndex = 6;
            // 
            // series
            // 
            this.series.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.series.Location = new System.Drawing.Point(40, 293);
            this.series.Name = "series";
            this.series.Size = new System.Drawing.Size(230, 35);
            this.series.TabIndex = 7;
            // 
            // model
            // 
            this.model.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.model.Location = new System.Drawing.Point(351, 149);
            this.model.Name = "model";
            this.model.Size = new System.Drawing.Size(215, 35);
            this.model.TabIndex = 8;
            // 
            // frequency
            // 
            this.frequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.frequency.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.frequency.Location = new System.Drawing.Point(351, 293);
            this.frequency.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.frequency.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.frequency.Name = "frequency";
            this.frequency.Size = new System.Drawing.Size(120, 35);
            this.frequency.TabIndex = 9;
            this.frequency.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.numericUpDown2.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.Location = new System.Drawing.Point(644, 149);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(180, 35);
            this.numericUpDown2.TabIndex = 10;
            this.numericUpDown2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // saveButtonf3
            // 
            this.saveButtonf3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.saveButtonf3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.saveButtonf3.Location = new System.Drawing.Point(644, 272);
            this.saveButtonf3.Name = "saveButtonf3";
            this.saveButtonf3.Size = new System.Drawing.Size(190, 56);
            this.saveButtonf3.TabIndex = 11;
            this.saveButtonf3.Text = "Сохранить";
            this.saveButtonf3.UseVisualStyleBackColor = false;
            this.saveButtonf3.Click += new System.EventHandler(this.saveButtonf3_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 450);
            this.Controls.Add(this.saveButtonf3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.frequency);
            this.Controls.Add(this.model);
            this.Controls.Add(this.series);
            this.Controls.Add(this.manufacturer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.frequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox manufacturer;
        private System.Windows.Forms.TextBox series;
        private System.Windows.Forms.TextBox model;
        private System.Windows.Forms.NumericUpDown frequency;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button saveButtonf3;
    }
}