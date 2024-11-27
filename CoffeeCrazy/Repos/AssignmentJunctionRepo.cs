using System.Data;
using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace CoffeeCrazy.Repos
{
    public class AssignmentJunctionRepo : IAssignmentJunctionRepo
    {
        private readonly string _connectionString;

        //CTOR
        public AssignmentJunctionRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }

        /// <summary>
        /// Uses both assignmentId and AssignmentSetId as parameters to create a new AssignmentJunction
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <param name="assignmentSetId"></param>
        /// <returns></returns>
        public async Task AddAssignmentToAssignmentSetAsync(int assignmentSetId, List<int> assignmentId)
        {
            if (assignmentId == null || !assignmentId.Any())
            {
                throw new Exception("No assignments provided to add to the set.");
            }

                try
                {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sqlQuery = @"
                                      INSERT INTO 
                                        AssignmentJunction (AssignmentSetId, AssignmentId) 
                                      VALUES 
                                        (@AssignmentSetId, @AssignmentId)";


                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);
                        command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                        
                        await command.ExecuteNonQueryAsync();
                    }

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

        //Olivers Add to Junction metode.
        //public async Task AddAssignmentsToSetAsync(int assignmentSetId, List<int> assignmentIds)
        //{
        //    if (assignmentIds == null || !assignmentIds.Any())
        //    {
        //        throw new Exception("No assignments provided to add to the set.");
        //    }
        //
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        //            await connection.OpenAsync();
        //            // vær ops på at Assignment til assignmentMellemManden ikke er lavet endnu husk at ret til.
        //            string query = @"
        //        INSERT INTO AssignmentSetAssignments (AssignmentSetId, AssignmentId) 
        //        VALUES (@AssignmentSetId, @AssignmentId)";
        //
        //            foreach (var assignmentId in assignmentIds)
        //            {
        //                using (SqlCommand command = new SqlCommand(query, connection))
        //                {
        //                    command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);
        //                    command.Parameters.AddWithValue("@AssignmentId", assignmentId);
        //                    await command.ExecuteNonQueryAsync();
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine($"SQL error: {ex.Message}");
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        throw;
        //    }
        //}

        /// <summary>
        /// Method to retrieve and read all attributes inside all objects stored in an AssignmentJunction object.
        /// </summary>
        /// <returns> return createListToGetAll </returns>
        public async Task GetAllObjectsFromAssignmentJunctionsAsync(int assignmentSetId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string getAssignmentsQuery = @"
                                    SELECT AssignmentId 
                                    FROM AssignmentJunction 
                                    WHERE AssignmentSetId = @AssignmentSetId";

                    List<int> assignmentIds = new List<int>();

                    using (SqlCommand command = new SqlCommand(getAssignmentsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Console.WriteLine($"AssignmentSetId: {reader.GetInt32(0)}");
                                Console.WriteLine($"SetTitle: {reader.GetString(1)}");
                                Console.WriteLine($"SetDescription: {(reader.IsDBNull(2) ? "No description" : reader.GetString(2))}");
                                Console.WriteLine($"SetCreateDate: {reader.GetDateTime(3)}");
                                Console.WriteLine("---------------------------------");
                            }
                        }
                    }
                    foreach (var assignmentId in assignmentIds)
                    {
                        string getAssignmentQuery = @"
                                    SELECT AssignmentId, Title, Comment, CreateDate, IsCompleted
                                    FROM Assignments
                                    WHERE AssignmentId = @AssignmentId";

                        using (SqlCommand command = new SqlCommand(getAssignmentQuery, connection))
                        {
                            command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    Console.WriteLine($"AssignmentId: {reader.GetInt32(0)}");
                                    Console.WriteLine($"Title: {reader.GetString(1)}");
                                    Console.WriteLine($"Comment: {(reader.IsDBNull(2) ? "No comment" : reader.GetString(2))}");
                                    Console.WriteLine($"CreateDate: {reader.GetDateTime(3)}");
                                    Console.WriteLine($"IsCompleted: {reader.GetBoolean(4)}");
                                    Console.WriteLine("---------------------------------");
                                }
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

        /// <summary>
        /// In this method we update individual assignments
        /// </summary>
        /// <param name="toBeUpdatedT"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateAssignmentAsync(int assignmentSetId)
        {
            throw new NotImplementedException();
        }     
        

        /// <summary>
        /// Delete method for AssignmentJunction.
        /// </summary>
        /// <returns> NIL </returns>
        public async Task DeleteAsync(int assignmentId, int assignmentSetId)
        {
            if (assignmentId <= 0 || assignmentSetId <= 0)
                throw new ArgumentException("Invalid AssignmentId or AssignmentSetId.");

            const string query = @"
            DELETE FROM AssignmentJunction
            WHERE AssignmentId = @AssignmentId AND AssignmentSetId = @AssignmentSetId;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.Add("@AssignmentId", SqlDbType.Int).Value = assignmentId;
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while deleting the AssignmentJunction.", ex);
            }
        }


    }
}
