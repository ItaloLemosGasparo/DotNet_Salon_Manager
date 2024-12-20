﻿using System;
using System.Data;
using System.Windows.Forms;
using DotNet_Salon_Manager.Classes;
using Globals;


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
            string email = txtEmail.Text.Trim();
            string password = txtPass.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both your email and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Global.IsValidEmail(email))
            {
                MessageBox.Show("Invalid Email format.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRow drUser = User.FindUserByEmail(email);

                if (drUser != null && Password.VerifyPassword(password, drUser["PasswordHash"].ToString(), drUser["PasswordSalt"].ToString()))
                {
                    User.LoadUserProfile(drUser);

                    using (frmMain frmMain = new frmMain())
                    {
                        this.Visible = false;
                        frmMain.ShowDialog();
                        Application.Exit();
                    }
                }
                else
                    MessageBox.Show("Incorrect Email or Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to log in. Details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void frmLoginScreen_Load(object sender, EventArgs e)
        {
            loadingImage.BringToFront();
            loadingImage.Visible = true;

            bool connectionSuccessful = await Queries.ConnectToDatabaseAsync();

            loadingImage.Visible = false;

            if (!connectionSuccessful)
            {
                MessageBox.Show(Queries.classReturnMessage);
                Application.Exit();
            }
            else
            {
                txtEmail.Enabled = true;
                txtEmail.Focus();
                txtPass.Enabled = true;
                btnLogin.Enabled = true;
                btnExit.Enabled = true;
                pbViewPassword.Enabled = true;
            }
        }

        private bool _viewPassword;
        private void pbViewPassword_Click(object sender, EventArgs e)
        {
            _viewPassword = !_viewPassword;
            pbViewPassword.Image = _viewPassword ? DotNet_Salon_Manager.Properties.Resources.UnViewPassword : DotNet_Salon_Manager.Properties.Resources.ViewPassword;
            txtPass.PasswordChar = _viewPassword? '\0' : '*';
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                
                btnLogin_Click(sender, e);
        }
    }
}
