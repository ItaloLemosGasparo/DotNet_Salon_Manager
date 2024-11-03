using System.Text.RegularExpressions;

namespace Globals
{
    public class Global
    {
        public static string ClassReturnMessage { get; set; }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
