using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_backend.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public int RecieverId { get; set; }

        public int RequesterId { get; set; }

        public DateTime DateOfFriend { get; set; }

        public bool IsFriend { get; set; }


    }
}
