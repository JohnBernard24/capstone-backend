using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string HashedPassword { get; set; } = null!;

		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }
		public string? Sex { get; set; }
		public string? PhoneNumber { get; set; }
		public string? AboutMe { get; set; }



		public int? ProfileImageId { get; set; }
		public Photo? Photo { get; set; }
	}

	public class UserRegisterDTO
	{
		[Required]
		public string FirstName { get; set; } = null!;

		[Required]
		public string LastName { get; set; } = null!;

		[Required(ErrorMessage = "Email address is required")]
		[EmailAddress(ErrorMessage = "Email provided is not valid!")]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		[Required]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }

		public string? Sex { get; set; }

		public string? PhoneNumber { get; set; }
	}

	public class UserLoginDTO
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;
	}


	public class LoginResponse
	{
		public string? Email { get; set; }
		public string? Token { get; set; }
	}
}
