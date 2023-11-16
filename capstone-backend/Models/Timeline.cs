using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Timeline
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}
