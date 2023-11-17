namespace capstone_backend.Models
{
	public class Timeline
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
