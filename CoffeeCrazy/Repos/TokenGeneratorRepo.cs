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
                string token = _tokenService.GenerateToken();

                using (var connection = new SqlConnection(_connectionString))
                {

                    string SQLquery = @"
                                    INSERT INTO PasswordResetTokens (Email, Token, ExpireDate)
                                    VALUES (@Email, @Token, @ExpireDate)";

                    var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@ExpireDate", DateTime.UtcNow.AddMinutes(30));

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

        public async Task<string> GetTokenAsync(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = "SELECT Token FROM PasswordResetTokens WHERE Email = @Email AND ExpireDate > @CurrentDate";

                    var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.UtcNow);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (string)reader["Token"];
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

        public async Task<bool> ValidateTokenAsync( string token)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"
                SELECT COUNT(1)  
                FROM PasswordResetTokens 
                WHERE Token = @Token";

                    var command = new SqlCommand(SQLquery, connection);             
                    command.Parameters.AddWithValue("@Token", token);
                   

                    await connection.OpenAsync();

                    var result = (int)await command.ExecuteScalarAsync();
                
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}

