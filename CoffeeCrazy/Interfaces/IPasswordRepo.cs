using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Interfaces
{
    public interface IPasswordRepo
    {
        Task ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<(byte[] passwordHash, byte[] passwordSalt, Role role, string firstName, int userId)> GetUserByEmailAsync(string username);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ValidateAndDeleteEmail(string email);
    }
}
