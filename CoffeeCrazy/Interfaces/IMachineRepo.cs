using CoffeeCrazy.Model;

namespace CoffeeCrazy.Interfaces
{
    public interface IMachineRepo : ICRUDRepo<Machine>
    {
        Task<List<Machine>> GetAllByCampusAsync(int campusId);
    }
}
