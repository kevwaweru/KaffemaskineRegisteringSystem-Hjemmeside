using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class JobRepo : IJobRepo
    {
        private readonly string _connectionString;
       

        public JobRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }



       /// <summary>
       /// 
       /// </summary>
       /// <param name="task"></param>
       /// <returns></returns>
        public async Task CreateAsync(Job task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"
                                    INSERT INTO Tasks 
                                    (TaskTemplateId, Comment, CreateDate, Deadline, IsCompleted, MachineId, UserId, FrequencyId)
                                    VALUES 
                                     (@TaskTemplateId, @Comment, @CreateDate, @Deadline, @IsCompleted, @MachineId, @UserId, @FrequencyId)";

         //TaskId og TaskTemplateId skal ikke sendes med pga. de contraints vi har lavet vel? E.g. (1,1).
                    using var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@TaskTemplateId", task.TaskTemplateId);
                    command.Parameters.AddWithValue("@Comment", task.Comment);
                    command.Parameters.AddWithValue("@CreateDate", task.CreatedDate);
                    command.Parameters.AddWithValue("@Deadline", task.Deadline);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    command.Parameters.AddWithValue("@MachineId", task.MachineId);
                    command.Parameters.AddWithValue("@UserId", task.UserId);
                    command.Parameters.AddWithValue("@FrequencyId", task.FrequencyId);


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

        
        public async Task DeleteAsync(Job toBeDeletedAssignment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Assignments WHERE TaskId = @TaskId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@TaskId", toBeDeletedAssignment.TaskId);
                    
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

        /// <summary>
        /// Den metode opdatere en assingment
        /// </summary>
        /// <param name="assignmentToBeUpdated">Angiv hvilke opgave der skal opdateres</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> den kaster excetion hvis du er dårlig til at kalde den.</exception>
        public async Task UpdateAsync(Job assignmentToBeUpdated)
        {
            try
            {
                if (assignmentToBeUpdated == null)
                {
                    throw new ArgumentNullException(nameof(assignmentToBeUpdated), "Du bliver nødt til at sende ny data med, hvis du vil have opdateret opgaven.");
                }
               
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                      Update Tasks
                      Set 
                          Comment = @Comment,
                          CreateDate = @CreateDate, 
                          Deadline = @Deadline, 
                          IsCompleted = @IsCompleted, 
                          MachineId = @MachineId,
                          UserId = @UserId,
                          FrequencyId = @FrequencyId,  
                      Where
                          TaskId = @TaskId";
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@TaskId", assignmentToBeUpdated.TaskId);
                        command.Parameters.AddWithValue("@Comment", (object?)assignmentToBeUpdated.Comment);
                        command.Parameters.AddWithValue("@CreateDate", assignmentToBeUpdated.CreatedDate);
                        command.Parameters.AddWithValue("@Deadline", assignmentToBeUpdated.Deadline);
                        command.Parameters.AddWithValue("@IsCompleted", assignmentToBeUpdated.IsCompleted);
                        command.Parameters.AddWithValue("@UserId", assignmentToBeUpdated.UserId);
                        command.Parameters.AddWithValue("@FrequencyId", assignmentToBeUpdated.FrequencyId);

                        connection.Open(); 
                        await command.ExecuteNonQueryAsync(); //

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
        }
       
        public async Task<List<Job>> GetAllAsync()
        {
            var  = new List<Job>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Tasks";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var job = new Job
                                {
                                    TaskId = reader.GetInt32(0),
                                    TaskTemplateId = reader.GetInt32(1),
                                    Comment = reader.GetString(2),
                                    CreatedDate = reader.GetDateTime(3),
                                    Deadline = reader.GetDateTime(4),
                                    IsCompleted = reader.GetBoolean(5),
                                    MachineId = reader.GetInt32(6),
                                    UserId = reader.GetInt32(7),
                                    FrequencyId = reader.GetInt32(8),
                                };
                                .Add(job);
                            }
                        }
                    }
                }
                return assignments;
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

        public async Task<Job> GetByIdAsync(int assignmentId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Assignments WHERE AssignmentId = @AssignmentId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Job
                                {
                                    AssignmentId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Comment = reader.GetString(2),
                                    CreateDate = reader.GetDateTime(3),
                                    IsCompleted = reader.GetBoolean(4)
                                };
                            }
                            else
                            {
                                throw new Exception($"Tasks with ID {assignmentId} does not exist.");
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

    }
}
