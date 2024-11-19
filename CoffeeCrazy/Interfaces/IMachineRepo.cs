using System.Reflection.PortableExecutable;

namespace CoffeeCrazy.Interfaces
{
    public interface IMachineRepo : ICRUDRepo<Machine>
    {
        Task<List<Machine>> GetAllCampusAsync(int campusId);
    }
}
