using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Notification
	{
		public int Id { get; set; }

		public int NotifiedUserId { get; set; }
		public User? NotifiedUser { get; set; } 

		public string NotificationType { get; set; } = null!;

		//this is for the id of the notification (comment/post/friend)
		public int ContextId { get; set; }

		public bool IsRead { get; set; }
	}

	public class NotificationDTO
	{
		public int? Id { get; set; }
		public int NotifiedUserId { get; set; }
		public MiniProfileDTO? NotifiedUser { get; set; }
		public string NotificationType { get; set; } = null!;
		public int ContextId { get; set; }
		public bool IsRead { get; set; }
	}

}
