using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public int? NotificationTypeId { get; set; }
         
    }
}
