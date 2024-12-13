namespace CoffeeCrazy.Repos
{
    public class NotificationRepo
    {
        private readonly string _connectionString;

        public NotificationRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //public void CreateNotification(Notification notification)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "INSERT INTO Notifications (Message, NotificationTypeId) VALUES (@Message, @NotificationTypeId)";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = notification.Message;

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Update an existing notification
        //public void UpdateNotification(Notification notification)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "UPDATE Notifications SET Message = @Message, NotificationTypeId = @NotificationTypeId WHERE NotificationId = @NotificationId";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = notification.NotificationId;
        //                command.Parameters.Add("@Message", SqlDbType.NVarChar).Value = notification.Message;

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Delete a notification
        //public void DeleteNotification(int notificationId)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "DELETE FROM Notifications WHERE NotificationId = @NotificationId";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = notificationId;
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Get all notifications
        //public List<Notification> GetAllNotifications()
        //{
        //    List<Notification> notifications = new List<Notification>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "SELECT NotificationId, Message, NotificationTypeId FROM Notifications";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    notifications.Add(new Notification
        //                    {
        //                        NotificationId = reader.GetInt32(0),
        //                        Message = reader.GetString(1),
        //                    });
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }

        //    return notifications;
        //}

        //// ------------------ Notify CRUD Operations ------------------

        //// Create a new Notify entry
        //public void CreateNotify(Notify notify)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "INSERT INTO Notifies (DateCreated, Status, AssignmentId, UserId, AssignmentSetId, NotificationId) VALUES (@DateCreated, @Status, @AssignmentId, @UserId, @AssignmentSetId, @NotificationId)";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = notify.DateCreated;
        //                command.Parameters.Add("@Status", SqlDbType.Bit).Value = notify.Status;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = notify.UserId;
        //                command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = notify.NotificationId;

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Update an existing Notify entry
        //public void UpdateNotify(Notify notify)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "UPDATE Notifies SET DateCreated = @DateCreated, Status = @Status, AssignmentId = @AssignmentId, UserId = @UserId, AssignmentSetId = @AssignmentSetId, NotificationId = @NotificationId WHERE NotificationId = @NotificationId";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = notify.DateCreated;
        //                command.Parameters.Add("@Status", SqlDbType.Bit).Value = notify.Status;
        //                command.Parameters.Add("@UserId", SqlDbType.Int).Value = notify.UserId;
        //                command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = notify.NotificationId;

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Delete a Notify entry
        //public void DeleteNotify(int notificationId)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "DELETE FROM Notifies WHERE NotificationId = @NotificationId";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.Parameters.Add("@NotificationId", SqlDbType.Int).Value = notificationId;
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        //// Get all Notify entries
        //public List<Notify> GetAllNotifies()
        //{
        //    List<Notify> notifies = new List<Notify>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            string sqlQuery = "SELECT DateCreated, Status, AssignmentId, UserId, AssignmentSetId, NotificationId FROM Notifies";

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    notifies.Add(new Notify
        //                    {
        //                        DateCreated = reader.GetDateTime(0),
        //                        Status = reader.GetBoolean(1),
        //                        TaskId = reader.GetInt32(2),
        //                        UserId = reader.GetInt32(3),
        //                        NotificationId = reader.GetInt32(5)
        //                    });
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error: " + ex.Message);
        //        }
        //    }

        //    return notifies;
        //}
    }

}

