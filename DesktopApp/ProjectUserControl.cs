using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using System.Threading;

namespace DesktopApp
{
    public partial class ProjectUserControl : UserControl
    {
        Project Project;
        Action<bool> ShowLoading;
        public ProjectUserControl(Project project, Action<bool> ShowLoading)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Project = project;
            this.ShowLoading = ShowLoading;
            //webBrowser.Navigate(project.Url);
            
        }

        private void removeProjectButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Do you really want delete project {Project.Name}?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            SetUpLoading(true);
            var thread = new Thread(() => {

                Project.Remove();
                Invoke((Action)(() => {
                    SetUpLoading(false);
                }));
            });
            thread.Start();
        }

        private void SetUpLoading(bool val)
        {
            ShowLoading(val);
            Enabled = !val;
        }

        private void openReportInBrowser_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Project.ReportUrl);
        }

        private void refreshButton_Click_1(object sender, EventArgs e)
        {
            var form = new RefreshArtefactForm(Project, UpdateRefreshHistory);
            form.Show();
        }

        private void ProjectUserControl_Load(object sender, EventArgs e)
        {
            UpdateRefreshHistory();
            toolTip1.SetToolTip(openReportInBrowser, "Show project in Power BI");
            toolTip1.SetToolTip(editTemplateButton, "Edit template in Power BI");
            toolTip1.SetToolTip(refreshButton, "Update artefact");
            toolTip1.SetToolTip(removeProjectButton, "Remove project");
        }

        private void editTemplateButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Project.EditReportUrl);
        }

        private void UpdateRefreshHistory()
        {
            RefreshHistoryListView.Items.Clear();

            var items = new List<ListViewItem>();
            foreach(var artefact in Project.RefreshHistory.Values)
            {
                var artefactName = artefact["TableName"].ToString();
                var fileName = artefact["FileName"].ToString();
                var date = artefact["Date"].ToString();
                var item = new ListViewItem(new string[] { artefactName, date, fileName });
                items.Add(item);
            }
            RefreshHistoryListView.Items.AddRange(items.ToArray());
        }

        private void Button_DragEnter(object sender, DragEventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.Bisque;
        }

        private void Button_DragLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.BackColor = Color.White;
        }
    }
}
