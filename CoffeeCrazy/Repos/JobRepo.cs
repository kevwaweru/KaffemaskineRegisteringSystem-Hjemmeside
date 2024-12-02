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

        // Olles version
        public int CreateTaskFromTemplate(int jobTemplateId, int userId, int taskSetId, int machineId, int frequencyId)
        {
            Job job = new Job();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string description;
                string title;
                var templateQuery = "SELECT Description, Title FROM TaskTemplates WHERE TaskTemplateId = @TaskTemplateId";
                using (var command = new SqlCommand(templateQuery, connection))
                {
                    command.Parameters.AddWithValue("@TaskTemplateId", jobTemplateId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            description = reader.GetString(0);
                            title = reader.GetString(1);
                        }
                        else
                        {
                            throw new Exception("TaskTemplate not found.");
                        }
                    }
                }

                var insertTaskQuery = @"

            INSERT INTO Tasks TaskTemplateId, Comment, CreateDate, Deadline, IsCompleted, MachineId, UserId, FrequencyId)
            OUTPUT INSERTED.TaskId
            VALUES (@TaskTemplateId, @Comment, @CreateDate, @Deadline, @IsCompleted, @MachineId, @UserId, @FrequencyId)";

                using (var command = new SqlCommand(insertTaskQuery, connection))
                {
                    command.Parameters.AddWithValue("@TaskTemplateId", jobTemplateId);
                    command.Parameters.AddWithValue("@Comment", job.Comment);
                    command.Parameters.AddWithValue("@CreateDate", job.CreatedDate);
                    command.Parameters.AddWithValue("@Deadline", job.Deadline);
                    command.Parameters.AddWithValue("@IsCompleted", job.IsCompleted);
                    command.Parameters.AddWithValue("@MachineId", machineId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@FrequencyId", frequencyId);

                    return (int)command.ExecuteScalar();
                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Job"></param>
        /// <returns></returns>
        public async Task CreateAsync(Job Job)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"
                                    INSERT INTO Tasks 
                                    (TaskTemplateId, Comment, CreatedDate, Deadline, IsCompleted, MachineId, FrequencyId)
                                    VALUES 
                                     (@TaskTemplateId, @Comment, @CreatedDate, @Deadline, @IsCompleted, @MachineId, @FrequencyId)";

                    using var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@TaskTemplateId", Job.JobTemplateId);
                    command.Parameters.AddWithValue("@CreatedDate", Job.CreatedDate);
                    command.Parameters.AddWithValue("@Deadline", Job.Deadline);
                    command.Parameters.AddWithValue("@IsCompleted", Job.IsCompleted);
                    command.Parameters.AddWithValue("@MachineId", Job.MachineId);
                    command.Parameters.AddWithValue("@Comment", Job.Comment);
                    command.Parameters.AddWithValue("@FrequencyId", Job.FrequencyId);


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

        public Task<List<Job>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Job> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
