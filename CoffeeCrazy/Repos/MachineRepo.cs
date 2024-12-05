using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : ICRUDRepo<Machine>
    {
        private readonly string _connectionString;
        private readonly ValidateDatabaseMethods _validateDatabaseMethods;

        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(Machine toBeCreatedMachine)
        {

            //validate input from parameter kunne blive sat ind her.
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"INSERT INTO Machines (Placement, CampusId, Image)
                                        VALUES (@Placement, @CampusId, @Image)";

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Placement", toBeCreatedMachine.Placement);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeCreatedMachine.Campus);
                    command.Parameters.AddWithValue("@Image", (byte[]?)toBeCreatedMachine.Image); //tilføjet Image til Create.

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

        public async Task<List<Machine>> GetAllAsync()
        {
            List<Machine> machines = new List<Machine>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Machines";

                    SqlCommand command = new SqlCommand(query, connection);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Machine machine = new Machine
                            {
                                MachineId = (int)reader["MachineId"],
                                Status = (bool)reader["Status"],
                                Placement = reader["Placement"] as string,
                                Campus = (Campus)reader["CampusId"],
                                Image = GetImageValue(reader["Image"]) //Image tilføjet 05.12
                                
                            };

                            machines.Add(machine);
                        }
                    }
                }
                return machines;
            }
            catch (SqlException ex)
            {
                // Log database-related errors.
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general errors.
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// En metode til at validere om vores billede i databasen indeholder værdier.
        /// Evt. give den et andet navn - alla ControlForImageValue eller sådan.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> returnerer en byte hvis der er noget i den gældende kolonne. Ellers sætter den kolonnen til at indeholde null, hvilekt den godt må </returns>
        public static byte[]? GetImageValue(object value)
        {
            if (value != DBNull.Value)  //validere at vores værdi ikke er null så der kunne komme en 
            {
                return (byte[])value;
            }
            else
            {
                return null;
            }
        }






        public async Task<Machine> GetByIdAsync(int machineId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Machines WHERE MachineId = @MachineId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MachineId", machineId);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Machine
                            {
                                MachineId = (int)reader["MachineId"],
                                Status = (bool)reader["Status"],
                                Placement = reader["Placement"] as string,
                                Campus = (Campus)reader["CampusId"]
                            };
                        }
                        else
                        {
                            throw new InvalidOperationException($"Machine with ID {machineId} does not exist.");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log database errors and rethrow.
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Log general errors and rethrow.
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Machine toBeUpdatedMachine)
        {
            try
            {
                if (toBeUpdatedMachine == null)
                {
                    throw new ArgumentNullException(nameof(toBeUpdatedMachine), "You will need to submit new data if you want the Machine updated.");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"UPDATE Machines SET
                                        Status = @Status,
                                        Placement = @Placement,
                                        CampusId = @CampusId
                                    WHERE
                                        MachineId = @MachineId";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Status", toBeUpdatedMachine.Status);
                    command.Parameters.AddWithValue("@Placement", toBeUpdatedMachine.Placement);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeUpdatedMachine.Campus);
                    command.Parameters.AddWithValue("@MachineId", toBeUpdatedMachine.MachineId);

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

    }
}
