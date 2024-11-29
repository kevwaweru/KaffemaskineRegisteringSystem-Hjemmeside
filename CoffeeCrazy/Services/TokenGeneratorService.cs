using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Repos;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeCrazy.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public TokenGeneratorService()
        {

        }
        public string GenerateToken()
        {
            StringBuilder token = new StringBuilder();
            int nr;
            for (int i = 0; i < 6; i++)
            {
                nr = RandomNumberGenerator.GetInt32(9);
                token.Append(Convert.ToString(nr));
            }
            return token.ToString();
        }
    }
}
