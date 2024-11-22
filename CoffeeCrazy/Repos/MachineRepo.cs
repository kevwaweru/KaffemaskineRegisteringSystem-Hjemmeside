using CoffeeCrazy.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo
    {
        private readonly string _connectionString;
        public MachineRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'Kaffe maskine database' not found.");
        }

        public void Create(Machine machine)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sqlQuery = "INSERT INTO Machines (MachineId, CampusId, Status, Placement) VALUES (@MachineName, @CampusId, @Status, @Placement)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@MachineName", SqlDbType.NVarChar).Value = machine.MachineId;
                        command.Parameters.Add("@CampusId", SqlDbType.Int).Value = (int)machine.Campus;
                        command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                        command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

                        command.ExecuteNonQuery();
                    }
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
        public void Update(Machine machine)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE Machines SET MachineName = @MachineName, CampusId = @CampusId, Status = @Status, Placement = @Placement WHERE MachineId = @MachineId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@MachineId", SqlDbType.Int).Value = machine.MachineId;
                        command.Parameters.Add("@CampusId", SqlDbType.Int).Value = (int)machine.Campus;
                        command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                        command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

                        command.ExecuteNonQuery();
                    }
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

