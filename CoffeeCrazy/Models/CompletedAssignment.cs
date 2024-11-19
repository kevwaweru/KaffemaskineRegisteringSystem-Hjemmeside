using System.Reflection.PortableExecutable;

namespace CoffeeCrazy.Models
{
    public class CompletedAssignment
    {
        public int CompletedAssignmentId { get; set; }
        public DateTime DateCompleted { get; set; }
        public int AssignmentSetId { get; set;  }

        public int CompletedByUserId { get; set; }

    }
}
