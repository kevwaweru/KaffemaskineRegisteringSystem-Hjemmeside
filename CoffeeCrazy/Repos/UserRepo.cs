using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace CoffeeCrazy.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionstring;

        public UserRepo(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("DefaultConnection");
        }
        public Task CreateAsync(User toBeCreatedT)
        {
            throw new NotImplementedException();
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

        public async Task UpdateAsync(User toBeUpdatedT)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionstring))
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
    }
}
