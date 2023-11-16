using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string PostTitle { get; set; } = null!;

        public int photoId { get; set; }
        public Photo photo { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
