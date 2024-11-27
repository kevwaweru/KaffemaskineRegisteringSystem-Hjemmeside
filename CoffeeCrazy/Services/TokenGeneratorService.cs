using CoffeeCrazy.Interfaces;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CoffeeCrazy.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public int GenerateToken()
        {
            int generatedToken = RandomNumberGenerator.GetInt32(20);

            return generatedToken;
        }
    }
}
