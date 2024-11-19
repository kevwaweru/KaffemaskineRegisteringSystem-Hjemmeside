using CoffeeCrazy.Model;

namespace CoffeeCrazy.Models
{
    public class Notify
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;      
        public bool Status { get; set; }             
        public int AssignmentId { get; set; }         
        public int UserId { get; set; }               
        public int AssignmentSetId { get; set; }      
        public int NotificationId { get; set; }       
    }
}
