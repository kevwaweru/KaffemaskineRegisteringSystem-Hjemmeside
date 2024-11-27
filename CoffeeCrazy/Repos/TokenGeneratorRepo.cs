using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Services;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class TokenGeneratorRepo : ITokenGeneratorRepo
    {
        private readonly string _connectionString;
        private readonly ITokenGeneratorService _tokenService;
        public TokenGeneratorRepo(IConfiguration configuration, ITokenGeneratorService tokenService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");

            _tokenService = tokenService;
        }


        /// <summary>
        /// This creates a token. with an expiredate and connect with User Email.
        /// </summary>
        /// <param name="email">Users Email</param>
        /// <returns>Dunno?</returns>
        public async Task CreateAsync(string email)
        {

            try
            {
                int token = Convert.ToInt32(_tokenService.GenerateToken());

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

        public async Task<int?> GetTokenAsync(string email)
        {
            int token;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = "SELECT Token FROM PasswordResetTokens WHERE Email = @Email AND ExpireDate > @CurrentDate";

                    var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("Email", email);
                    command.Parameters.AddWithValue("CurrentDate", DateTime.Now);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return token = (int)reader["Token"];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            return null;
        }

    }
}
