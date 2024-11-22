namespace CoffeeCrazy.Models
{
    public class AssignmentSet
    {

        /// <summary>
        /// This class is used to hold alot of assignments in a set of assignments
        /// </summary>
        public int AssignmentSetId { get ; set; }
        public DateTime Deadline { get; set; }
        public bool SetCompleted { get; set; }
        public Machine MachineId { get; set; }
    }
}
