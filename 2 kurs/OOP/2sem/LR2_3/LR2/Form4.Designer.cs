namespace LR2
{
    partial class Form4
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
            this.textBoxf4 = new System.Windows.Forms.TextBox();
            this.listBoxComputers = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // textBoxf4
            // 
            this.textBoxf4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.textBoxf4.Location = new System.Drawing.Point(422, 34);
            this.textBoxf4.Multiline = true;
            this.textBoxf4.Name = "textBoxf4";
            this.textBoxf4.ReadOnly = true;
            this.textBoxf4.Size = new System.Drawing.Size(557, 497);
            this.textBoxf4.TabIndex = 0;
            // 
            // listBoxComputers
            // 
            this.listBoxComputers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.listBoxComputers.FormattingEnabled = true;
            this.listBoxComputers.ItemHeight = 29;
            this.listBoxComputers.Location = new System.Drawing.Point(37, 69);
            this.listBoxComputers.Name = "listBoxComputers";
            this.listBoxComputers.Size = new System.Drawing.Size(312, 410);
            this.listBoxComputers.TabIndex = 1;
            this.listBoxComputers.SelectedIndexChanged += new System.EventHandler(this.listBoxComputers_SelectedIndexChanged);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 584);
            this.Controls.Add(this.listBoxComputers);
            this.Controls.Add(this.textBoxf4);
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxf4;
        private System.Windows.Forms.ListBox listBoxComputers;
    }
}