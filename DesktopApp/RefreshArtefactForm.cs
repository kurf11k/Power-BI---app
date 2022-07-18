using BusinessLayer;
using BusinessLayer.DatasetTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class RefreshArtefactForm : Form
    {
        Project Project;
        Action UpdateRefreshHistory;
        public string FilePath { get { return fileTextBox.Text; } set { fileTextBox.Text = value; } }
        public RefreshArtefactForm(Project project, Action updateRefreshHistory)
        {
            InitializeComponent();
            Project = project;
            UpdateRefreshHistory = updateRefreshHistory;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            fileTextBox.Text = dialog.FileName;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            if(FilePath == "")
            {
                MessageBox.Show("You must enter path to artefact.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            var artefact = (TableTemplate)specificationTypeComboBox.SelectedItem;
            var date = dateTimePicker.Value.ToString(@"yyyy\/MM\/dd");
            SetUpLoading(true);
            Project.UpdateData(FilePath, artefact, date, UpdateAccepted, UpdateFailed);
        }

        private void UpdateAccepted()
        {
            Invoke((Action)(() => {
                UpdateRefreshHistory();
                Close();
            }));
        }

        private void UpdateFailed(string exception)
        {
            Invoke((Action)(() => {
                SetUpLoading(false);
                MessageBox.Show(exception, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        
        }

        private void SetUpLoading(bool val)
        {
            loadingLabel.Visible = val;
            refreshButton.Enabled = !val;
            panel1.Enabled = !val;
        }
        private void RefreshArtefactForm_Load(object sender, EventArgs e)
        {
            specificationTypeComboBox.DataSource = Project.Artefacts;
            specificationTypeComboBox.DisplayMember = "name";

            toolTip1.SetToolTip(refreshButton, "Update");
            toolTip1.SetToolTip(browseButton, "Browse");
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
