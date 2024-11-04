using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Globals
{
    public class Queries
    {
        private static string AzureConnectionString = ConfigurationManager.ConnectionStrings["AzureConnectionString"].ConnectionString;
        public static string classReturnMessage { get; private set; }

        public static async Task<bool> ConnectToDatabaseAsync()
        {
            classReturnMessage = "OK";

            using (SqlConnection connection = new SqlConnection(AzureConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    return true; 
                }
                catch (Exception ex)
                {
                    classReturnMessage = "An error occurred while trying to access the database. Details: \n" + ex.Message;
                    return false; 
                }
            }
        }

        public static async Task<bool> PingDatabaseAsync()
        {
            using (SqlConnection connection = new SqlConnection(AzureConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT 1";
                        await cmd.ExecuteNonQueryAsync();
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static int ExecuteProcedure(string procedure, SqlParameterCollection lstParametros, SqlParameter idOutput = null, int timeout = 30)
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

                        cmd.CommandTimeout = timeout;
                        id = cmd.ExecuteNonQuery();

                        if (idOutput != null)
                            id = Convert.ToInt32(idOutput.Value);
                    }
                }
                catch (Exception ex)
                {
                    classReturnMessage = "An error occurred while trying to access the database. Details: \n" + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return id;
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
                    classReturnMessage = "An error occurred while trying to acess the database. Details: \n" + ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return dataTable;
        }        
    }
}
