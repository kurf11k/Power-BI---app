using BusinessLayer;
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
    public partial class MainForm : Form
    {
        private App app;
        
        public MainForm()
        {
            InitializeComponent();
            app = App.GetInstance();
        }

        private void newProjectButton_Click(object sender, EventArgs e)
        {
            var form = new CreateProjectForm(AddProject);
            form.Show();
        }

        private void AddProject(Project project)
        {          
            var projectUserControl = new ProjectUserControl(project, SetUpLoading);

            project.RemoveProject += RemoveProject;
            var tab = new TabPage(project.Name);
            tab.BackColor = Color.Red;

            tab.Controls.Add(projectUserControl);
            
            projectTabControl.TabPages.Add(tab);
            //tab.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            //projectUserControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Hide();
            toolTip1.SetToolTip(newProjectButton, "Add new project");
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Enabled = false;
            var form = new LoginForm(LoginSuccesfully, CloseApp);
            form.Show(); 
        }
        private void LoginSuccesfully()
        {
            Enabled = true;
            //WindowState = FormWindowState.Maximized;
            
            LoadData();
        }

        private void CloseApp()
        {
            Close();
            Application.Exit();
        }

        private void LoadData()
        {
            var projects = app.LoadProjects();
            foreach(var project in projects)
            {               
                AddProject(project);
            }
        }

        private void RemoveProject(object sender, EventArgs args)
        {
            var project = sender as Project;
            TabPage tabToRemove = null;
            foreach(TabPage tab in projectTabControl.Controls)
            {
                if(tab.Text == project.Name)
                {
                    tabToRemove = tab;
                    break;
                }    
            }
            Invoke((Action)(() => projectTabControl.TabPages.Remove(tabToRemove)));
        }

        private void SetUpLoading(bool val)
        {
            loadingLabel.Visible = val;
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
