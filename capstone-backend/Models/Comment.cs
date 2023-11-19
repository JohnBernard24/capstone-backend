using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public string CommentContent { get; set; } = null!;
		public DateTime DateCommented { get; set; } = DateTime.Now;


		public int PostId { get; set; }
		public Post Post { get; set; } = null!;
	}
}
