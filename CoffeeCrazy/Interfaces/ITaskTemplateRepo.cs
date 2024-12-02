using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    public interface ITaskTemplateRepo: ICRUDRepo<TaskTemplate>
    {
        Task<List<Models.Job>> GetByAssignmentSetIdAsync(int assignmentSetId);
    }
}
