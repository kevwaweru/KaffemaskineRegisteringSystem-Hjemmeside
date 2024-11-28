using CoffeeCrazy.Models;

namespace CoffeeCrazy.Interfaces
{
    public interface IAssignmentJunctionRepo
    {
        //CRUD - Create.
        Task AddAssignmentToAssignmentSetAsync(int assignmentSetId, List<int> assignmentId);

        //CRUD - Read ALL assignment(s) in AssignmentSet.
        Task GetAllObjectsFromAssignmentJunctionsAsync(int assignmentSetId);

        //CRUD - Update one Assignment in an AssignmentSet in AssignmentJunction.
        Task UpdateAssignmentAsync(int assignmentSetId, int oldAssignmentId, int newAssignmentId);

        //CRUD - Delete an AssignmentJunction.
        Task DeleteAsync(int assignmentId, int assignmentSetId);

    }
}
