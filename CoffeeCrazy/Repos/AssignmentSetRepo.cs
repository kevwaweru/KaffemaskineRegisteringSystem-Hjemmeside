using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class AssignmentSetRepo : IAssignmentSetRepo
    {
        public Task CreateAsync(AssignmentSet toBeCreatedT)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(AssignmentSet toBeDeletedT)
        {
            throw new NotImplementedException();
        }

        public Task<List<AssignmentSet>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSet> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AssignmentSet toBeUpdatedT)
        {
            throw new NotImplementedException();
        }

            private readonly string _connectionString;

            // Konstruktor
            public AssignmentSetRepo(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection")
                                    ?? throw new InvalidOperationException("Connection string not found.");
            }
        
            // Opret en opgaveliste
            //public async Task CreateAsync(AssignmentSet assignmentSet)
            //{
            //    //SqlConnection connection = new SqlConnection(_connectionString);

            //    try
            //    {
            //        using (SqlConnection connection = new SqlConnection(_connectionString))
            //        {
            //            string sqlQuery = @"INSERT INTO AssignmentSets 
            //                                       (AssignmentSetId, SetCompleted, Deadline) 
            //                                   VALUES
            //                                       (@AssignmentSetId, @SetCompleted, @Deadline)";

            //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            //            {
            //                command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = assignmentSet.AssignmentSetId;
            //                command.Parameters.Add("@SetCompleted", SqlDbType.Bit).Value = assignmentSet.SetCompleted;
            //                command.Parameters.Add("@Deadline", SqlDbType.DateTime).Value = assignmentSet.Deadline;
            //                // der mangler AssignmentId før det her kan virke
            //                command.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error: " + ex.Message);
            //    }

            //}

            // Hent alle opgavelister
            public List<AssignmentSet> GetAll()
            {
                List<AssignmentSet> assignmentSets = new List<AssignmentSet>();

                SqlConnection connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();
                    string sqlQuery = "SELECT AssignmentSetId, SetCompleted, Deadline FROM AssignmentSets";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assignmentSets.Add(new AssignmentSet
                            {
                                AssignmentSetId = reader.GetInt32(0),
                                SetCompleted = reader.GetBoolean(1),
                                Deadline = reader.GetDateTime(2)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return assignmentSets;
            }

            // Hent en opgaveliste baseret på ID
            public AssignmentSet? GetById(int assignmentSetId)
            {
                AssignmentSet? assignmentSet = null;

                SqlConnection connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();

                    string sqlQuery = "SELECT AssignmentSetId, SetCompleted, Deadline FROM AssignmentSets WHERE AssignmentSetId = @AssignmentSetId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                assignmentSet = new AssignmentSet
                                {
                                    AssignmentSetId = reader.GetInt32(0),
                                    SetCompleted = reader.GetBoolean(1),
                                    Deadline = reader.GetDateTime(2)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }

                return assignmentSet;
            }

            // Opdater en opgaveliste
            public void Update(AssignmentSet assignmentSet)
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();

                    string sqlQuery = "UPDATE AssignmentSets SET AssignmentSetId = @AssignmentSetId, SetCompleted = @SetCompleted, Deadline = @Deadline WHERE AssignmentSetId = @AssignmentSetId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSet.AssignmentSetId;
                        command.Parameters.Add("@SetCompleted", SqlDbType.Bit).Value = assignmentSet.SetCompleted;
                        command.Parameters.Add("@Deadline", SqlDbType.DateTime).Value = assignmentSet.Deadline;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

                    
            public void Delete(int assignmentSetId)
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();
                    string sqlQuery = "DELETE FROM AssignmentSets WHERE AssignmentSetId = @AssignmentSetId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            /// <summary>
            /// This method is used to add a assignment to a set.
            /// </summary>
            /// <param name="assignmentSetId">Chose a Id to collect</param>
            /// <param name="assignment">and chose an assingment to add</param>
            public void AddAssignmentToSet(int assignmentSetId, Assignment assignment)
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                try
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO Assignments (AssignmentSetId, Comment, CreateDate, AssignmentSetId, IsCompleted) VALUES (@Title, @Comment, @CreateDate, @AssignmentSetId, @IsCompleted)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;
                        command.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = assignment.Comment;
                        command.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = assignment.CreateDate;
                        command.Parameters.Add("@IsCompleted", SqlDbType.Bit).Value = assignment.IsCompleted;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }


