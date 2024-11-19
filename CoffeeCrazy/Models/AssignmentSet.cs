using CoffeeCrazy.Model;

namespace CoffeeCrazy.Models
{
    public class AssignmentSet
    {
        public int AssignmentSetId { get ; set; }
        public DateTime Deadline { get; set; }
        public bool SetCompleted { get; set; }

        public Machine MachineId { get; set; }
    }
}
