namespace CoffeeCrazy.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Job
    {
        public int TaskId { get; set; }
        public int TaskTemplateId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime Deadline {  get; set; }
        public bool IsCompleted { get; set; } = false;
        public int MachineId { get; set; }
        public int FrequencyId { get; set; }


    }
}
