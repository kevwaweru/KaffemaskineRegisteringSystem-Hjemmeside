using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Model;

namespace CoffeeCrazy.Repos
{
    public class MachineRepo : ICRUDRepo<Machine>
    {
        public Task CreatAsyncc(Machine toBeCreatedT)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Machine toBeDeletedT)
        {
            throw new NotImplementedException();
        }

        public Task<List<Machine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Machine> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Machine toBeUpdatedT)
        {
            throw new NotImplementedException();
        }
    }
}
