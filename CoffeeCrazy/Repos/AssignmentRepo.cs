using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;
using System.Formats.Asn1;

namespace CoffeeCrazy.Repos
{
    public class AssignmentRepo : ICRUDRepo<Assignment>
    {
        private readonly string _connectionString;

        public AssignmentRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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


        public Task DeleteAsync(Assignment toBeDeletedT)
        {
            throw new NotImplementedException();
        }

        public Task<List<Assignment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Assignment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Assignment toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
    }
}
