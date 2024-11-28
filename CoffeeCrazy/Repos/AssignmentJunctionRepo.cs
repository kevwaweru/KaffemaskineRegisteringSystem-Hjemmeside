using System.Data;
using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace CoffeeCrazy.Repos
{
    public class AssignmentJunctionRepo : IAssignmentJunctionRepo
    {
        //mangler adskillige af metoderne i IAssignmentRepo.
        
        private readonly string _connectionString;
        private readonly IAssignmentSetRepo _assignmentSetRepo;
        private readonly IAssignmentRepo _assignmentRepo;

        //CTOR
        public AssignmentJunctionRepo(IConfiguration configuration, IAssignmentSetRepo assignmentSetRepo, IAssignmentRepo assignmentRepo)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe Maskine Database' not found.");
            _assignmentRepo = assignmentRepo;
            _assignmentSetRepo = assignmentSetRepo;

        }

        /// <summary>
        /// Uses both assignmentId and AssignmentSetId as parameters to create a new AssignmentJunction
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <param name="assignmentSetId"></param>
        /// <returns></returns>
        public async Task AddAssignmentToAssignmentSetAsync(int assignmentSetId, List<int> assignmentId)
        {
            if (assignmentId == null || !assignmentId.Any())
            {
                throw new Exception("No assignments provided to add to the set.");
            }

                try
                {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sqlQuery = @"
                                      INSERT INTO 
                                        AssignmentJunction (AssignmentSetId, AssignmentId) 
                                      VALUES 
                                        (@AssignmentSetId, @AssignmentId)";


                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        
                        command.Parameters.AddWithValue("@AssignmentSetId", assignmentSetId);
                        command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                        
                        await command.ExecuteNonQueryAsync();
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

        /// <summary>
        /// Method to retrieve and read all attributes inside all objects stored in an AssignmentJunction object.
        /// </summary>
        /// <returns> return createListToGetAll </returns>
        public async Task GetAllObjectsFromAssignmentJunctionsAsync(int assignmentSetId)
        {
            //Valideringstest af databasen og om AssignmentSetId eksisterer.
            if (!await DoesAssignmentIdExistAsync(assignmentSetId))
            {
                throw new InvalidOperationException($"The assignment Id {assignmentSetId} does not exist.");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    //await _assignmentSetRepo.GetByAssignmentSetIdAsync(assignmentSetId);

                    string getAssignmentsQuery = @"
                                    SELECT AssignmentId 
                                    FROM AssignmentJunction 
                                    WHERE AssignmentSetId = @AssignmentSetId";


                    List<int> assignmentIds = new List<int>();

                    foreach (var assignmentId in assignmentIds)
                    {
                        string getAssignmentQuery = @"
                                        SELECT AssignmentId, Title, Comment, CreateDate, IsCompleted
                                        FROM Assignments
                                        WHERE AssignmentId = @AssignmentId";

                        using (SqlCommand command = new SqlCommand(getAssignmentQuery, connection))
                        {
                            command.Parameters.AddWithValue("@AssignmentId", assignmentId);

                            using (SqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    AssignmentId = reader.GetInt32(0),
                                    Title: reader.GetString(1),
                                    Comment: (reader.IsDBNull(2) ?: reader.GetString(2)),
                                    CreateDate: reader.GetDateTime(3),
                                    IsCompleted: reader.GetBoolean(4),
                                   
                                };
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
        /// In this method we update individual assignments
        /// </summary>
        /// <param name="toBeUpdatedT"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateAssignmentAsync(int assignmentSetId, int oldAssignmentId, int newAssignmentId)
        {
            //Valideringstest af input.
            if(assignmentSetId <= 0 || oldAssignmentId <= 0 || newAssignmentId <= 0)
            {
                throw new ArgumentException("Assignment Ids must be greater than 0.");
            }

            //Valideringstest af databasen og om AssignmentSetId eksisterer.
            if (!await DoesAssignmentIdExistAsync(assignmentSetId))
            {
                throw new InvalidOperationException($"The assignment Id {assignmentSetId}  does not exist.");
            }

            //Valideringstest af databasen og om det gamle assignmentId eksisterer.
            if (!await DoesAssignmentIdExistAsync(oldAssignmentId))
            {
                throw new InvalidOperationException($"The assignment Id {oldAssignmentId}  does not exist.");
            }

            //Valideringstest af databasen og om det nye assignmentId eksisterer.
            if (!await DoesAssignmentSetIdExistAsync(newAssignmentId))
            {
                throw new InvalidOperationException($"The assignment ID {newAssignmentId} does not exist.");
            }

            //Metode til at erstatte det ene Id med det andet.
            await ReplaceAssignmentIdAsync(oldAssignmentId, newAssignmentId);
        }

        /// <summary>
        /// Metode til at kontrollere om assignmentSetId eksisterer i databasen.
        /// </summary>
        /// <returns> Returnere en bool, som er taget fra Sql's EXISTS, der returnerer 0 eller 1.
        /// 0 & 1 konverterer vi dermed til true eller false.
        /// </returns>
        private async Task<bool> DoesAssignmentSetIdExistAsync(int assignmentSetId)
        {
            string query = @"
                        SELECT CASE WHEN EXISTS (
                            SELECT AssignmentSetId
                            FROM AssignmentSets
                            WHERE AssignmentSetId = @AssignmentSetId)
                            THEN 1 ELSE 0 END";

                                                    
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;

                    var result = await command.ExecuteScalarAsync();

                    return Convert.ToInt32(result) == 1;
                }

            }
        }

        /// <summary>
        /// Metode til at kontrollere om databasen indeholder ens AssignmentId-Query
        /// </summary>
        /// <returns> returnerer en bool-lignende værdi, igennem Sql Exist Query. 
        /// Exist returnere 0 eller 1, hvilket svarer til true eller false, men vi skal have konverteret det først.        /// 
        /// </returns>
        //Metode til at kontrollere om databasen indeholde ens AssignmentId-query. 
        private async Task<bool> DoesAssignmentIdExistAsync(int assignmentId)
        {
            string query = @"
                            SELECT CASE WHEN EXISTS (
                            SELECT AssignmentId
                            FROM Assignment
                            WHERE AssignmentId = @AssignmentId)
                            THEN 1 ELSE 0 END";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.Add("@AssignmentId", SqlDbType.Int).Value = assignmentId;
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) == 1;
                }

            }
        }


        //Metode til at erstatte et Id med et andet Id.
        private async Task ReplaceAssignmentIdAsync(int oldAssignmentId, int newAssignmentId)
        {
            string query = @"
                    UPDATE AssignmentJunction
                    SET AssignmentId = @NewAssignmentId
                    WHERE AssignmentId = @OldAssignmentId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@AssignmentId", SqlDbType.Int).Value = newAssignmentId;
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }

            }
            
        }


        /// <summary>
        /// Delete method for AssignmentJunction.
        /// </summary>
        /// <returns> NIL </returns>
        public async Task DeleteAsync(int assignmentId, int assignmentSetId)
        {
            if (assignmentId <= 0 || assignmentSetId <= 0)
                throw new ArgumentException("Invalid AssignmentId or AssignmentSetId.");

            const string query = @"
            DELETE FROM AssignmentJunction
            WHERE AssignmentId = @AssignmentId AND AssignmentSetId = @AssignmentSetId;";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.Add("@AssignmentId", SqlDbType.Int).Value = assignmentId;
                        command.Parameters.Add("@AssignmentSetId", SqlDbType.Int).Value = assignmentSetId;

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while deleting the AssignmentJunction.", ex);
            }
        }


    }
}
