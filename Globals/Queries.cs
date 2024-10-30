using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Globals
{
    public class Queries
    {
        public static void TestarConecao()
        {
            string AzureConnectionString = ConfigurationManager.ConnectionStrings["AzureConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(AzureConnectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Conexão bem-sucedida!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar: " + ex.Message);
                }
            }
        }
    }
}
