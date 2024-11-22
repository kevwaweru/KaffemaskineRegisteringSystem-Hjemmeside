using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    public interface IMachineRepo : ICRUDRepo<Machine>
    {
        Task<List<Machine>> GetAllCampusAsync(int campusId);
    }
}
