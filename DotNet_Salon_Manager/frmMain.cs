using DotNet_Salon_Manager.modules;
using DotNet_Salon_Manager.modules.Appointments;
using DotNet_Salon_Manager.modules.Classes;
using DotNet_Salon_Manager.modules.Clients;
using DotNet_Salon_Manager.modules.Employees;
using DotNet_Salon_Manager.modules.Products;
using System;
using System.Windows.Forms;

namespace DotNet_Salon_Manager
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openForm(Form frm)
        {
            lblForm.Text = frm.Name;
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
                openForm(new frmNewProduct());
        }

        private void appointmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.HasAccess(4, $"{sender.ToString()}"))
                openForm(new frmAppoitments());
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
