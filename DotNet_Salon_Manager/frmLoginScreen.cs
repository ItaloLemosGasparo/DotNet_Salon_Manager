using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
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
            frmMain frmMain = new frmMain();    
            frmMain.ShowDialog();
        }

        private void LoginScreen_Load(object sender, System.EventArgs e)
        {
            Queries.TestarConecao();
        }

        public static (string Hash, string Salt) HashPassword(string password)
        {
            // Gerar um salt aleatório
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Concatenar salt à senha e hash
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = password + salt;
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            string saltedPassword = password + storedSalt;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string hash = Convert.ToBase64String(hashBytes);
                return hash == storedHash;
            }
        }
    }
}
