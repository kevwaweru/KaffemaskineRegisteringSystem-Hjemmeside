using CoffeeCrazy.Models;
using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Interfaces
{
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<bool> DeleteUserAsync(int userIdToDelete, int currentAdminUserId);
    }
}
