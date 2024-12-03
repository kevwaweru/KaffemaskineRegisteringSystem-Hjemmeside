using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class JobTemplateRepo : IJobTemplateRepo
    {
        private readonly string _connectionString;

        public JobTemplateRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }

        /// <summary>
        /// Use to Create a TaskTemplate, with diffrent Tasks
        /// </summary>
        /// <param name="JobTemplate">Takes an objekt of a Tasks.</param>
        /// <returns>A Sql Query to the database</returns>
        public async Task CreateAsync(JobTemplate JobTemplate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    string sqlQuery = @"
                                           INSERT INTO TaskTemplates (Title, Description) 
                                           VALUES (@Title, @Description)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", SqlDbType.NVarChar).Value = JobTemplate.Title;
                        command.Parameters.AddWithValue("Description", SqlDbType.NVarChar).Value = JobTemplate.Description;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine($"SQL error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Mistakes has happened: {ex.Message}");
                throw;
            }
        }

        public async Task<List<JobTemplate>> GetAllAsync()
        {
            var JobTemplates = new List<JobTemplate>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM TaskTemplates";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var JobTemplate = new JobTemplate
                                {
                                    JobTemplateId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2)                           
                                };
                                JobTemplates.Add(JobTemplate);
                            }
                        }
                    }
                }
                return JobTemplates;
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

        public async Task<JobTemplate> GetByIdAsync(int TaskTemplateId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM TaskTemplates";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new JobTemplate
                                {
                                    JobTemplateId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2),
                                };
                            }
                            else
                            {
                                throw new Exception($"User with ID {TaskTemplateId} does not exist.");
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
        /// Updates an assignmentSet
        /// </summary>
        /// <param name="TaskTemplate"> Takes an assignmentSet and updates the data.</param>
        /// <returns>The A sql query that UPDATES the assignmentSet Data</returns>
        /// <exception cref="ArgumentNullException">Cast an exception if Id == null</exception>
        public async Task UpdateAsync(JobTemplate TaskTemplate)
        {
            try
            {
                if (TaskTemplate == null)
                {
                    throw new ArgumentNullException(nameof(TaskTemplate), "Du bliver nødt til at sende ny data med, hvis du vil have opdateret opgavenSet.");
                }


                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                      Update TaskTemplates
                      Set 
                        Title        = @Title
                        Description = @Description
                      Where
                          TaskTemplateId = @TaskTemplateId";

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@TaskTemplateId", TaskTemplate.JobTemplateId);
                        command.Parameters.AddWithValue("@Title", TaskTemplate.Title);
                        command.Parameters.AddWithValue("@Description", TaskTemplate.Description);              

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
        /// <summary>
        /// Deletes an AssignmentSet. From the Database
        /// </summary>
        /// <param name="toBeDeletedAssignment">Takes the Id of an AssignmentSet</param>
        /// <returns>A Sql query that delete assignment with that ID</returns>
        public async Task DeleteAsync(JobTemplate JobTemplateToDelete)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM TaskTemplates WHERE TaskTemplateId = @TaskTemplateId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@TaskTemplateId", JobTemplateToDelete.JobTemplateId);

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
    }
}


