using System;
using System.Data.SqlClient;
using System.Data;
using Globals;
using System.Windows.Forms;

namespace DotNet_Salon_Manager.Classes
{
    internal class User
    {
        public static void LoadUserProfile(DataRow drUser)
        {
            UserProfile.StartProfile(
                Convert.ToInt32(drUser["UserId"]),
                drUser["Name"].ToString(),
                drUser["CPF"].ToString(),
                drUser["Email"].ToString(),
                Convert.ToInt32(drUser["AccessLevelId"])
            );
        }

        public static bool HasAccess(int accessLevelNeeded, string frm)
        {
            if (UserProfile.AccessLevelId <= accessLevelNeeded)
                return true;
            else
            {
                MessageBox.Show($"You don't have access to \"{frm}\".", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public static DataRow FindUserByEmail(string email)
        {
            SqlParameterCollection sqlParameter = new SqlCommand().Parameters;
            sqlParameter.Add("@Email", SqlDbType.VarChar).Value = email;

            DataTable dataTable = Queries.LoadDataTable("SP_FIND_USER_BY_EMAIL", sqlParameter);

            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }

        public static void RegisterUser(string name, string cpf, string email, int accessLevel, string password)
        {
            var (passwordHash, passwordSalt) = Password.HashPassword(password);

            string stringHash = Convert.ToBase64String(passwordHash);
            string stringSalt = Convert.ToBase64String(passwordSalt);

            SqlParameterCollection sqlParameter = new SqlCommand().Parameters;
            sqlParameter.Add("@Name", SqlDbType.VarChar).Value = name;
            sqlParameter.Add("@CPF", SqlDbType.VarChar).Value = cpf;
            sqlParameter.Add("@Email", SqlDbType.VarChar).Value = email;
            sqlParameter.Add("@AccessLevel", SqlDbType.Int).Value = accessLevel;
            sqlParameter.Add("@PasswordHash", SqlDbType.VarChar).Value = stringHash;
            sqlParameter.Add("@PasswordSalt", SqlDbType.VarChar).Value = stringSalt;

            Queries.ExecuteProcedure("SP_REGISTER_USER", sqlParameter);
        }
    }
}
