using CoffeeCrazy.Model;

namespace CoffeeCrazy.Interfaces
{
    public interface IAssignmentRepo : ICRUDRepo<Assignment>
    {
        Task<List<Assignment>> GetByAssignmentSetIdAsync(int assignmentSetId);
    }
}
