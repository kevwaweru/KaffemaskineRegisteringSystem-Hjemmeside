using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : IMachineRepo
    {

        //Standard formalia. Can be deleted on merge.
        private readonly string _connectionString;
        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }

        //Method to get ALL machines from database.
        public async Task<List<Machine>> GetAllAsync()
        {
            var machines = new List<Machine>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sqlQuery = @"SELECT MachineId, Status, CampusId, Placement FROM Machines";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                machines.Add(new Model.Machine
                                {
                                    MachineId = reader.GetInt32(0),
                                    Status = reader.GetBoolean(1),
                                    Campus = (Campus)reader.GetInt32(2),
                                    Placement = reader.GetString(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException SqlEx)
            {
                Console.WriteLine("Sql-Exception Error." + SqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
            return machines;
        }


        //Get a single machine by its Id.
        public async Task<Machine> GetByIdAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    await connection.OpenAsync();

                    string sqlQuery = @"SELECT MachineId, Status, CampusId, Placement 
                                FROM Machines 
                                WHERE MachineId = @MachineId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@MachineId", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Machine
                                {
                                    MachineId = reader.GetInt32(0),
                                    Status = reader.GetBoolean(1),
                                    Campus = (Campus)reader.GetInt32(2),
                                    Placement = reader.GetString(3)
                                };
                            }
                            else
                            {
                                throw new Exception($"The machine with ID {id} does not exist.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }    }
}
