using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string NotificationType { get; set; } = null!;

        //this is for the id of the notification (comment/post/friend)
        public int ContextId { get; set; }

        public bool IsRead { get; set; }
    }
}
