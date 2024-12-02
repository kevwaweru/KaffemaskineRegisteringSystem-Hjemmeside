namespace CoffeeCrazy.Models
{
    /// <summary>
    /// This class is used to hold alot of assignments in a set of assignments
    /// </summary>
    public class JobTemplate
    {    
        public int JobTemplateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskTemplateId { get; internal set; }
    }
}
