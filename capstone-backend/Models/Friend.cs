using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capstone_backend.Models
{
	public class Friend
	{
		public int Id { get; set; }

		public int ReceiverId { get; set; }

		public User Receiver { get; set; } = null!;

		public int SenderId { get; set; }

		public User Sender { get; set; } = null!;

		[DataType(DataType.Date)]
		public DateTime? FriendshipDate { get; set; }

		public bool isFriend { get; set; }


	}
}
