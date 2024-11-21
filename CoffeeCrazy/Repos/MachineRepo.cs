using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo
    {
        public void Create(Machine machine)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "INSERT INTO Machines (MachineId, CampusId, Status, Placement) VALUES (@MachineName, @CampusId, @Status, @Placement)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@MachineName", SqlDbType.NVarChar).Value = machine.MachineId;
                        command.Parameters.Add("@CampusId", SqlDbType.Int).Value = machine.CampusId;
                        command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                        command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void Update(Machine machine)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlQuery = "UPDATE Machines SET MachineName = @MachineName, CampusId = @CampusId, Status = @Status, Placement = @Placement WHERE MachineId = @MachineId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add("@MachineId", SqlDbType.Int).Value = machine.MachineId;
                        command.Parameters.Add("@CampusId", SqlDbType.Int).Value = machine.CampusId;
                        command.Parameters.Add("@Status", SqlDbType.Bit).Value = machine.Status;
                        command.Parameters.Add("@Placement", SqlDbType.NVarChar).Value = machine.Placement;

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
