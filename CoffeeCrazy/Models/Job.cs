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

        public Job(int jobId, string title, string description, string? comment, bool isCompleted, DateTime dateCreated, DateTime deadline, int frequencyId, int machineId, int? userId)
        {
            JobId = jobId;
            Title = title;
            Description = description;
            Comment = comment;
            IsCompleted = isCompleted;
            DateCreated = dateCreated;
            Deadline = deadline;
            FrequencyId = frequencyId;
            MachineId = machineId;
            UserId = userId;
        }
    }
}