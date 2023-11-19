using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string CommentContent { get; set; } = null!;
		public DateTime DateCommented { get; set; } = DateTime.Now;


		public int PostId { get; set; }
		public Post? Post { get; set; }
		public int UserId { get; set; }
		public User? Commenter { get; set; }

	}

	public class CommentDTO
	{
		public string CommentContent { get; set; } = null!;
		public DateTime DateCommented { get; set; } = DateTime.Now;

		public int PostId { get; set; }
	}
}
