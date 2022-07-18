using BusinessLayer;
using BusinessLayer.DatasetTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class CreateProjectForm : Form
    {
        Action<Project> AddProject;
        private App app;
        DatasetTemplate DatasetTemplate;
        public CreateProjectForm(Action<Project> AddProject)
        {
            InitializeComponent();
            app = App.GetInstance();
            this.AddProject = AddProject;
            DatasetTemplate = app.DatasetTemplate;
        }

        private void CreateProject(object sender, EventArgs e)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("ProjectName", projectNameTextBox.Text);
            parameters.Add("ProjectStart", projectStartDateTimePicker.Value.ToString(@"yyyy\/MM\/dd"));
            parameters.Add("ProjectRelease", projectReleaseDateTimePicker.Value.ToString(@"yyyy\/MM\/dd"));
            parameters.Add("Budget", budgetTextBox.Text);
            parameters.Add("RiskBuffer", riskBufferTextBox.Text);

            SetUpLoading(true);
            app.CreateProject(parameters, DatasetTemplate, ProjectAccepted, ProjectCanceled);
 
        }
        
        private void ProjectAccepted(Project project)
        {
            this.Invoke((Action)(() => {
                AddProject(project);
                MessageBox.Show($"Project {project.Name} was succesfully created.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }));
        }
        private void ProjectCanceled(Exception ex)
        {
            this.Invoke((Action)(() => { 
                SetUpLoading(false);
                MessageBox.Show(ex.Message, "Notification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
            
        }
        private void SetUpLoading(bool val)
        {
            loadingLabel.Visible = val;
            createProjectButton.Enabled = !val;
            editDatasetButton.Enabled = !val;
            panel1.Enabled = !val;
        }
        private void CreateProjectForm_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(createProjectButton, "Create project");
            toolTip1.SetToolTip(editDatasetButton, "Edit dataset template");
        }

        private void projectNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CreateProject(sender, EventArgs.Empty);
        }

        private void editDatasetButton_Click(object sender, EventArgs e)
        {
            var form = new EditTableTemplateForm(DatasetTemplate, EditDatasetTemplate);
            form.Show();
        }
        private void EditDatasetTemplate(DatasetTemplate dataset)
        {
            DatasetTemplate = dataset;
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
