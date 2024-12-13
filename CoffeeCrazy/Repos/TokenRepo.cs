using CoffeeCrazy.Interfaces;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    /// <summary>
    /// Repository for handling token-related operations in the database.
    /// </summary>
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
        /// Creates a new token, associates it with a user's email, and sets an expiration date.
        /// </summary>
        /// <param name="email">The email address to associate the token with.</param>
        /// <exception cref="SqlException">Thrown if a database-related error accurs.</exception>
        public async Task CreateTokenAsync(string email)
        {
            try
            {
                string token = _tokenService.GenerateToken();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"INSERT INTO PasswordResetTokens (Token, ExpireDate, Email)
                                    VALUES (@Token, @ExpireDate, @Email)";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
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
        /// Retrieves the token associated with a given email, provided the token has not expired.
        /// </summary>
        /// <param name="email">The email address to look up the token for.</param>
        /// <returns>The token associated with the email.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no token is found for the email.</exception>
        /// <exception cref="SqlException">Thrown if a database-related error accurs.</exception>
        public async Task<string> GetTokenAsync(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"SELECT Token FROM PasswordResetTokens
                                    WHERE Email = @Email
                                    AND ExpireDate > @CurrentDate";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
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
        /// Validates if a given token exists in the database.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <returns>True if the token exists. otherwise false.</returns>
        /// <exception cref="SqlException">Thrown if a database-related error occurs.</exception>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"SELECT COUNT(1)
                                    FROM PasswordResetTokens 
                                    WHERE Token = @Token";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@Token", token);

                    await connection.OpenAsync();

                    int result = (int)await command.ExecuteScalarAsync();
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
        /// Deletes a specific token from the database.
        /// </summary>
        /// <param name="token">The token to delete.</param>
        /// <exception cref="SqlException">Thrown if a database-related error accurs.</exception>
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
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }
    }
}