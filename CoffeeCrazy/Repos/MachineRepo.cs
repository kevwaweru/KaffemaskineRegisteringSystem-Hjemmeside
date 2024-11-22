using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : IMachineRepo
    {
        private readonly string _connectionString;
        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe maskine database' not found.");
        }

        public async Task CreateAsync(Machine machine)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "INSERT INTO Machines (MachineId, CampusId, Status, Placement) VALUES (@MachineName, @CampusId, @Status, @Placement)";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.Add("@MachineName", SqlDbType.NVarChar).Value = machine.MachineId;
                    command.Parameters.Add("@CampusId", SqlDbType.Int).Value = (int)machine.Campus;
                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                    command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

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

        public async Task UpdateAsync(Machine machine)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "UPDATE Machines SET MachineName = @MachineName, CampusId = @CampusId, Status = @Status, Placement = @Placement WHERE MachineId = @MachineId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.Add("@MachineId", SqlDbType.Int).Value = machine.MachineId;
                    command.Parameters.Add("@CampusId", SqlDbType.Int).Value = (int)machine.Campus;
                    command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                    command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

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

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            machines.Add(new Machine
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

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

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
            catch (SqlException SqlEx)
            {
                Console.WriteLine("Sql-Exception Error." + SqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex);
            }
            return null;
        }

        public Task<List<Machine>> GetAllByCampusAsync(int campusId)
        {
            throw new NotImplementedException();
        }
    }
}
