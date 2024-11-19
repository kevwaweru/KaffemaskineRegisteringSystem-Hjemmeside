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
        public async Task CreateAsync(Assignment assignment)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                const string query = @"
                                    INSERT INTO Assignment (Titel, Comment, CreateDate, IsCompleted)
                                    VALUES (@Titel, @Comment, @CreateDate, @IsCompleted)";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Titel", assignment.Titel);
                command.Parameters.AddWithValue("@Comment", assignment.Comment ?? (object)DBNull.Value); //Do not know if needed, it just Makes                                                                                         Comment null-able 
                command.Parameters.AddWithValue("@CreateDate", assignment.CreateDate);
                command.Parameters.AddWithValue("@IsCompleted", assignment.IsCompleted);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
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

        //This Method Creates a Assignment


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
