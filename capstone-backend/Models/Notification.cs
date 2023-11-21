using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Notification
    {
        public int Id { get; set; }

        //Change UserId to "NotifiedUserId" and User to "NotifiedUser"
        public int NotifiedUserId { get; set; }
        public User? NotifiedUser { get; set; } 

        public string NotificationType { get; set; } = null!;

        //this is for the id of the notification (comment/post/friend)
        public int ContextId { get; set; }

        public bool IsRead { get; set; }
    }


    public class NotificationContextDTO
    {

    }
}
