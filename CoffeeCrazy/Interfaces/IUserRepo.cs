using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Interfaces
{
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<bool> DeleteUserAsync(int userIdToDelete, int currentAdminUserId);


        Task<(byte[] passwordHash, byte[] passwordSalt, Role role, string firstName, int userId)> GetUserByEmailAsync(string username);

        Task ChangePasswordAsync(string email, string currentPassword, string newPassword);

        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
}
