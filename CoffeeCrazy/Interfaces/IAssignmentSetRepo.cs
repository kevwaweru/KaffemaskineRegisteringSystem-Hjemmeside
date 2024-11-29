using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    public interface IAssignmentSetRepo: ICRUDRepo<AssignmentSet>
    {
        Task<List<Assignment>> GetByAssignmentSetIdAsync(int assignmentSetId);
    }
}
