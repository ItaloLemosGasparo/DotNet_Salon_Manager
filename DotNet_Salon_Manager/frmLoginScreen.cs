using System;
using System.Data;
using System.Windows.Forms;
using Classes;
using Globals;
using DotNet_Salon_Manager.modules.Classes;


namespace DotNet_Salon_Manager.modules
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            //string email = txtEmail.Text.Trim();
            //string password = txtPass.Text;

            //if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            //{
            //    MessageBox.Show("Please enter both your email and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (!Global.IsValidEmail(email))
            //{
            //    MessageBox.Show("Invalid Email format.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //try
            //{
            //    DataRow drUser = User.FindUserByEmail(email);

            //    if (drUser != null && Password.VerifyPassword(password, drUser["PasswordHash"].ToString(), drUser["PasswordSalt"].ToString()))
            //    {
            //        LoadUserProfile(drUser);

                    using (frmMain frmMain = new frmMain())
                    {
                        this.Visible = false;
                        frmMain.ShowDialog();
                        Application.Exit();
                    }
            //    }
            //    else
            //        MessageBox.Show("Incorrect Email or Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred while trying to log in. Details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void LoadUserProfile(DataRow drUser)
        {
            UserProfile.StartProfile(
                Convert.ToInt32(drUser["UserId"]),
                drUser["Name"].ToString(),
                drUser["CPF"].ToString(),
                drUser["Email"].ToString(),
                Convert.ToInt32(drUser["AccessLevelId"]));
        }
    }
}
