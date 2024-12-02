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

                    using var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@TaskTemplateId", task.JobTemplateId);
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


        public async Task DeleteAsync(Job toBeDeletedJob)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Tasks WHERE TaskId = @TaskId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@TaskId", toBeDeletedJob.JobId);

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
        /// <param name="JobToBeUpdated">Angiv hvilke opgave der skal opdateres</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> den kaster excetion hvis du er dårlig til at kalde den.</exception>
        public async Task UpdateAsync(Job JobToBeUpdated)
        {
            try
            {
                if (JobToBeUpdated == null)
                {
                    throw new ArgumentNullException(nameof(JobToBeUpdated), "Du bliver nødt til at sende ny data med, hvis du vil have opdateret opgaven.");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                      Update Tasks
                      Set 
                          TaskTemplateId = @TaskTemplateId,
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
                        command.Parameters.AddWithValue("@TaskTemplateId", JobToBeUpdated.JobTemplateId);
                        command.Parameters.AddWithValue("@Comment", (object?)JobToBeUpdated.Comment);
                        command.Parameters.AddWithValue("@CreateDate", JobToBeUpdated.CreatedDate);
                        command.Parameters.AddWithValue("@Deadline", JobToBeUpdated.Deadline);
                        command.Parameters.AddWithValue("@IsCompleted", JobToBeUpdated.IsCompleted);
                        command.Parameters.AddWithValue("@UserId", JobToBeUpdated.UserId);
                        command.Parameters.AddWithValue("@FrequencyId", JobToBeUpdated.FrequencyId);

                        connection.Open();
                        await command.ExecuteNonQueryAsync(); 

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
            var jobs = new List<Job>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string getJobsQuery = @"
                SELECT TaskId, TaskTemplateId, Comment, CreatedDate, Deadline, 
                       IsCompleted, MachineId, UserId, FrequencyId 
                FROM Tasks";

                    using (var command = new SqlCommand(getJobsQuery, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Create a new Job object and populate it with data from the database.
                                var job = new Job
                                {
                                    JobId = reader.GetInt32(0),
                                    JobTemplateId = reader.GetInt32(1),
                                    Comment = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    CreatedDate = reader.GetDateTime(3),
                                    Deadline = reader.GetDateTime(4),
                                    IsCompleted = reader.GetBoolean(5),
                                    MachineId = reader.GetInt32(6),
                                    UserId = reader.GetInt32(7),
                                    FrequencyId = reader.GetInt32(8),

                                    // Including TaskTemplate data
                                    JobTemplate = new JobTemplate
                                    {
                                        TaskTemplateId = reader.GetInt32(1),
                                        Title = reader.GetString(9),
                                        Description = reader.IsDBNull(10) ? null : reader.GetString(10)
                                    }
                                };

                                // Add the job to the list.
                                jobs.Add(job);
                            }
                        }
                    }
                }
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

            // Return the list of jobs.
            return jobs;
        }

        public async Task<Job> GetByIdAsync(int taskId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // SQL query to retrieve data from Tasks and TaskTemplates.
                    const string query = @"
                SELECT t.TaskId, t.TaskTemplateId, t.Comment, t.CreatedDate, t.Deadline, 
                       t.IsCompleted, t.MachineId, t.UserId, t.FrequencyId,
                       tt.Title AS TaskTemplateTitle, tt.Description AS TaskTemplateDescription
                FROM Tasks t
                INNER JOIN TaskTemplates tt ON t.TaskTemplateId = tt.TaskTemplateId
                WHERE t.TaskId = @TaskId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameter for the task ID.
                        command.Parameters.AddWithValue("@TaskId", taskId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Check if the record exists and return the job object.
                            if (await reader.ReadAsync())
                            {
                                return new Job
                                {
                                    JobId = reader.GetInt32(0),
                                    JobTemplateId = reader.GetInt32(1),
                                    Comment = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    CreatedDate = reader.GetDateTime(3),
                                    Deadline = reader.GetDateTime(4),
                                    IsCompleted = reader.GetBoolean(5),
                                    MachineId = reader.GetInt32(6),
                                    UserId = reader.GetInt32(7),
                                    FrequencyId = reader.GetInt32(8),

                                    // Including TaskTemplate data
                                    JobTemplate = new JobTemplate
                                    {
                                        TaskTemplateId = reader.GetInt32(1),
                                        Title = reader.GetString(9),
                                        Description = reader.IsDBNull(10) ? null : reader.GetString(10)
                                    }
                                };
                            }
                            else
                            {
                                throw new InvalidOperationException($"Task with ID {taskId} does not exist.");
                            }
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
    }
}
