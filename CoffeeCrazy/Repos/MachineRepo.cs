using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo 
    {
        private readonly string _connectionString;
        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
        }

        //
        public async Task<List<Machine>> ReadAsync()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"SELECT 
                                        MachineId, Status, Placement
                                        FROM
                                        Machines
                                        ";


                    //    string query = "SELECT Id, Name, Description FROM YourTable";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))

                    
                        
                        await connection.OpenAsync();
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

        //public async Task<List<YourEntity>> ReadAsync()
        //{
        //    var results = new List<YourEntity>();
        //
        //    // Define your SQL query
        //    string query = "SELECT Id, Name, Description FROM YourTable";
        //
        //    // Open SQL connection using SqlConnection
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        // Create the SQL command
        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            // Open the connection
        //            await connection.OpenAsync();
        //
        //            // Execute the query and retrieve results
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    // Map data from the database to your entity
        //                    var entity = new YourEntity
        //                    {
        //                        Id = reader.GetInt32(0),             // Example for an int column
        //                        Name = reader.GetString(1),          // Example for a string column
        //                        Description = reader.GetString(2)    // Example for another string column
        //                    };
        //
        //                    results.Add(entity);
        //                }
        //            }
        //        }
        //    }
        //
        //    return results;
        //}






















    }
}
