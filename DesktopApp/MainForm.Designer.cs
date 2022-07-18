namespace DesktopApp
{
    partial class MainForm
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.newProjectButton = new System.Windows.Forms.Button();
            this.projectTabControl = new System.Windows.Forms.TabControl();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // newProjectButton
            // 
            this.newProjectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.newProjectButton.FlatAppearance.BorderSize = 0;
            this.newProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("newProjectButton.Image")));
            this.newProjectButton.Location = new System.Drawing.Point(12, 12);
            this.newProjectButton.Name = "newProjectButton";
            this.newProjectButton.Size = new System.Drawing.Size(60, 62);
            this.newProjectButton.TabIndex = 0;
            this.newProjectButton.UseVisualStyleBackColor = true;
            this.newProjectButton.Click += new System.EventHandler(this.newProjectButton_Click);
            this.newProjectButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.newProjectButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // projectTabControl
            // 
            this.projectTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectTabControl.Location = new System.Drawing.Point(12, 82);
            this.projectTabControl.Name = "projectTabControl";
            this.projectTabControl.SelectedIndex = 0;
            this.projectTabControl.Size = new System.Drawing.Size(672, 532);
            this.projectTabControl.TabIndex = 1;
            // 
            // loadingLabel
            // 
            this.loadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingLabel.Image = ((System.Drawing.Image)(resources.GetObject("loadingLabel.Image")));
            this.loadingLabel.Location = new System.Drawing.Point(636, 15);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(48, 48);
            this.loadingLabel.TabIndex = 5;
            this.loadingLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(693, 626);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.projectTabControl);
            this.Controls.Add(this.newProjectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Power BI - App";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newProjectButton;
        private System.Windows.Forms.TabControl projectTabControl;
        private System.Windows.Forms.Label loadingLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

