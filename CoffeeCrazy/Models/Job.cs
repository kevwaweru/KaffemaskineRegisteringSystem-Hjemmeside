namespace CoffeeCrazy.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Comment { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Deadline { get; set; }
        public int FrequencyId { get; set; }
        public int MachineId { get; set; }
        public int? UserId { get; set; }  
    }
}