using System;
using System.Windows.Forms;
using DotNet_Salon_Manager.Classes;
using DotNet_Salon_Manager.modules.Appointments;
using DotNet_Salon_Manager.modules.Clients;
using DotNet_Salon_Manager.modules.Employees;
using DotNet_Salon_Manager.modules.Products;
using Globals;


namespace DotNet_Salon_Manager
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            InitializeConnectionTimer();
        }

        private void InitializeConnectionTimer()
        {
            connectionTimer = new Timer();
            connectionTimer.Tick += ConnectionTimer_Tick;
            connectionTimer.Start();
        }

        private async void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            bool connectionAlive = await Queries.PingDatabaseAsync();
            if (!connectionAlive)
            {
                MessageBox.Show("Connection to the database has been lost. The application will now close.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void openForm(Form frm)
        {
            Form openForm = Application.OpenForms[frm.Name];
            if (openForm != null)
            {
                openForm.BringToFront();
                lblForm.Text = openForm.Text;
                return;
            }

            lblForm.Text = frm.Text;
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void newClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmNewClient());
        }

        private void clientsManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmClientsManagement());
        }

        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmNewProduct());
        }

        private void productsManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmProductManagement());
        }

        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(4, $"{sender.ToString()}"))
                openForm(new frmSchedules());
        }

        private void newEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmNewEmployee());
        }

        private void employeesManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(3, $"{sender.ToString()}"))
                openForm(new frmEmployeeManagement());
        }
    }
}
