using CoffeeCrazy.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeCrazy.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        /// <summary>
        /// Generates a 6-digit number token.
        /// </summary>
        /// <returns>A string representing a 6 digit number token.</returns>
        public string GenerateToken()
        {
            StringBuilder token = new StringBuilder();
            int nr;

            for (int i = 0; i < 6; i++)
            {
                nr = RandomNumberGenerator.GetInt32(9);
                token.Append(nr.ToString());
            }

            return token.ToString();
        }
    }
}
