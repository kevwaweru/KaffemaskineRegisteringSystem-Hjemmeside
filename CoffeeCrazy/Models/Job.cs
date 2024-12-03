using System.ComponentModel.DataAnnotations;

namespace CoffeeCrazy.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Job
    {
        public int JobId { get; set; }
        [Required]
        public int JobTemplateId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline {  get; set; }
        public bool IsCompleted { get; set; }
        [Required]
        public int MachineId { get; set; }
        [Required]
        public int FrequencyId { get; set; }
        public int? UserId { get; set; }
        
    }
}
