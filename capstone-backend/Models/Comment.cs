using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; } = null!;
		public string CommentContent { get; set; } = null!;
	}
}
