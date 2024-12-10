﻿using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;
using CoffeeCrazy.Utilities;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;
        private readonly IImageService _imageService;

        public UserRepo(IConfiguration configuration, IImageService imageService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _imageService = imageService;
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="user"></param>
        /// <returns> Nil </returns>
        public async Task CreateAsync(User user)
        {
            try
            {
                var (passwordHash, passwordSalt) = PasswordHelper.CreatePasswordHash(user.Password);
                user.Password = Convert.ToBase64String(passwordHash);
                user.PasswordSalt = Convert.ToBase64String(passwordSalt);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO Users (FirstName, LastName, Email, Password, PasswordSalt, CampusId, RoleId, UserImage)
                                    VALUES (@FirstName, @LastName, @Email, @Password, @PasswordSalt, @CampusId, @RoleId, @UserImage)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email.ToLower());
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@CampusId", (int)user.Campus);
                    command.Parameters.AddWithValue("@RoleId", (int)user.Role);
                    command.Parameters.AddWithValue("@UserImage", user.UserImage != null ? _imageService.ConvertImageToByteArray(user.UserImage) : DBNull.Value);


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

        /// <summary>
        /// Read/Get all Users
        /// </summary>
        /// <returns>A list of Users</returns>
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
                                Role = (Role)reader["RoleId"],
                                Campus = (Campus)reader["CampusId"],
                                UserImage = reader["UserImage"] != DBNull.Value ? _imageService.ConvertArrayToIFormFile((byte[])reader["UserImage"]) : null
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

        /// <summary>
        /// Get/Read a user by its UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetByIdAsync(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Users WHERE UserId = @UserId";

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
                                Role = (Role)reader["RoleId"],
                                Campus = (Campus)reader["CampusId"],
                                UserImage = reader["UserImage"] != DBNull.Value ? _imageService.ConvertArrayToIFormFile((byte[])reader["UserImage"]) : null
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
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"UPDATE Users SET
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        Email = @Email,
                                        CampusId = @CampusId,
                                        RoleId = @RoleId,
                                        UserImage = @UserImage
                                    WHERE
                                        UserId = @UserId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", toBeUpdatedUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", toBeUpdatedUser.LastName);
                    command.Parameters.AddWithValue("@Email", toBeUpdatedUser.Email);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeUpdatedUser.Campus);
                    command.Parameters.AddWithValue("@RoleId", (int)toBeUpdatedUser.Role);
                    command.Parameters.AddWithValue("@UserId", toBeUpdatedUser.UserId);
                    command.Parameters.AddWithValue("@UserImage", toBeUpdatedUser.UserImage != null ? _imageService.ConvertImageToByteArray(toBeUpdatedUser.UserImage) : DBNull.Value);

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
        // der er 2 delete metoder??
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