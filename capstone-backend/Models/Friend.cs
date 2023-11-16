using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_backend.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RecieverId { get; set; }

        [Required]
        public int RequesterId { get; set; }

        [Required]
        public DateTime DateOfFriend { get; set; }

        [Required]
        public bool IsFriend { get; set; }


    }
}
