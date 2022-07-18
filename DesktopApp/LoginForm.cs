using BusinessLayer;
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
    public partial class LoginForm : Form
    {
        private App app;
        Action LoginSuccesfully;
        Action CloseApp;
        bool UserClosed = true;
        public LoginForm(Action LoginSuccesfully, Action CloseApp)
        {
            InitializeComponent();
            app = App.GetInstance();
            this.LoginSuccesfully = LoginSuccesfully;
            this.CloseApp = CloseApp;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (app.LoginToPowerBI())
            {
                UserClosed = false;
                LoginSuccesfully.Invoke();
                Close(); 
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(UserClosed)
                CloseApp();
        }
    }
}
