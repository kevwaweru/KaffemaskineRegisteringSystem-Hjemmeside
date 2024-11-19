using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;

namespace CoffeeCrazy.Interface
{
    //Kevin
    public interface IUser : ICRUD<User>
    {
        User GetUserByEmail(string email);
    }
}
