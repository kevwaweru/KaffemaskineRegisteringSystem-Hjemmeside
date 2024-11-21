using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace CoffeeCrazy.Repos
{
    public class AssignmentRepo
    {
        private readonly string _connectionString;

        public AssignmentRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }



        //This Method Creates a Assignment
        public async Task CreateAsync(Assignment assignment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SQLquery = @"
                                    INSERT INTO Assignments (Title, Comment, CreateDate, IsCompleted)
                                    VALUES (@Title, @Comment, @CreateDate, @IsCompleted)";

                    using var command = new SqlCommand(SQLquery, connection);
                    command.Parameters.AddWithValue("@Title", assignment.Title);
                    command.Parameters.AddWithValue("@Comment", assignment.Comment);
                    command.Parameters.AddWithValue("@CreateDate", assignment.CreateDate);
                    command.Parameters.AddWithValue("@IsCompleted", assignment.IsCompleted);

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



        //CRUD Delete
        public async Task DeleteAsync(Assignment toBeDeletedAssignment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "DELETE FROM Assignments WHERE AssignmentId = @AssignmentId";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    command.Parameters.AddWithValue("@AssignmentId", toBeDeletedAssignment.AssignmentId);
                    
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







        //CRUD - Update method.
        public async Task UpdateAsync(Assignment assignmentToBeUpdated)
        {
            try
            {
                //throw new NotImplementedException();
                //Ensures that value is sent along the update method
                if (assignmentToBeUpdated == null)
                {
                    throw new ArgumentNullException(nameof(assignmentToBeUpdated), "Du bliver nødt til at sende ny data med, hvis du vil have opdateret opgaven.");
                }

                //We use our connection string again, and with using, we don't have to use the dispose method.
                using (SqlConnection connection = new SqlConnection(_connectionstring))
                {
                    //I use the @ to make the string query in a "verbatim string literal", which means the code is read as if in one line and can span multiple lines. Easier to read.
                    //I then write the SQL commands necessary to update the table.

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
                        command.Parameters.AddWithValue("@Titel", assignmentToBeUpdated.Titel);
                        command.Parameters.AddWithValue("@Comment", (object?)assignmentToBeUpdated.Comment);
                        command.Parameters.AddWithValue("@CreateDate", assignmentToBeUpdated.CreateDate);
                        command.Parameters.AddWithValue("@IsCompleted", assignmentToBeUpdated.IsCompleted);

                        connection.Open(); //we open the connection and because we are using "using" the connection closes automatically after use.
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



















    }
}
