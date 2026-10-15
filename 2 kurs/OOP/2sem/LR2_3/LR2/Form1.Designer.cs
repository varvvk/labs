namespace LR2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.typeComboBox1 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.ramSizeListBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ramTypeTextBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.diskSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.diskTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CPUinfoButton1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.videoCardButton1 = new System.Windows.Forms.Button();
            this.priceButton1 = new System.Windows.Forms.Button();
            this.saveButton1 = new System.Windows.Forms.Button();
            this.showButton2 = new System.Windows.Forms.Button();
            this.priceOutput = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.searchItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripBack = new System.Windows.Forms.ToolStripButton();
            this.toolStripForward = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLDate = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.diskSizeNumeric)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(757, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ввод данных";
            // 
            // typeComboBox1
            // 
            this.typeComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.typeComboBox1.FormattingEnabled = true;
            this.typeComboBox1.Items.AddRange(new object[] {
            "Ноутбук",
            "Сервер",
            "Рабочая станция"});
            this.typeComboBox1.Location = new System.Drawing.Point(26, 152);
            this.typeComboBox1.Name = "typeComboBox1";
            this.typeComboBox1.Size = new System.Drawing.Size(219, 37);
            this.typeComboBox1.TabIndex = 1;
            this.typeComboBox1.Text = "Ноутбук";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dateTimePicker1.Location = new System.Drawing.Point(314, 381);
            this.dateTimePicker1.MaxDate = new System.DateTime(2099, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.MinDate = new System.DateTime(1974, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(255, 35);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.Value = new System.DateTime(2025, 4, 3, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(21, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "Тип компьютера";
            // 
            // ramSizeListBox1
            // 
            this.ramSizeListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ramSizeListBox1.FormattingEnabled = true;
            this.ramSizeListBox1.ItemHeight = 29;
            this.ramSizeListBox1.Items.AddRange(new object[] {
            "4",
            "8 ",
            "16 ",
            "32"});
            this.ramSizeListBox1.Location = new System.Drawing.Point(26, 273);
            this.ramSizeListBox1.MultiColumn = true;
            this.ramSizeListBox1.Name = "ramSizeListBox1";
            this.ramSizeListBox1.Size = new System.Drawing.Size(91, 120);
            this.ramSizeListBox1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(21, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "Размер ОЗУ (ГБ)";
            // 
            // ramTypeTextBox1
            // 
            this.ramTypeTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ramTypeTextBox1.Location = new System.Drawing.Point(157, 411);
            this.ramTypeTextBox1.Name = "ramTypeTextBox1";
            this.ramTypeTextBox1.Size = new System.Drawing.Size(138, 35);
            this.ramTypeTextBox1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(21, 414);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "Тип ОЗУ";
            // 
            // diskSizeNumeric
            // 
            this.diskSizeNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.diskSizeNumeric.Location = new System.Drawing.Point(314, 152);
            this.diskSizeNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.diskSizeNumeric.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.diskSizeNumeric.Name = "diskSizeNumeric";
            this.diskSizeNumeric.Size = new System.Drawing.Size(120, 35);
            this.diskSizeNumeric.TabIndex = 12;
            this.diskSizeNumeric.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(309, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(286, 29);
            this.label5.TabIndex = 13;
            this.label5.Text = "Размер жесткого диска";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(313, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(244, 29);
            this.label6.TabIndex = 14;
            this.label6.Text = "Тип жесткого диска";
            // 
            // diskTypeComboBox
            // 
            this.diskTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.diskTypeComboBox.FormattingEnabled = true;
            this.diskTypeComboBox.Items.AddRange(new object[] {
            "SSD",
            "HDD",
            "NVMe SSD",
            "External"});
            this.diskTypeComboBox.Location = new System.Drawing.Point(318, 262);
            this.diskTypeComboBox.Name = "diskTypeComboBox";
            this.diskTypeComboBox.Size = new System.Drawing.Size(144, 37);
            this.diskTypeComboBox.TabIndex = 15;
            this.diskTypeComboBox.Text = "SSD";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(309, 325);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(243, 29);
            this.label7.TabIndex = 16;
            this.label7.Text = "Дата приобретения";
            // 
            // CPUinfoButton1
            // 
            this.CPUinfoButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CPUinfoButton1.Location = new System.Drawing.Point(370, 478);
            this.CPUinfoButton1.Name = "CPUinfoButton1";
            this.CPUinfoButton1.Size = new System.Drawing.Size(187, 51);
            this.CPUinfoButton1.TabIndex = 17;
            this.CPUinfoButton1.Text = "Добавить";
            this.CPUinfoButton1.UseVisualStyleBackColor = true;
            this.CPUinfoButton1.Click += new System.EventHandler(this.CPUinfoButton1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBox1.Location = new System.Drawing.Point(26, 471);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(300, 68);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "Добавить информацию о процессоре";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBox2.Location = new System.Drawing.Point(26, 559);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(300, 67);
            this.textBox2.TabIndex = 19;
            this.textBox2.Text = "Добавить информацию о видеокарте";
            // 
            // videoCardButton1
            // 
            this.videoCardButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.videoCardButton1.Location = new System.Drawing.Point(370, 565);
            this.videoCardButton1.Name = "videoCardButton1";
            this.videoCardButton1.Size = new System.Drawing.Size(190, 51);
            this.videoCardButton1.TabIndex = 20;
            this.videoCardButton1.Text = "Добавить";
            this.videoCardButton1.UseVisualStyleBackColor = true;
            this.videoCardButton1.Click += new System.EventHandler(this.videoCardButton1_Click);
            // 
            // priceButton1
            // 
            this.priceButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.priceButton1.Location = new System.Drawing.Point(732, 152);
            this.priceButton1.Name = "priceButton1";
            this.priceButton1.Size = new System.Drawing.Size(270, 79);
            this.priceButton1.TabIndex = 21;
            this.priceButton1.Text = "Рассчитать стоимость";
            this.priceButton1.UseVisualStyleBackColor = true;
            this.priceButton1.Click += new System.EventHandler(this.priceButton1_Click);
            // 
            // saveButton1
            // 
            this.saveButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.saveButton1.Location = new System.Drawing.Point(732, 262);
            this.saveButton1.Name = "saveButton1";
            this.saveButton1.Size = new System.Drawing.Size(270, 68);
            this.saveButton1.TabIndex = 22;
            this.saveButton1.Text = " Сохранить";
            this.saveButton1.UseVisualStyleBackColor = true;
            this.saveButton1.Click += new System.EventHandler(this.saveButton1_Click);
            // 
            // showButton2
            // 
            this.showButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.showButton2.Location = new System.Drawing.Point(732, 391);
            this.showButton2.Name = "showButton2";
            this.showButton2.Size = new System.Drawing.Size(270, 74);
            this.showButton2.TabIndex = 23;
            this.showButton2.Text = "Показать информацию";
            this.showButton2.UseVisualStyleBackColor = true;
            this.showButton2.Click += new System.EventHandler(this.showButton2_Click);
            // 
            // priceOutput
            // 
            this.priceOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.priceOutput.Location = new System.Drawing.Point(732, 531);
            this.priceOutput.Multiline = true;
            this.priceOutput.Name = "priceOutput";
            this.priceOutput.ReadOnly = true;
            this.priceOutput.Size = new System.Drawing.Size(268, 95);
            this.priceOutput.TabIndex = 24;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchItem,
            this.sortItem,
            this.aboutItem,
            this.toggleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1134, 36);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // searchItem
            // 
            this.searchItem.Name = "searchItem";
            this.searchItem.Size = new System.Drawing.Size(85, 32);
            this.searchItem.Text = "Поиск";
            this.searchItem.Click += new System.EventHandler(this.searchItem_Click);
            // 
            // sortItem
            // 
            this.sortItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1});
            this.sortItem.Name = "sortItem";
            this.sortItem.Size = new System.Drawing.Size(139, 32);
            this.sortItem.Text = "Сортировка";
            this.sortItem.Click += new System.EventHandler(this.sortItem_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 33);
            // 
            // aboutItem
            // 
            this.aboutItem.Name = "aboutItem";
            this.aboutItem.Size = new System.Drawing.Size(152, 32);
            this.aboutItem.Text = "О программе";
            this.aboutItem.Click += new System.EventHandler(this.aboutItem_Click);
            // 
            // toggleToolStripMenuItem
            // 
            this.toggleToolStripMenuItem.Name = "toggleToolStripMenuItem";
            this.toggleToolStripMenuItem.Size = new System.Drawing.Size(299, 32);
            this.toggleToolStripMenuItem.Text = "Скрыть панель инструментов";
            this.toggleToolStripMenuItem.Click += new System.EventHandler(this.toggleToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripClear,
            this.toolStripDeleteLast,
            this.toolStripBack,
            this.toolStripForward});
            this.toolStrip1.Location = new System.Drawing.Point(0, 36);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(10, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(551, 37);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 26;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(73, 32);
            this.toolStripButton1.Text = "Поиск";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(127, 32);
            this.toolStripButton2.Text = "Сортировка";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripClear
            // 
            this.toolStripClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripClear.Image")));
            this.toolStripClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripClear.Name = "toolStripClear";
            this.toolStripClear.Size = new System.Drawing.Size(101, 32);
            this.toolStripClear.Text = "Очистить";
            this.toolStripClear.Click += new System.EventHandler(this.toolStripClear_Click);
            // 
            // toolStripDeleteLast
            // 
            this.toolStripDeleteLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDeleteLast.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDeleteLast.Image")));
            this.toolStripDeleteLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteLast.Name = "toolStripDeleteLast";
            this.toolStripDeleteLast.Size = new System.Drawing.Size(154, 32);
            this.toolStripDeleteLast.Text = "Удалить заявку";
            this.toolStripDeleteLast.Click += new System.EventHandler(this.toolStripDeleteLast_Click);
            // 
            // toolStripBack
            // 
            this.toolStripBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBack.Image")));
            this.toolStripBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBack.Name = "toolStripBack";
            this.toolStripBack.Size = new System.Drawing.Size(34, 32);
            this.toolStripBack.Text = "toolStripButton5";
            this.toolStripBack.Click += new System.EventHandler(this.toolStripBack_Click);
            // 
            // toolStripForward
            // 
            this.toolStripForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripForward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripForward.Image")));
            this.toolStripForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripForward.Name = "toolStripForward";
            this.toolStripForward.Size = new System.Drawing.Size(34, 32);
            this.toolStripForward.Text = "toolStripButton6";
            this.toolStripForward.Click += new System.EventHandler(this.toolStripForward_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCount,
            this.toolStripStatusLabelAction,
            this.toolStripStatusLDate});
            this.statusStrip1.Location = new System.Drawing.Point(0, 652);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1134, 32);
            this.statusStrip1.TabIndex = 27;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelCount
            // 
            this.toolStripStatusLabelCount.Name = "toolStripStatusLabelCount";
            this.toolStripStatusLabelCount.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabelCount.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabelAction
            // 
            this.toolStripStatusLabelAction.Name = "toolStripStatusLabelAction";
            this.toolStripStatusLabelAction.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabelAction.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLDate
            // 
            this.toolStripStatusLDate.Name = "toolStripStatusLDate";
            this.toolStripStatusLDate.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLDate.Text = "toolStripStatusLabel2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1134, 684);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.priceOutput);
            this.Controls.Add(this.showButton2);
            this.Controls.Add(this.saveButton1);
            this.Controls.Add(this.priceButton1);
            this.Controls.Add(this.videoCardButton1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.CPUinfoButton1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.diskTypeComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.diskSizeNumeric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ramTypeTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ramSizeListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.typeComboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.diskSizeNumeric)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeComboBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ramSizeListBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ramTypeTextBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown diskSizeNumeric;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox diskTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button CPUinfoButton1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button videoCardButton1;
        private System.Windows.Forms.Button priceButton1;
        private System.Windows.Forms.Button saveButton1;
        private System.Windows.Forms.Button showButton2;
        private System.Windows.Forms.TextBox priceOutput;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem searchItem;
        private System.Windows.Forms.ToolStripMenuItem sortItem;
        private System.Windows.Forms.ToolStripMenuItem aboutItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripClear;
        private System.Windows.Forms.ToolStripButton toolStripDeleteLast;
        private System.Windows.Forms.ToolStripButton toolStripBack;
        private System.Windows.Forms.ToolStripButton toolStripForward;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAction;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLDate;
        private System.Windows.Forms.ToolStripMenuItem toggleToolStripMenuItem;
    }
}

