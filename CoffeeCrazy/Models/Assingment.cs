namespace CoffeeCrazy.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Assignment
    {
        public int AssignmentId { get; set; }        public string Title { get; set; }
        public string? Comment { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now; 
        public bool IsCompleted { get; set; } = false;
    }
}
