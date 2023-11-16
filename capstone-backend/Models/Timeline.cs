using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
    public class Timeline
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
