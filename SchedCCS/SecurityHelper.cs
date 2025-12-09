using System;
using System.Security.Cryptography;
using System.Text;

namespace SchedCCS
{
    public static class SecurityHelper
    {
        #region 1. Hashing Logic

        // Computes SHA256 hash of a plain text string
        public static string HashPassword(string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword)) return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        #endregion

        #region 2. Verification Logic

        // Compares a plain text input against a stored hash
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            string hashOfInput = HashPassword(inputPassword);
            return hashOfInput == storedHash;
        }

        #endregion
    }
}