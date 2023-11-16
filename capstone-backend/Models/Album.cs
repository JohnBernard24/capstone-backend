using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        public string AlbumName { get; set; } = null!;

        [Required]
        public string ThumbnailUrl { get; set; } = null!;
    }
}
