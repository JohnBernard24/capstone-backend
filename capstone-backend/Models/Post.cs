using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string PostTitle { get; set; } = null!;
		public int PhotoId { get; set; }
		public Photo Photo { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int TimelineId { get; set; }
		public Timeline Timeline { get; set; } = null!;
		public int UserId { get; set; }
		public User? User { get; set; }
		[DataType(DataType.Date)]
		public DateTime DatePosted { get; set; }
	}
}
