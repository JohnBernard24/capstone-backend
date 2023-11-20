using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string PostTitle { get; set; } = null!;
		public string Description { get; set; } = null!;
		public DateTime DatePosted { get; set; } = DateTime.Now;


		public int? TimelineId { get; set; }
		public Timeline? Timeline { get; set; }
		public int? PhotoId { get; set; }
		public Photo? Photo { get; set; }
		public int PosterId { get; set; }
		public User? Poster { get; set; }
	}

	public class PostDTO
	{
		[Required]
		public string PostTitle { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;



		public Photo? Photo { get; set; }
		public int PosterId { get; set; }
	}

	public class PostViewResponse
	{
		public int PostId { get; set; }
		public string PostTitle { get; set; } = null!;
		public string Description { get; set; } = null!;
		public DateTime DatePosted { get; set; }
		public Photo? Photo { get; set; }
		public User? Poster { get; set; }
		public Timeline? Timeline { get; set; }

	}
}
