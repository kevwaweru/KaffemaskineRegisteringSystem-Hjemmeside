using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;

        public UserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Users (FirstName, LastName, Email, Password, RoleId, CampusId) VALUES (@FirstName, @LastName, @Email, @Password, @RoleId, @CampusId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);    
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleId", (int)user.Role);
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

        public Task DeleteAsync(User toBeDeletedT)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
    }
}
