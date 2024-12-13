namespace CoffeeCrazy.Utilities
{
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordHelper
    {

        /// <summary>
        /// Creates a hashed password and a corresponding salt using HMACSHA256.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item><description><c>hash</c>: The computed hash of the password.</description></item>
        /// <item><description><c>salt</c>: The generated salt used for hashing.</description></item>
        /// </list>
        /// </returns>
        public static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                byte[] salt = hmac.Key;
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (hash, salt);
            }
        }

        /// <summary>
        /// Validates a password by comparing it against a stored hash and salt.
        /// </summary>
        /// <param name="password">The plaintext password entered by the user.</param>
        /// <param name="storedHash">The previously stored hash of the password.</param>
        /// <param name="storedSalt">The previously stored salt used to compute the hash.</param>
        /// <returns>
        /// True if the computed hash matches the stored hash; otherwise, false.
        /// </returns>
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA256(storedSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash); 
            }
        }
    }
}
