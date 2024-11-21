using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : ICRUDRepo<Machine>
    {

        private readonly string _connectionString;
        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'VaskEnTidDB' not found.");
        }


        public async Task DeleteAsync(Machine toBeDeletedMachine)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Machines WHERE MachineId = @MachineId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@MachineId", toBeDeletedMachine.MachineId);

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
        public Task CreatAsyncc(Machine toBeCreatedT)
        {
            throw new NotImplementedException();
        }

    
        public Task<List<Machine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Machine> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Machine toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
    }
}
