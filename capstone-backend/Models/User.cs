using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class User
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string PasswordHash { get; set; } = null!;

		public DateTime Birthdate { get; set; }

		public string Sex { get; set; } = null!;

		public string MobileNumber { get; set; }

		public int? ProfileImageId { get; set; }
		public Photo? Photo { get; set; }

		public string AboutMe { get; set; } = null!;

	}

	public class UserloginDTO
	{
		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}

	public class LoginResponse
	{
		public string? email { get; set; }
		public bool? isAdmin { get; set; }
		public int? token { get; set; }
	}
}
