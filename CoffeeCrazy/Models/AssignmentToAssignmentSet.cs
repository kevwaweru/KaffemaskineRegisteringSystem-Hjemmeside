using CoffeeCrazy.Model;

namespace CoffeeCrazy.Models
{
    public class AssignmentToAssignmentSet
    {
       public int AssignmentToAssignmentSetId { get; set; }

        public AssignmentSet AssignmentSetId { get; set; }
        public Assignment AssignmentId { get; set; }
    }
}
