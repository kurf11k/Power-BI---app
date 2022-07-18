namespace DesktopApp
{
    partial class RefreshArtefactForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RefreshArtefactForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.specificationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTimePicker);
            this.panel1.Controls.Add(this.specificationTypeComboBox);
            this.panel1.Controls.Add(this.browseButton);
            this.panel1.Controls.Add(this.fileTextBox);
            this.panel1.Location = new System.Drawing.Point(14, 14);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(471, 135);
            this.panel1.TabIndex = 6;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dateTimePicker.Location = new System.Drawing.Point(16, 94);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(390, 26);
            this.dateTimePicker.TabIndex = 18;
            // 
            // specificationTypeComboBox
            // 
            this.specificationTypeComboBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.specificationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.specificationTypeComboBox.FormattingEnabled = true;
            this.specificationTypeComboBox.Location = new System.Drawing.Point(16, 52);
            this.specificationTypeComboBox.Name = "specificationTypeComboBox";
            this.specificationTypeComboBox.Size = new System.Drawing.Size(390, 28);
            this.specificationTypeComboBox.TabIndex = 17;
            // 
            // browseButton
            // 
            this.browseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.browseButton.FlatAppearance.BorderSize = 0;
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButton.Image = ((System.Drawing.Image)(resources.GetObject("browseButton.Image")));
            this.browseButton.Location = new System.Drawing.Point(414, 6);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(38, 38);
            this.browseButton.TabIndex = 15;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(16, 12);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size(390, 26);
            this.fileTextBox.TabIndex = 14;
            // 
            // refreshButton
            // 
            this.refreshButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.Location = new System.Drawing.Point(500, 92);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(58, 57);
            this.refreshButton.TabIndex = 16;
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            this.refreshButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.refreshButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // loadingLabel
            // 
            this.loadingLabel.Image = ((System.Drawing.Image)(resources.GetObject("loadingLabel.Image")));
            this.loadingLabel.Location = new System.Drawing.Point(508, 14);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(46, 43);
            this.loadingLabel.TabIndex = 17;
            this.loadingLabel.Visible = false;
            // 
            // RefreshArtefactForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(572, 168);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.refreshButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RefreshArtefactForm";
            this.Text = "Power BI - App";
            this.Load += new System.EventHandler(this.RefreshArtefactForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.ComboBox specificationTypeComboBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}