using CoffeeCrazy.Model;

namespace CoffeeCrazy.Interfaces
{
    //Kevin
    public interface IUserRepo : ICRUDRepo<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
