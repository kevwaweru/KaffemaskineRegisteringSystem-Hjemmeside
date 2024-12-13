using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Utilities;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class PasswordRepo : IPasswordRepo
    {
        private readonly ITokenRepo _tokenGeneratorRepo;
        private readonly string _connectionString;

        public PasswordRepo(IConfiguration configuration, ITokenRepo tokenGeneratorRepo)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tokenGeneratorRepo = tokenGeneratorRepo;
        }

        /// <summary>
        /// Retrieves a user's hashed password, salt, role, first name, and user ID by their email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>
        /// A tuple containing the password hash, password salt, role, first name, and user ID.
        /// Throws an exception if the user is not found.
        /// </returns>
        public async Task<(byte[] passwordHash, byte[] passwordSalt, Role role, string firstName, int userId)> GetUserByEmailAsync(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT Password, PasswordSalt, RoleId, FirstName, UserId
                                 FROM Users 
                                 WHERE Email = @Email";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string passwordBase64 = reader["Password"].ToString();
                                string saltBase64 = reader["PasswordSalt"].ToString();

                                byte[] passwordHash = Convert.FromBase64String(passwordBase64);
                                byte[] passwordSalt = Convert.FromBase64String(saltBase64);

                                Role role = (Role)reader["RoleId"];
                                string firstName = reader["FirstName"].ToString();
                                int userId = (int)reader["UserId"];

                                return (passwordHash, passwordSalt, role, firstName, userId);
                            }
                        }
                    }
                    throw new Exception("User not found");
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
        /// Changes the user's password by validating the current password and updating it to a new password.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to set.</param>
        public async Task ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string selectQuery = @"SELECT Password, PasswordSalt 
                                       FROM Users 
                                       WHERE Email = @Email";

                    using (var selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Email", email);

                        using (var reader = await selectCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var storedHash = Convert.FromBase64String((string)reader["Password"]);
                                var storedSalt = Convert.FromBase64String((string)reader["PasswordSalt"]);

                                if (!PasswordHelper.VerifyPasswordHash(currentPassword, storedHash, storedSalt))
                                {
                                    throw new Exception("Current password is incorrect.");
                                }
                            }
                            else
                            {
                                throw new Exception("User not found.");
                            }
                        }
                    }

                    var (newHash, newSalt) = PasswordHelper.CreatePasswordHash(newPassword);

                    string updateQuery = @"UPDATE Users 
                                       SET Password = @NewPasswordHash, PasswordSalt = @NewPasswordSalt
                                       WHERE Email = @Email";

                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Email", email);
                        updateCommand.Parameters.AddWithValue("@NewPasswordHash", Convert.ToBase64String(newHash));
                        updateCommand.Parameters.AddWithValue("@NewPasswordSalt", Convert.ToBase64String(newSalt));

                        await updateCommand.ExecuteNonQueryAsync();
                    }
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
        /// Resets the user's password using a token and a new password.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password reset is successful, false otherwise.</returns>
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string email = await GetEmailByTokenAsync(token, connection);
                    if (email == null)
                    {
                        return false;
                    }

                    var (hash, salt) = PasswordHelper.CreatePasswordHash(newPassword);

                    await UpdateUserPasswordAsync(email, hash, salt, connection);

                    await _tokenGeneratorRepo.DeleteTokenAsync(token);
                }

                return true;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error: " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates if an email exists and deletes associated data if it does.
        /// </summary>
        /// <param name="email">The email to validate and delete.</param>
        /// <returns>True if email existed and data was deleted, false otherwise.</returns>
        public async Task<bool> ValidateAndDeleteEmail(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    bool emailExists = await ValidateEmailAsync(email, connection);
                    if (!emailExists)
                    {
                        return false;
                    }

                    await DeleteEmailAsync(email, connection);
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error: " + sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates if an email exists in the PasswordResetTokens table.
        /// </summary>
        private async Task<bool> ValidateEmailAsync(string email, SqlConnection connection)
        {
            string query = @"SELECT COUNT(1) 
                         FROM PasswordResetTokens 
                         WHERE Email = @Email";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);

                int emailCount = (int)await command.ExecuteScalarAsync();
                return emailCount > 0;
            }
        }

        /// <summary>
        /// Deletes all entries for a given email in the PasswordResetTokens table.
        /// </summary>
        private async Task DeleteEmailAsync(string email, SqlConnection connection)
        {
            string query = @"DELETE 
                         FROM PasswordResetTokens 
                         WHERE Email = @Email";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Retrieves the email associated with a given token.
        /// </summary>
        private async Task<string?> GetEmailByTokenAsync(string token, SqlConnection connection)
        {
            try
            {
                string query = @"SELECT Email
                             FROM PasswordResetTokens
                             WHERE Token = @Token";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", token);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader["Email"].ToString();
                        }
                    }
                }
                return null;
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
        /// Updates the password and salt for a given email in the Users table.
        /// </summary>
        private async Task UpdateUserPasswordAsync(string email, byte[] hash, byte[] salt, SqlConnection connection)
        {
            try
            {
                string query = @"UPDATE Users 
                             SET Password = @Password, 
                                 PasswordSalt = @PasswordSalt
                             WHERE Email = @Email";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Password", Convert.ToBase64String(hash));
                    command.Parameters.AddWithValue("@PasswordSalt", Convert.ToBase64String(salt));
                    command.Parameters.AddWithValue("@Email", email);

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
