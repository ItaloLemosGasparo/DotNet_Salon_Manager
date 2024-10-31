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

        public static int ExecuteProcedure(string procedure, SqlParameterCollection lstParametros, SqlParameter idOutput = null, Ambiente ambiente = Ambiente.Producao)
        {
            int id = 0;
            MsgRetornoClasse = "OK";
        
            SegCriptografia crypto = new SegCriptografia(SegCriptografia.CryptoTypes.encTypeDES);
            string cnx = crypto.Decrypt(CieloDBConnection.strCnxDBCieloSolucion(ambiente), "Key_str.Cnx.DB.CieloSolucion");
        
            using (SqlConnection connection = new SqlConnection(cnx))
            {
                try
                {
                    connection.Open();
        
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
        
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = procedure;
        
                        cmd.Parameters.Clear();
        
                        foreach (var item in lstParametros.Cast<SqlParameter>().ToList())
                        {
                            cmd.Parameters.Add(item.ParameterName, item.SqlDbType).Value = item.Value;
                        }
        
                        if (idOutput != null)
                            cmd.Parameters.Add(idOutput);
        
                        cmd.CommandTimeout = 1000;
                        id = cmd.ExecuteNonQuery();
        
                        if (idOutput != null)
                            id = Convert.ToInt32(idOutput.Value);
                    }
                }
                catch (Exception ex)
                {
                    MsgRetornoClasse = "Erro ao acessar a base de dados. Detalhe do Erro: \n" + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }
        
            return id;
        }
    }    
}
