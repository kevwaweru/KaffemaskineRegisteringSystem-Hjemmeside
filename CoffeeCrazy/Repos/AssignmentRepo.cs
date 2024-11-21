using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Repos
{
    public class AssignmentRepo : ICRUDRepo<Assignment> 
    {
        private readonly string _connectionstring;

        public AssignmentRepo(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public Task CreatAsyncc(Assignment toBeCreatedT)
        {
            throw new NotImplementedException();
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

        //CRUD - Update method.
        public async Task UpdateAsync(Assignment assignmentToBeUpdated)
        {
            try { 
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
