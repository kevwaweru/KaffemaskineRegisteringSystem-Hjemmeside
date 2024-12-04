namespace CoffeeCrazy.Utilities
{
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordHelper
    {
        /// <summary>
        /// this Creates Password Hash it used an Encoding system.
        /// </summary>
        /// <param name="password">Entered Password</param>
        /// <returns>A salt key and Hashed Passwrod</returns>
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
        /// Validates Password against excisting password with hash and salt
        /// </summary>
        /// <param name="password">Entered Password</param>
        /// <param name="storedHash">Stored HashPassword</param>
        /// <param name="storedSalt">Stored SaltPassword</param>
        /// <returns></returns>
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
