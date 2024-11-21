using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : ICRUDRepo<User>
    {
        private readonly string _connectionString;
        public UserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'VaskEnTidDB' not found.");
        }
        public Task CreatAsyncc(User toBeCreatedT)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User toBeDeletedT)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT UserId, FirstName, LastName, Password, Role, Campus FROM Users";

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
                                    Passowrd = reader.GetString(3),
                                    Role = Enum.Parse<Role>(reader.GetString(4)),
                                    Campus = Enum.Parse<Campus>(reader.GetString(5))
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
                    string query = "SELECT UserId, FirstName, LastName, Password, Role, Campus FROM Users WHERE UserId = @UserId";

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
                                    Passowrd = reader.GetString(3),
                                    Role = Enum.Parse<Role>(reader.GetString(4)),
                                    Campus = Enum.Parse<Campus>(reader.GetString(5))
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



        public Task UpdateAsync(User toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
    }
}
