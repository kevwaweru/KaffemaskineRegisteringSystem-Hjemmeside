﻿namespace CoffeeCrazy.Models
{
    public class Notify
    {
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }             
        public int UserId { get; set; }   
        public int JobId { get; set; }
        public int NotificationId { get; set; }

        public Notify(DateTime dateCreated, bool status, int userId, int jobId, int notificationId)
        {
            DateCreated = dateCreated;
            Status = status;
            UserId = userId;
            JobId = jobId;
            NotificationId = notificationId;
        }
    }
}
