using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Services;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class TokenGeneratorRepo 
    {
        private readonly string _connectionString;
        private readonly ITokenGeneratorService _ITokenGeneratorService;
        public TokenGeneratorRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }
   

        /// <summary>
        /// This creates a token. with an expiredate and connect with User Email.
        /// </summary>
        /// <param name="email">Users Email</param>
        /// <returns>Dunno?</returns>
        public async Task Create(string email)
        {
            
            try
            {
                int token = _ITokenGeneratorService.GenerateToken();

                using (var connection = new SqlConnection(_connectionString))
                {
                    
                    string SQLquery = @"
                                    INSERT INTO PasswordResetTokens (Email, Token, ExpireDate)
                                    VALUES (@Email, @Token, @ExpireDate)";

                    var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("Email", email);
                    command.Parameters.AddWithValue("Token", token);
                    command.Parameters.AddWithValue("ExpireDate", DateTime.Now.AddMinutes(30));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
                throw;
            }
        }
        


    }
}
