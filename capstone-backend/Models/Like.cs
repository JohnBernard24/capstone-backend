using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
