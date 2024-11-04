using System;
using System.Security.Cryptography;
using System.Text;

namespace Globals
{
    public class Password
    {
        public static (byte[] Hash, byte[] Salt) HashPassword(string password)
        {
            byte[] salt = new byte[32]; 
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = GenerateHash(password, salt);
            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] originaloriginalHash = Convert.FromBase64String(storedHash);
            byte[] originalstoredSalt = Convert.FromBase64String(storedSalt);

            byte[] hash = GenerateHash(password, originalstoredSalt);

            return CompareHashes(originaloriginalHash, hash);
        }

        private static byte[] GenerateHash(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];

                Buffer.BlockCopy(passwordBytes, 0, passwordWithSalt, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, passwordWithSalt, passwordBytes.Length, salt.Length);

                return sha256.ComputeHash(passwordWithSalt);
            }
        }

        private static bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }
    }
}
