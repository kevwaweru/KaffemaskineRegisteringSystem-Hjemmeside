using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Data.SqlClient;
using Task = System.Threading.Tasks.Task;

namespace CoffeeCrazy.Repos
{
    public class TaskRepo : ITaskRepo
    {
        private readonly string _connectionString;
       

        public TaskRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="task"></param>
       /// <returns></returns>
        public async System.Threading.Tasks.Task CreateAsync(Models.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"
                                    INSERT INTO Tasks 
                                    (TaskTemplate, Comment, CreateDate, IsCompleted, MachineId, FrequencyId)
                                    VALUES 
                                    (@TaskTemplate, @Comment, @CreateDate, @IsCompleted, @MachineId, @FrequencyId)";

         //TaskId og TaskTemplateId skal ikke sendes med pga. de contraints vi har lavet vel? E.g. (1,1).
                    using var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@TaskTemplateId", task.TaskTemplateId);
                    command.Parameters.AddWithValue("@Comment", task.Comment);
                    command.Parameters.AddWithValue("@CreateDate", task.CreateDate);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

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

        
        public async System.Threading.Tasks.Task DeleteAsync(Models.Task toBeDeletedAssignment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Assignments WHERE AssignmentId = @AssignmentId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@AssignmentId", toBeDeletedAssignment.TaskId);
                    
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
        public async System.Threading.Tasks.Task UpdateAsync(Models.Task assignmentToBeUpdated)
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
                      Update Assignments
                      Set 
                          Titel = @Titel,
                          Comment = @Comment,
                          CreateDate = @CreateDate, 
                          IsCompleted = @IsCompleted,
                      Where
                          AssignmentId = @AssignmentId";
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@AssignmentId", assignmentToBeUpdated.AssignmentId);
                        command.Parameters.AddWithValue("@Titel", assignmentToBeUpdated.Title);
                        command.Parameters.AddWithValue("@Comment", (object?)assignmentToBeUpdated.Comment);
                        command.Parameters.AddWithValue("@CreateDate", assignmentToBeUpdated.CreateDate);
                        command.Parameters.AddWithValue("@IsCompleted", assignmentToBeUpdated.IsCompleted);

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
       
        public async Task<List<Models.Task>> GetAllAsync()
        {
            var assignments = new List<Models.Task>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Assignments";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var assignment = new Models.Task
                                {
                                    TaskId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Comment = reader.GetString(2),
                                    CreateDate = reader.GetDateTime(3),
                                    IsCompleted = reader.GetBoolean(4)
                                };

                                assignments.Add(assignment);
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

        public async Task<Task> GetByIdAsync(int assignmentId)
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
                                return new Models.Task
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
