﻿using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class TaskTemplateRepo : ITaskTemplateRepo
    {
        private readonly string _connectionString;

        public TaskTemplateRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }

        /// <summary>
        /// Use to Create a TaskTemplate, with diffrent Tasks
        /// </summary>
        /// <param name="TaskTemplate">Takes an objekt of a Tasks.</param>
        /// <returns>A Sql Query to the database</returns>
        public async Task CreateAsync(TaskTemplate TaskTemplate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    string sqlQuery = @"
                                           INSERT INTO AssignmentSets (Title, SetCompleted, Deadline, AssignmentId, MachineId) 
                                           VALUES (@AssignmentSetId, @SetCompleted, @Deadline, @AssignmentId, @MachineId)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Title", SqlDbType.NVarChar).Value = TaskTemplate.Title;
                        command.Parameters.AddWithValue("@SetCompleted", SqlDbType.Bit).Value = TaskTemplate.SetCompleted;
                        command.Parameters.AddWithValue("@Deadline", SqlDbType.DateTime).Value = TaskTemplate.Deadline;
                        command.Parameters.AddWithValue("@AssignmentId", SqlDbType.Int).Value = TaskTemplate.AssignmentId;
                        command.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = TaskTemplate.MachineId;
                        
                        command.ExecuteNonQuery();
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

        public async Task<List<TaskTemplate>> GetAllAsync()
        {
            var assignmentSets = new List<TaskTemplate>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT AssignmentSetId, SetCompleted, Deadline, AssignmentId, MachineId FROM AssignmentSets";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var assignmentSet = new TaskTemplate
                                {
                                    AssignmentSetId = reader.GetInt32(0),
                                    SetCompleted = reader.GetBoolean(1),
                                    Deadline = reader.GetDateTime(2),
                                    AssignmentId = reader.GetInt32(3),
                                    MachineId = reader.GetInt32(4),
                                };
                                assignmentSets.Add(assignmentSet);
                            }
                        }
                    }
                }
                return assignmentSets;
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

        // Hent en opgaveliste baseret på ID
        public async Task<TaskTemplate> GetByIdAsync(int assignmentSetId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT AssignmentSetId, SetCompleted, Deadline, AssignmentId, MachineId FROM AssignmentSets";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new TaskTemplate
                                {
                                    AssignmentSetId = reader.GetInt32(0),
                                    SetCompleted = reader.GetBoolean(1),
                                    Deadline = reader.GetDateTime(2),
                                    AssignmentId = reader.GetInt32(3),
                                    MachineId = reader.GetInt32(4),
                                };
                            }
                            else
                            {
                                throw new Exception($"User with ID {assignmentSetId} does not exist.");
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
        /// <param name="assignmentSetToBeUpdated"> Takes an assignmentSet and updates the data.</param>
        /// <returns>The A sql query that UPDATES the assignmentSet Data</returns>
        /// <exception cref="ArgumentNullException">Cast an exception if Id == null</exception>
        public async System.Threading.Tasks.Task UpdateAsync(TaskTemplate assignmentSetToBeUpdated)
        {
            try
            {
                if (assignmentSetToBeUpdated == null)
                {
                    throw new ArgumentNullException(nameof(assignmentSetToBeUpdated), "Du bliver nødt til at sende ny data med, hvis du vil have opdateret opgavenSet.");
                }


                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                      Update Assignments
                      Set 
                        Title        = @Title
                        SetCompleted = @SetCompleted
                        Deadline     = @Deadline
                        AssignmentId = @AssignmentId
                        MachineId    = @MachineId
                      Where
                          AssignmentSetId = @AssignmentSetId";

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetToBeUpdated.AssignmentSetId);
                        command.Parameters.AddWithValue("@Title", assignmentSetToBeUpdated.AssignmentId);
                        command.Parameters.AddWithValue("@SetCompleted", assignmentSetToBeUpdated.SetCompleted);
                        command.Parameters.AddWithValue("@Deadline", (object?)assignmentSetToBeUpdated.Deadline);
                        command.Parameters.AddWithValue("@AssignmentId", assignmentSetToBeUpdated.AssignmentSetId);
                        command.Parameters.AddWithValue("@MachineId", assignmentSetToBeUpdated.MachineId);

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
        /// <summary>
        /// Deletes an AssignmentSet. From the Database
        /// </summary>
        /// <param name="toBeDeletedAssignment">Takes the Id of an AssignmentSet</param>
        /// <returns>A Sql query that delete assignment with that ID</returns>
        public async System.Threading.Tasks.Task DeleteAsync(TaskTemplate toBeDeletedAssignmentSet)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM AssignmentSets WHERE AssignmentSetId = @AssignmentSetId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@AssignmentSetId", toBeDeletedAssignmentSet.TaskTemplateId);

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
        /// ikke tænke på den endu. det skal lige rettes til
        /// </summary>
        /// <param name="assignmentSetId"></param>
        /// <returns></returns>
        public async Task<List<Models.Job>> GetByAssignmentSetIdAsync(int assignmentSetId)
        {
            var assignments = new List<Models.Job>();
            string errorMessage = string.Empty;
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT AssignmentSetId joint, Title, Comment, CreateDate, IsCompleted FROM Assignments WHERE AssignmentSetId = @AssignmentSetId";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var assignment = new Models.Job
                                {
                                    AssignmentId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Comment = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    CreateDate = reader.GetDateTime(3),
                                    IsCompleted = reader.GetBoolean(4),
                                };
                                assignments.Add(assignment);
                            }
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                errorMessage = "sql error:" + ex.Message;

            }
            catch (Exception ex)
            {
                errorMessage = "Unexpected error:" + ex.Message;

            }
            return (assignments);
        }

    }
}


