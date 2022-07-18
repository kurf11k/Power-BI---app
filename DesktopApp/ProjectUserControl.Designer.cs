namespace DesktopApp
{
    partial class ProjectUserControl
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

        #region Kód vygenerovaný pomocí Návrháře komponent

        /// <summary> 
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectUserControl));
            this.removeProjectButton = new System.Windows.Forms.Button();
            this.openReportInBrowser = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.editTemplateButton = new System.Windows.Forms.Button();
            this.RefreshHistoryListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // removeProjectButton
            // 
            this.removeProjectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removeProjectButton.FlatAppearance.BorderSize = 0;
            this.removeProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.removeProjectButton.Image = ((System.Drawing.Image)(resources.GetObject("removeProjectButton.Image")));
            this.removeProjectButton.Location = new System.Drawing.Point(201, 3);
            this.removeProjectButton.Name = "removeProjectButton";
            this.removeProjectButton.Size = new System.Drawing.Size(57, 48);
            this.removeProjectButton.TabIndex = 1;
            this.removeProjectButton.UseVisualStyleBackColor = true;
            this.removeProjectButton.Click += new System.EventHandler(this.removeProjectButton_Click);
            this.removeProjectButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.removeProjectButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // openReportInBrowser
            // 
            this.openReportInBrowser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.openReportInBrowser.FlatAppearance.BorderSize = 0;
            this.openReportInBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openReportInBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.openReportInBrowser.Image = ((System.Drawing.Image)(resources.GetObject("openReportInBrowser.Image")));
            this.openReportInBrowser.Location = new System.Drawing.Point(12, 3);
            this.openReportInBrowser.Name = "openReportInBrowser";
            this.openReportInBrowser.Size = new System.Drawing.Size(57, 48);
            this.openReportInBrowser.TabIndex = 6;
            this.openReportInBrowser.UseVisualStyleBackColor = true;
            this.openReportInBrowser.Click += new System.EventHandler(this.openReportInBrowser_Click);
            this.openReportInBrowser.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.openReportInBrowser.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // refreshButton
            // 
            this.refreshButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.Location = new System.Drawing.Point(138, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(57, 48);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click_1);
            this.refreshButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.refreshButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // editTemplateButton
            // 
            this.editTemplateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editTemplateButton.FlatAppearance.BorderSize = 0;
            this.editTemplateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editTemplateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.editTemplateButton.Image = ((System.Drawing.Image)(resources.GetObject("editTemplateButton.Image")));
            this.editTemplateButton.Location = new System.Drawing.Point(75, 3);
            this.editTemplateButton.Name = "editTemplateButton";
            this.editTemplateButton.Size = new System.Drawing.Size(57, 48);
            this.editTemplateButton.TabIndex = 8;
            this.editTemplateButton.UseVisualStyleBackColor = true;
            this.editTemplateButton.Click += new System.EventHandler(this.editTemplateButton_Click);
            this.editTemplateButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.Button_DragEnter);
            this.editTemplateButton.DragLeave += new System.EventHandler(this.Button_DragLeave);
            // 
            // RefreshHistoryListView
            // 
            this.RefreshHistoryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.RefreshHistoryListView.HideSelection = false;
            this.RefreshHistoryListView.Location = new System.Drawing.Point(4, 58);
            this.RefreshHistoryListView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshHistoryListView.Name = "RefreshHistoryListView";
            this.RefreshHistoryListView.Size = new System.Drawing.Size(644, 422);
            this.RefreshHistoryListView.TabIndex = 9;
            this.RefreshHistoryListView.UseCompatibleStateImageBehavior = false;
            this.RefreshHistoryListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Artefact";
            this.columnHeader1.Width = 96;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Last update";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Update file name";
            this.columnHeader3.Width = 106;
            // 
            // ProjectUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.RefreshHistoryListView);
            this.Controls.Add(this.editTemplateButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.openReportInBrowser);
            this.Controls.Add(this.removeProjectButton);
            this.Name = "ProjectUserControl";
            this.Size = new System.Drawing.Size(656, 488);
            this.Load += new System.EventHandler(this.ProjectUserControl_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button removeProjectButton;
        private System.Windows.Forms.Button openReportInBrowser;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button editTemplateButton;
        private System.Windows.Forms.ListView RefreshHistoryListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
