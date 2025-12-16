using System;
using System.Security.Cryptography;
using System.Text;

namespace SchedCCS
{
    /// <summary>
    /// Provides static utility methods for sensitive data operations, 
    /// primarily focusing on SHA-256 password hashing and verification.
    /// </summary>
    public static class SecurityHelper
    {
        #region 1. Hashing Logic

        /// <summary>
        /// Computes a SHA-256 hash from a plain text string.
        /// </summary>
        /// <param name="rawPassword">The plain text password to be hashed.</param>
        /// <returns>A hexadecimal string representation of the computed hash.</returns>
        public static string HashPassword(string rawPassword)
        {
            // Validate input to prevent null reference or empty string processing
            if (string.IsNullOrEmpty(rawPassword))
                return string.Empty;

            // Initialize the SHA-256 cryptographic provider
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert string input to a byte array and compute the hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));

                // Convert the resulting byte array into a hexadecimal string
                // Standard SHA-256 hex output is 64 characters
                StringBuilder builder = new StringBuilder(64);
                foreach (byte b in bytes)
                {
                    // "x2" format specifier ensures a two-digit lowercase hex representation
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion

        #region 2. Verification Logic

        /// <summary>
        /// Validates a plain text password against a previously stored SHA-256 hash.
        /// </summary>
        /// <param name="inputPassword">The plain text password provided during login.</param>
        /// <param name="storedHash">The hexadecimal hash retrieved from the database.</param>
        /// <returns>True if the hash of the input matches the stored hash; otherwise, false.</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Early return if either parameter is null or empty
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHash))
                return false;

            // Hash the user's input using the same algorithm
            string hashOfInput = HashPassword(inputPassword);

            // Compare generated hash against the database record
            // OrdinalIgnoreCase is used to maintain consistency across various database collations
            return string.Equals(hashOfInput, storedHash, StringComparison.OrdinalIgnoreCase);
        }

        #endregion
    }
}