using CoffeeCrazy.Interfaces;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class TokenRepo : ITokenRepo
    {
        private readonly ITokenGeneratorService _tokenService;
        private readonly string _connectionString;

        public TokenRepo(IConfiguration configuration, ITokenGeneratorService tokenService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tokenService = tokenService;
        }

        /// <summary>
        /// This creates a token. with an expiredate and connect with User Email.
        /// </summary>
        /// <param name="email">Users Email</param>
        /// <returns>Dunno?</returns>
        public async Task CreateTokenAsync(string email)
        {
            try
            {
                string token = _tokenService.GenerateToken();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"INSERT INTO PasswordResetTokens (Token, ExpireDate, Email)
                                        VALUES (@Token, @ExpireDate, @Email)";

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@ExpireDate", DateTime.UtcNow.AddMinutes(30));
                    command.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                // SQL Errors
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Other Errors
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetTokenAsync(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"SELECT Token FROM PasswordResetTokens
                                        WHERE Email = @Email
                                        AND ExpireDate > @CurrentDate";

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return (string)reader["Token"];
                        }
                        else
                        {
                            throw new InvalidOperationException($"No token generated for email: {email}.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log database-related errors.
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general errors.
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"SELECT COUNT(1)
                                        FROM PasswordResetTokens 
                                        WHERE Token = @Token";

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Token", token);

                    await connection.OpenAsync();

                    int result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                // SQL Errors
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Other Errors
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteTokenAsync(string token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM PasswordResetTokens WHERE Token = @Token";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@Token", token);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                // SQL Errors
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Other Errors
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }
    }
}