using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : ICRUDRepo<User>
    {

        public UserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'VaskEnTidDB' not found.");
        }

        public Task CreatAsyncc(User toBeCreatedT)
        {
            throw new NotImplementedException();
        }

        private readonly string _connectionString;

    

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

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
       
    }
}
