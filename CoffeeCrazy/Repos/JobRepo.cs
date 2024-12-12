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
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(Job toBeCreatedJob)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"INSERT INTO Jobs (Title, Description, Comment, IsCompleted, DateCreated, Deadline, FrequencyId, MachineId)
                                        VALUES (@Title, @Description, @Comment, @IsCompleted, @DateCreated, @Deadline, @FrequencyId, @MachineId)";

                    DateTime deadline;
                    if (toBeCreatedJob.FrequencyId == 3)
                    {
                        deadline = DateTime.Now.AddMonths(1);
                    }
                    else if (toBeCreatedJob.FrequencyId == 2)
                    {
                        deadline = DateTime.Now.AddDays(7);
                    }
                    else
                    {
                        deadline = DateTime.Now.AddDays(1);
                    }

                    SqlCommand command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Title", toBeCreatedJob.Title);
                    command.Parameters.AddWithValue("@Description", toBeCreatedJob.Description);
                    command.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(toBeCreatedJob.Comment) ? DBNull.Value : (object)toBeCreatedJob.Comment);
                    command.Parameters.AddWithValue("@IsCompleted", toBeCreatedJob.IsCompleted != true ? toBeCreatedJob.IsCompleted : false);
                    command.Parameters.AddWithValue("@DateCreated", toBeCreatedJob.DateCreated != default ? toBeCreatedJob.DateCreated : DateTime.Now);
                    command.Parameters.AddWithValue("@Deadline", toBeCreatedJob.Deadline != default ? toBeCreatedJob.Deadline : deadline);
                    command.Parameters.AddWithValue("@FrequencyId", toBeCreatedJob.FrequencyId);
                    command.Parameters.AddWithValue("@MachineId", toBeCreatedJob.MachineId);


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

        public async Task<List<Job>> GetAllAsync()
        {
            List<Job> jobs = new List<Job>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Jobs";

                    SqlCommand command = new SqlCommand(query, connection);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Job job = new Job
                            {
                                JobId = (int)reader["JobId"],
                                Title = (string)reader["Title"],
                                Description = (string)reader["Description"],
                                Comment = reader["Comment"] as string,
                                IsCompleted = (bool)reader["IsCompleted"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                Deadline = (DateTime)reader["Deadline"],
                                FrequencyId = (int)reader["FrequencyId"],
                                MachineId = (int)reader["MachineId"],
                                UserId = reader["UserId"] != DBNull.Value ? (int?)reader["UserId"] : null
                            };

                            jobs.Add(job);
                        }
                    }
                }
                return jobs;
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

        public async Task<Job> GetByIdAsync(int jobId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Jobs WHERE JobId = @JobId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@JobId", jobId);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Job
                            {
                                JobId = (int)reader["JobId"],
                                Title = (string)reader["Title"],
                                Description = (string)reader["Description"],
                                Comment = reader["Comment"] as string,
                                IsCompleted = (bool)reader["IsCompleted"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                Deadline = (DateTime)reader["Deadline"],
                                FrequencyId = (int)reader["FrequencyId"],
                                MachineId = (int)reader["MachineId"],
                                UserId = reader["UserId"] != DBNull.Value ? (int?)reader["UserId"] : null
                            };
                        }
                        else
                        {
                            throw new InvalidOperationException($"Job with ID {jobId} does not exist.");
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
        /// Den metode opdatere en assingment
        /// </summary>
        /// <param name="toBeUpdatedJob">Angiv hvilke opgave der skal opdateres</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> den kaster excetion hvis du er dårlig til at kalde den.</exception>
        public async Task UpdateAsync(Job toBeUpdatedJob)
        {
            try
            {
                if (toBeUpdatedJob == null)
                {
                    throw new ArgumentNullException(nameof(toBeUpdatedJob), "You will need to submit new data if you want the Job updated.");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"UPDATE Jobs SET
                                        Title = @Title,
                                        Description = @Description,
                                        Comment = @Comment,
                                        IsCompleted = @IsCompleted,
                                        FrequencyId = @FrequencyId,
                                        MachineId = @MachineId
                                    WHERE
                                        JobId = @JobId";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", toBeUpdatedJob.Title);
                    command.Parameters.AddWithValue("@Description", toBeUpdatedJob.Description);
                    command.Parameters.AddWithValue("@Comment", toBeUpdatedJob.Comment ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", toBeUpdatedJob.IsCompleted);
                    command.Parameters.AddWithValue("@FrequencyId", toBeUpdatedJob.FrequencyId);
                    command.Parameters.AddWithValue("@MachineId", toBeUpdatedJob.MachineId);
                    command.Parameters.AddWithValue("@JobId", toBeUpdatedJob.JobId);


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
                    string sqlQuery = "DELETE FROM Jobs WHERE JobId = @JobId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    command.Parameters.AddWithValue("@JobId", toBeDeletedJob.JobId);

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
        /// Fetches unique jobs for a specific machine and frequency, ensuring only one job per title is returned.
        /// </summary>
        /// <param name="machineId">The ID of the machine for which jobs are being fetched.</param>
        /// <param name="frequencyId">The frequency ID (daily, weekly, Monthly) to filter jobs.</param>
        /// <returns>A list of unique jobs, with one job per title.</returns>
        public async Task<List<Job>> GetGroupedJobsByFrequencyAsync(int machineId, int frequencyId)
        {
            List<Job> jobs = new List<Job>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                WITH RankedJobs AS
                (
                    SELECT *, ROW_NUMBER() OVER (PARTITION BY Title ORDER BY JobId) AS RowNum
                    FROM Jobs
                    WHERE MachineId = @MachineId AND FrequencyId = @FrequencyId
                )
                SELECT *
                FROM RankedJobs
                WHERE RowNum = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MachineId", machineId);
                    command.Parameters.AddWithValue("@FrequencyId", frequencyId);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Job job = new Job
                            {
                                JobId = (int)reader["JobId"],
                                Title = (string)reader["Title"],
                                Description = (string)reader["Description"],
                                Comment = reader["Comment"] as string,
                                IsCompleted = (bool)reader["IsCompleted"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                Deadline = (DateTime)reader["Deadline"],
                                FrequencyId = (int)reader["FrequencyId"],
                                MachineId = (int)reader["MachineId"],
                                UserId = reader["UserId"] != DBNull.Value ? (int?)reader["UserId"] : null
                            };

                            jobs.Add(job);
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

            return jobs;
        }
    }
}

