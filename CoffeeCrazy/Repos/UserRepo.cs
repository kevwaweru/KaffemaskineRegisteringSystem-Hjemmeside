﻿using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

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
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "EXEC AdminCreateEmployee @FirstName, @LastName, @Email, @Password, @CampusId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@CampusId", (int)user.Campus);

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


        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
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

    }
}