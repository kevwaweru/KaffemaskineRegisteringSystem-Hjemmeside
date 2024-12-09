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
        /// Creates a token. with an expiredate and connect with User Email.
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
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// This search the DB where Email and token is hand in hand
        /// </summary>
        /// <param name="email">Users Email</param>
        /// <returns>The Token linked to the Email</returns>
        /// <exception cref="InvalidOperationException"></exception>
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
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Check the the token that is intered in HTML is in the DB.
        /// </summary>
        /// <param name="token">The Token the user get by mail</param>
        /// <returns>True if token match false if dont</returns>
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
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Deletes the token.
        /// </summary>
        /// <param name="token">A token</param>
        /// <returns>Query thats delete the token that match the intered token.</returns>
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