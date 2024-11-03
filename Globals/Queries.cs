using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Classes;

namespace Globals
{
    public class Queries
    {
        private static string AzureConnectionString = ConfigurationManager.ConnectionStrings["AzureConnectionString"].ConnectionString;
        public static string classReturnMessage { get; set; }

        public static int ExecuteProcedure(string procedure, SqlParameterCollection lstParametros, SqlParameter idOutput = null)
        {
            int id = 0;
            classReturnMessage = "OK";

            using (SqlConnection connection = new SqlConnection(AzureConnectionString))
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
                    classReturnMessage = "Erro ao acessar a base de dados. Detalhe do Erro: \n" + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return id;
        }

        public static DataRow FindUserByEmail(string Email)
        {
            SqlParameterCollection sqlParameter = new SqlCommand().Parameters;
            sqlParameter.Add("@Email", SqlDbType.VarChar).Value = Email;

            DataTable dataTable = LoadDataTable("SP_FIND_USER_BY_EMAIL", sqlParameter);

            return dataTable.Rows.Count > 0? dataTable.Rows[0] : null;
        }

        public static DataTable LoadDataTable (string procedure, SqlParameterCollection lstParametros = null, int timeout = 30)
        {
            classReturnMessage = "OK";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(AzureConnectionString))
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

                        if (lstParametros != null)
                        {
                            foreach (var item in lstParametros.Cast<SqlParameter>().ToList())
                            {
                                cmd.Parameters.Add(item.ParameterName, item.SqlDbType).Value = item.Value;
                            }
                        }

                        cmd.CommandTimeout = timeout;

                        adapter.SelectCommand = cmd;
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    classReturnMessage = "Erro ao acessar a base de dados. Detalhe do Erro: \n" + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return dataTable;
        }

        public static void RegisterUser(string name, string cpf, string email, int accessLevel, string password)
        {
            var (passwordHash, passwordSalt) = PasswordHasher.HashPassword(password);

            // Converter o hash e o salt para strings base64
            string stringHash = Convert.ToBase64String(passwordHash);
            string stringSalt = Convert.ToBase64String(passwordSalt);

            SqlParameterCollection sqlParameter = new SqlCommand().Parameters;
            sqlParameter.Add("@Name", SqlDbType.VarChar).Value = name;
            sqlParameter.Add("@CPF", SqlDbType.VarChar).Value = cpf;
            sqlParameter.Add("@Email", SqlDbType.VarChar).Value = email;
            sqlParameter.Add("@AccessLevel", SqlDbType.Int).Value = accessLevel;
            sqlParameter.Add("@PasswordHash", SqlDbType.VarChar).Value = stringHash;
            sqlParameter.Add("@PasswordSalt", SqlDbType.VarChar).Value = stringSalt;

            ExecuteProcedure("SP_REGISTER_USER", sqlParameter);
        }
    }
}
