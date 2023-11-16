using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string NotificationType { get; set; } = null!;

        [Required]
        public int ContextId { get; set; }

        [Required]
        public bool IsRead { get; set; }
    }
}
