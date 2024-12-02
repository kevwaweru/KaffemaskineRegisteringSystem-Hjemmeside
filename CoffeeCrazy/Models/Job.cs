namespace CoffeeCrazy.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Job
    {
        public int JobId { get; set; }
        public int JobTemplateId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public bool IsCompleted { get; set; } = false;
        public int MachineId { get; set; }
        public int FrequencyId { get; set; }
        public int UserId { get; set; }
    }
}
