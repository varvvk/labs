namespace LR2
{
    partial class SearchResultsForm
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
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.checkBoxRange = new System.Windows.Forms.CheckBox();
            this.checkBoxPartialMatch = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.checkBoxLetter = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxResults = new System.Windows.Forms.RichTextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBoxSearch.Location = new System.Drawing.Point(216, 64);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(535, 35);
            this.textBoxSearch.TabIndex = 1;
            // 
            // checkBoxRange
            // 
            this.checkBoxRange.AutoSize = true;
            this.checkBoxRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxRange.Location = new System.Drawing.Point(21, 131);
            this.checkBoxRange.Name = "checkBoxRange";
            this.checkBoxRange.Size = new System.Drawing.Size(521, 33);
            this.checkBoxRange.TabIndex = 3;
            this.checkBoxRange.Text = "Поиск в диапазоне (цена от 3000 до 4000)";
            this.checkBoxRange.UseVisualStyleBackColor = true;
            // 
            // checkBoxPartialMatch
            // 
            this.checkBoxPartialMatch.AutoSize = true;
            this.checkBoxPartialMatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxPartialMatch.Location = new System.Drawing.Point(641, 151);
            this.checkBoxPartialMatch.Name = "checkBoxPartialMatch";
            this.checkBoxPartialMatch.Size = new System.Drawing.Size(320, 33);
            this.checkBoxPartialMatch.TabIndex = 4;
            this.checkBoxPartialMatch.Text = "Неполное соответствие";
            this.checkBoxPartialMatch.UseVisualStyleBackColor = true;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonSearch.Location = new System.Drawing.Point(414, 279);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(196, 44);
            this.buttonSearch.TabIndex = 5;
            this.buttonSearch.Text = "Поиск";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // checkBoxLetter
            // 
            this.checkBoxLetter.AutoSize = true;
            this.checkBoxLetter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkBoxLetter.Location = new System.Drawing.Point(21, 180);
            this.checkBoxLetter.Name = "checkBoxLetter";
            this.checkBoxLetter.Size = new System.Drawing.Size(528, 33);
            this.checkBoxLetter.TabIndex = 6;
            this.checkBoxLetter.Text = "Наличие букв на определенных позициях";
            this.checkBoxLetter.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(60, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(462, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "Введите в формате БукваПозиция (А0)";
            // 
            // richTextBoxResults
            // 
            this.richTextBoxResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.richTextBoxResults.Location = new System.Drawing.Point(270, 348);
            this.richTextBoxResults.Name = "richTextBoxResults";
            this.richTextBoxResults.ReadOnly = true;
            this.richTextBoxResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxResults.Size = new System.Drawing.Size(469, 283);
            this.richTextBoxResults.TabIndex = 8;
            this.richTextBoxResults.Text = "";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.buttonSave.Location = new System.Drawing.Point(428, 667);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(168, 50);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // SearchResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 746);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.richTextBoxResults);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxLetter);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.checkBoxPartialMatch);
            this.Controls.Add(this.checkBoxRange);
            this.Controls.Add(this.textBoxSearch);
            this.Name = "SearchResultsForm";
            this.Text = "SearchResultsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.CheckBox checkBoxRange;
        private System.Windows.Forms.CheckBox checkBoxPartialMatch;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.CheckBox checkBoxLetter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxResults;
        private System.Windows.Forms.Button buttonSave;
    }
}