using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }

        [Required]
        public string PhotoUrl { get; set; } = null!;

        [Required]
        public int AlbumId { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }
    }
}
