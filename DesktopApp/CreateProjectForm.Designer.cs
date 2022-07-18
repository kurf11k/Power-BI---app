namespace DesktopApp
{
    partial class CreateProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateProjectForm));
            this.label1 = new System.Windows.Forms.Label();
            this.projectNameTextBox = new System.Windows.Forms.TextBox();
            this.createProjectButton = new System.Windows.Forms.Button();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.budgetTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.projectStartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.projectReleaseDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.riskBufferTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.editDatasetButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // projectNameTextBox
            // 
            this.projectNameTextBox.Location = new System.Drawing.Point(165, 15);
            this.projectNameTextBox.Name = "projectNameTextBox";
            this.projectNameTextBox.Size = new System.Drawing.Size(163, 26);
            this.projectNameTextBox.TabIndex = 0;
            this.projectNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.projectNameTextBox_KeyDown);
            // 
            // createProjectButton
            // 
            this.createProjectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.createProjectButton.FlatAppearance.BorderSize = 0;
            this.createProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("createProjectButton.Image")));
            this.createProjectButton.Location = new System.Drawing.Point(333, 337);
            this.createProjectButton.Name = "createProjectButton";
            this.createProjectButton.Size = new System.Drawing.Size(74, 71);
            this.createProjectButton.TabIndex = 5;
            this.createProjectButton.UseVisualStyleBackColor = true;
            this.createProjectButton.Click += new System.EventHandler(this.CreateProject);
            this.createProjectButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.createProjectButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // loadingLabel
            // 
            this.loadingLabel.Image = ((System.Drawing.Image)(resources.GetObject("loadingLabel.Image")));
            this.loadingLabel.Location = new System.Drawing.Point(360, 12);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(52, 62);
            this.loadingLabel.TabIndex = 23;
            this.loadingLabel.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(16, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 29);
            this.label4.TabIndex = 14;
            this.label4.Text = "Start";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(16, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 29);
            this.label5.TabIndex = 16;
            this.label5.Text = "Release";
            // 
            // budgetTextBox
            // 
            this.budgetTextBox.Location = new System.Drawing.Point(165, 192);
            this.budgetTextBox.Name = "budgetTextBox";
            this.budgetTextBox.Size = new System.Drawing.Size(163, 26);
            this.budgetTextBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(16, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 29);
            this.label6.TabIndex = 18;
            this.label6.Text = "Budget";
            // 
            // projectStartDateTimePicker
            // 
            this.projectStartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.projectStartDateTimePicker.Location = new System.Drawing.Point(165, 69);
            this.projectStartDateTimePicker.Name = "projectStartDateTimePicker";
            this.projectStartDateTimePicker.Size = new System.Drawing.Size(163, 26);
            this.projectStartDateTimePicker.TabIndex = 1;
            // 
            // projectReleaseDateTimePicker
            // 
            this.projectReleaseDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.projectReleaseDateTimePicker.Location = new System.Drawing.Point(165, 129);
            this.projectReleaseDateTimePicker.Name = "projectReleaseDateTimePicker";
            this.projectReleaseDateTimePicker.Size = new System.Drawing.Size(163, 26);
            this.projectReleaseDateTimePicker.TabIndex = 2;
            // 
            // riskBufferTextBox
            // 
            this.riskBufferTextBox.Location = new System.Drawing.Point(165, 258);
            this.riskBufferTextBox.Name = "riskBufferTextBox";
            this.riskBufferTextBox.Size = new System.Drawing.Size(163, 26);
            this.riskBufferTextBox.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.Location = new System.Drawing.Point(16, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 29);
            this.label7.TabIndex = 22;
            this.label7.Text = "Risk Buffer";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.riskBufferTextBox);
            this.panel1.Controls.Add(this.projectNameTextBox);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.projectReleaseDateTimePicker);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.projectStartDateTimePicker);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.budgetTextBox);
            this.panel1.Location = new System.Drawing.Point(15, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 306);
            this.panel1.TabIndex = 0;
            // 
            // editDatasetButton
            // 
            this.editDatasetButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editDatasetButton.FlatAppearance.BorderSize = 0;
            this.editDatasetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editDatasetButton.Image = ((System.Drawing.Image)(resources.GetObject("editDatasetButton.Image")));
            this.editDatasetButton.Location = new System.Drawing.Point(254, 337);
            this.editDatasetButton.Name = "editDatasetButton";
            this.editDatasetButton.Size = new System.Drawing.Size(74, 71);
            this.editDatasetButton.TabIndex = 6;
            this.editDatasetButton.UseVisualStyleBackColor = true;
            this.editDatasetButton.Click += new System.EventHandler(this.editDatasetButton_Click);
            this.editDatasetButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.editDatasetButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // CreateProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(423, 414);
            this.Controls.Add(this.editDatasetButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.createProjectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CreateProjectForm";
            this.Text = "Power BI - App";
            this.Load += new System.EventHandler(this.CreateProjectForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox projectNameTextBox;
        private System.Windows.Forms.Button createProjectButton;
        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox budgetTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker projectStartDateTimePicker;
        private System.Windows.Forms.DateTimePicker projectReleaseDateTimePicker;
        private System.Windows.Forms.TextBox riskBufferTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button editDatasetButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}