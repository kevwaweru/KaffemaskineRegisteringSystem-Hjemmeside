using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Utilitys;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Interfaces
{
    //Kevin
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<(byte[] passwordHash, byte[] passwordSalt, Role role)> GetUserByEmailAsync(string username);

        Task ChangePasswordAsync(string email, string currentPassword, string newPassword);
        
        

    }
}
