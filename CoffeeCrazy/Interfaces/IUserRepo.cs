using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    //Kevin
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
