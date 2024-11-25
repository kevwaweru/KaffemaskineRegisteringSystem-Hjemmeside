using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Utilitys;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;
        public UserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe maskine database' not found.");
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
                    string query = @"
                INSERT INTO Users (FirstName, LastName, Email, Password, PasswordSalt, CampusId, RoleId)
                VALUES (@FirstName, @LastName, @Email, @Password, @PasswordSalt, @CampusId, @RoleId)"; ;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
        }

        public async Task UpdateAsync(User toBeUpdatedT)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "EXEC AdminUpdateEmployee @UserId, @FirstName, @LastName, @Email, @Password, @CampusId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", (int)toBeUpdatedT.UserId);
                    command.Parameters.AddWithValue("@FirstName", toBeUpdatedT.FirstName);
                    command.Parameters.AddWithValue("@LastName", toBeUpdatedT.LastName);
                    command.Parameters.AddWithValue("@Email", toBeUpdatedT.Email);
                    command.Parameters.AddWithValue("@Password", toBeUpdatedT.Password);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeUpdatedT.Campus);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error:" + sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex);
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
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT UserId, FirstName, LastName, Email, Password, RoleId, CampusId FROM Users";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Password = reader.GetString(4),
                                    Role = (Role)reader.GetInt32(5),
                                    Campus = (Campus)reader.GetInt32(6)
                                };

                                users.Add(user);
                            }
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
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT UserId, FirstName, LastName, Email, Password, RoleId, CampusId FROM Users";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserId = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Password = reader.GetString(4),
                                    Role = (Role)reader.GetInt32(5),
                                    Campus = (Campus)reader.GetInt32(6)
                                };
                            }
                            else
                            {
                                throw new Exception($"User with ID {userId} does not exist.");
                            }
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
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(byte[] passwordHash, byte[] passwordSalt, Role role)> GetUserByEmailAsync(string email)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                SELECT Password, PasswordSalt, RoleId 
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
                                return (passwordHash, passwordSalt, role);
                            }
                        }
                    }

                    throw new Exception("User not found");
                }
            }
            catch (SqlException sqlEx)
            {

                Console.Error.WriteLine($"SQL Error: {sqlEx.Message}");
                throw new Exception("A database error occurred. Please try again later.", sqlEx);
            }
            catch (Exception ex)
            {

                Console.Error.WriteLine($"Error: {ex.Message}");
                throw new Exception("An error occurred while retrieving the user.", ex);
            }

        }

        public async Task ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Hent den aktuelle adgangskode og salt
                    string selectQuery = @"
                SELECT PasswordHash, PasswordSalt 
                FROM Users 
                WHERE Email = @Email";

                    using (var selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Email", email);

                        using (var reader = await selectCommand.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var storedHash = Convert.FromBase64String((string)reader["PasswordHash"]);
                                var storedSalt = Convert.FromBase64String((string)reader["PasswordSalt"]);

                                // Validér den aktuelle adgangskode
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


                    string updateQuery = @"
                UPDATE Users 
                SET PasswordHash = @NewPasswordHash, PasswordSalt = @NewPasswordSalt
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}