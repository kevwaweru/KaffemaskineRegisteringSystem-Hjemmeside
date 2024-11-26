using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace CoffeeCrazy.Repos
{
    public class AssignmentJunctionRepo : IAssignmentJunctionRepo
    {
        private readonly string _connectionString;

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
        public async Task AddAssignmentToAssignmentSetAsync(int assignmentId, int assignmentSetId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"
                                           INSERT INTO AssignmentJunction (AssignmentSetId, AssignmentId) 
                                           VALUES (@AssignmentSetId, @AssignmentId)";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);
                        command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                        await connection.OpenAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AssignmentJunction>> GetAllAssignmentsFromAssignmentJunctionAsync()
        {
            List<AssignmentJunction> getAllDetails = new List<AssignmentJunction>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    //Først tager vi og henter AssignmentSetId og AssignmentId og smider dem på AssignmentJunction
                    //Dernæst tager vi assignment(s) attributter (fire stk.s)
                    //Dernæst tager vi
                    await connection.OpenAsync();
                    string query = @"SELECT 
                                        aj.AssignmentSetId, 
                                        aj.AssignmentId,
                                        
                                        a.Title AS AssignmentTitle,
                                        a.Comment AS AssignmentComment,
                                        a.CreateDate,
                                        a.IsCompleted,

                                        s.Deadline,
                                        s.SetCompleted,
                                        s.MachineId,

                                    FROM AssignmentJunction aj
                                    JOIN AssignmentSet s ON aj.AssignmentSetId = s.AssignmentSetId
                                    JOIN Assignment a ON aj.AssignmentId = a.AssignmentId";                       
                        

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                AssignmentJunction assignmentJunction = new AssignmentJunction
                                {
                                    //AssignmentSetId = reader.GetInt32(0),
                                    //AssignmentId = reader.GetInt32(1),

                                    AssignmentSetId = reader.GetInt32(0),
                                    AssignmentId = reader.GetInt32(1),
                                    Deadline = reader.GetDateTime(2),
                                    SetCompleted = reader.GetBoolean(3),
                                    MachineId = reader.GetInt32(4),
                                    AssignmentTitle = reader.GetString(5),
                                    AssignmentComment = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    CreateDate = reader.GetDateTime(7),
                                    IsCompleted = reader.GetBoolean(8)


                                };

                                getAllDetails.Add(assignmentJunction);
                            }

                        }
                    }
                }
                return getAllDetails;
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






        public async Task<AssignmentJunction> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(AssignmentJunction toBeUpdatedT)
        {
            throw new NotImplementedException();
        }     
        

        /// <summary>
        /// Delete method for AssignmentJunction.
        /// Holds Sql logic, uses "using".
        /// </summary>
        /// <param name="toBeDeletedT"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(AssignmentJunction toBeDeletedT)
        {
            throw new NotImplementedException();
        }
    }
}
