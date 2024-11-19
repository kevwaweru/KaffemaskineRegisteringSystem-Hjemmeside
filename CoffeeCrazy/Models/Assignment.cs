namespace CoffeeCrazy.Model
{
    public class Assignment
    {

        /// <summary>
        /// This Class defines what a Assigment is
        /// </summary>
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string? Comment { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now; 
        public bool IsCompleted { get; set; } = false;




        
    }
}
