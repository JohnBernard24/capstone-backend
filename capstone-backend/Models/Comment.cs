using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public string CommentContext { get; set; } = null!;
    }
}
