using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Utilitys;
using Microsoft.Data.SqlClient;

namespace CoffeeCrazy.Interfaces
{
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<bool> DeleteUserAsync(int userIdToDelete, int currentAdminUserId);


        Task<(byte[] passwordHash, byte[] passwordSalt, Role role, string firstName, int userId)> GetUserByEmailAsync(string username);

        System.Threading.Tasks.Task ChangePasswordAsync(string email, string currentPassword, string newPassword);

        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
