using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PostTitle { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }
    }
}
