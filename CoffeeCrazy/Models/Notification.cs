using CoffeeCrazy.Models.Enums;

namespace CoffeeCrazy.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public Notification(int notificationId, string message, NotificationType notificationType)
        {
            NotificationId = notificationId;
            Message = message;
            NotificationType = notificationType;
        }



    }
}
