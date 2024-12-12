using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Services;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : ICRUDRepo<Machine>
    {
        private readonly string _connectionString;
        //private readonly ValidateDataRepo _validateDatabaseRepo;
        private readonly IImageService _imageService;
        public MachineRepo(IConfiguration configuration, IImageService imageService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _imageService = imageService;
        }

        public async Task CreateAsync(Machine toBeCreatedMachine)
        {

            //validate input from parameter kunne blive sat ind her.
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"INSERT INTO Machines (Placement, CampusId, MachineImage)
                                        VALUES (@Placement, @CampusId, @MachineImage)";

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Placement", toBeCreatedMachine.Placement);
                    command.Parameters.AddWithValue("@CampusId", (int)toBeCreatedMachine.Campus);
                    command.Parameters.AddWithValue("@MachineImage", toBeCreatedMachine.MachineImage != null ? _imageService.FormFileToByteArray(toBeCreatedMachine.MachineImage) : DBNull.Value);


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
        /// Get all machines from list.
        /// </summary>
        /// <returns> Machine </returns>
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
                                MachineImage = reader["MachineImage"] != DBNull.Value ? _imageService.ByteArrayToFormFile((byte[])reader["MachineImage"]) : null
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
        /// Read/Get all method for machine
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns> Machine </returns>
        /// <exception cref="InvalidOperationException"></exception>
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
                                Campus = (Campus)reader["CampusId"],
                                MachineImage = reader["MachineImage"] != DBNull.Value ? _imageService.ByteArrayToFormFile((byte[])reader["MachineImage"]) : null
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


        /// <summary>
        /// Method to update a Machine object.
        /// </summary>
        /// <param name="toBeUpdatedMachine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
                                        CampusId = @CampusId,
                                        MachineImage=@MachineImage
                                    WHERE
                                        MachineId = @MachineId";

                    var existingMachine = await GetByIdAsync(toBeUpdatedMachine.MachineId);

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Status", toBeUpdatedMachine.Status != true ? toBeUpdatedMachine.Status : existingMachine.Status);
                    command.Parameters.AddWithValue("@Placement", toBeUpdatedMachine.Placement ?? existingMachine.Placement);
                    command.Parameters.AddWithValue("@CampusId", toBeUpdatedMachine.Campus != 0 ? (int)toBeUpdatedMachine.Campus : (int)existingMachine.Campus);
                    command.Parameters.AddWithValue("@MachineId", toBeUpdatedMachine.MachineId);

                    if (toBeUpdatedMachine.MachineImage != null)
                    {
                        command.Parameters.AddWithValue("@MachineImage", _imageService.FormFileToByteArray(toBeUpdatedMachine.MachineImage));
                    }
                    else if (existingMachine.MachineImage != null)
                    {
                        command.Parameters.AddWithValue("@MachineImage", _imageService.FormFileToByteArray(existingMachine.MachineImage));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MachineImage", DBNull.Value);
                    }

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
        /// Method to delete a Machine object.
        /// </summary>
        /// <param name="toBeDeletedMachine"></param>
        /// <returns></returns>
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
