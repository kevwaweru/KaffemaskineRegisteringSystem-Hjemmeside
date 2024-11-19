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
                ?? throw new InvalidOperationException("Connection string 'VaskEnTidDB' not found.");
        }

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
    }
}
