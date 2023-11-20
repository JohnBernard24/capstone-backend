using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Like
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public Post? Post { get; set; }
		public int LikerId { get; set; }
		public User? Liker { get; set; }
	}

	public class LikeDTO
	{
		public int PostId { get; set; }
		public int LikerId { get; set; }
	}

	public class LikeViewResponse
	{
		public Post? Post { get; set; }
		public User? Liker { get; set; }
	}
}
