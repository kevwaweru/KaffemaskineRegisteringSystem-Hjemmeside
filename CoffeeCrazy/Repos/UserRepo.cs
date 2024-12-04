using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Utilities;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ITokenRepo _tokenGeneratorRepo;
        private readonly string _connectionString;

        public UserRepo(IConfiguration configuration, ITokenRepo tokenGeneratorRepo)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tokenGeneratorRepo = tokenGeneratorRepo;
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(user.Password);
                user.Password = Convert.ToBase64String(passwordHash);
                user.PasswordSalt = Convert.ToBase64String(passwordSalt);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO Users (FirstName, LastName, Email, Password, PasswordSalt, CampusId, RoleId)
                                    VALUES (@FirstName, @LastName, @Email, @Password, @PasswordSalt, @CampusId, @RoleId)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@CampusId", (int)user.Campus);
                    command.Parameters.AddWithValue("@RoleId", (int)user.Role);

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

        public async Task<List<User>> GetAllAsync()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Users";

                    SqlCommand command = new SqlCommand(query, connection);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User user = new User
                            {
                                UserId = (int)reader["UserId"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["Password"],
                                PasswordSalt = (string)reader["PasswordSalt"],
                                Role = (Role)reader["RoleId"],
                                Campus = (Campus)reader["CampusId"]
                            };

                            users.Add(user);
                        }
                    }
                }
                return users;
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
        public async Task<User> GetByIdAsync(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * WHERE UserId = @UserId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = (int)reader["UserId"],
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["Password"],
                                PasswordSalt = (string)reader["PasswordSalt"],
                                Role = (Role)reader["RoleId"],
                                Campus = (Campus)reader["CampusId"]
                            };
                        }
                        else
                        {
                            throw new Exception($"User with ID {userId} does not exist.");
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

        public async Task UpdateAsync(User toBeUpdatedUser)
        {
            try
            {
                var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(toBeUpdatedUser.Password);
                toBeUpdatedUser.Password = Convert.ToBase64String(passwordHash);
                toBeUpdatedUser.PasswordSalt = Convert.ToBase64String(passwordSalt);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"UPDATE Users SET
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        Email = @Email,
                                        Password = @Password,
                                        PasswordSalt = @PasswordSalt,
                                        CampusId = @CampusId,
                                        RoleId = @RoleId
                                    WHERE
                                        JobId = @JobId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", toBeUpdatedUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", toBeUpdatedUser.LastName);
                    command.Parameters.AddWithValue("@Email", toBeUpdatedUser.Email);
                    command.Parameters.AddWithValue("@Password", toBeUpdatedUser.Password);
                    command.Parameters.AddWithValue("@PasswordSalt", toBeUpdatedUser.PasswordSalt);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeUpdatedUser.Campus);
                    command.Parameters.AddWithValue("@RoleId", (int)toBeUpdatedUser.Role);
                    command.Parameters.AddWithValue("@UserId", toBeUpdatedUser.UserId);

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

        public async Task DeleteAsync(User toBeDeletedUser)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Users WHERE UserId = @UserId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@UserId", toBeDeletedUser.UserId);

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



        // -- Jeg kunne godt tænke mig vi flytte disse til et PasswordRepo --
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

                    string updateQuery = @"UPDATE Users SET Password = @NewPasswordHash, PasswordSalt = @NewPasswordSalt
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

                    // Slet token fra databasen
                    // skal bare implamenteres

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
        /// A
        /// </summary>
        /// <param name="token"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private async Task UpdateUserPasswordAsync(string email, byte[] hash, byte[] salt, SqlConnection connection)
        {
            try
            {
                string query = @"UPDATE Users SET Password = @Password, PasswordSalt = @PasswordSalt
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

        public async Task<bool> DeleteUserAsync(int userId, int currentUserId)
        {
            try
            {
                if (userId == currentUserId)
                {
                    return false;
                }


                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Users WHERE UserId = @UserId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();
                    int affectedRows = await command.ExecuteNonQueryAsync();

                    // Check if any rows were deleted
                    return affectedRows > 0;
                }
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
    }
}