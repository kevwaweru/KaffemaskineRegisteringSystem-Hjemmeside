namespace CoffeeCrazy.Models
{
    public class AssignmentJunctionDTO
    {
        public int AssignmentSetId { get; set; }
        public int AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; }
        public string? AssignmentComment { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Deadline { get; set; }
        public bool SetCompleted { get; set; }
        public int MachineId { get; set; }

    }
}
