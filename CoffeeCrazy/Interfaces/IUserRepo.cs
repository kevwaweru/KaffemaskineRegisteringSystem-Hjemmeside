using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    //Kevin
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<bool> DeleteUserAsync(int userIdToDelete, int currentAdminUserId);
        Task<User> GetUserByEmail(string email);
    }
}
