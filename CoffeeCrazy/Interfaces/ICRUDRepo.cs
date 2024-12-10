using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    public interface ICRUDRepo<T>
    {
        //Create
        Task CreateAsync(T toBeCreatedT);

        //Read
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        //Update
        Task UpdateAsync(T toBeUpdatedT);

        //Delete
        Task DeleteAsync(T toBeDeletedT);
        
    }
}
