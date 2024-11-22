namespace CoffeeCrazy.Models
{
    /// <summary>
    /// This class is used to hold alot of assignments in a set of assignments
    /// </summary>
    public class AssignmentSet
    {    
        public int AssignmentSetId { get ; set; }
        public DateTime Deadline { get; set; }
        public bool SetCompleted { get; set; }

        public int MachineId { get; set; }
        public int AssignmentId { get; set; }
    }
}
