namespace capstone_backend.AuthenticationService.Models
{
	public class AccessToken
	{
		public Guid Id { get; set; }
		public string? Token { get; set; }
		public int UserId { get; set; }
	}
}
