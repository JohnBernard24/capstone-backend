using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using NuGet.Protocol.Plugins;


namespace capstone_backend.Controllers
{
	[Route("api/authentication")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		private readonly ApplicationDbContext _context;
		private readonly BcryptPasswordHasher _passwordHasher;

		public AuthenticationController(ApplicationDbContext context, UserRepository userRepository, BcryptPasswordHasher passwordHasher)
		{
			_context = context;
			_userRepository = userRepository;
			_passwordHasher = passwordHasher;
		}


		[HttpPost("login")]
		public ActionResult<LoginResponse> Login(UserLoginDTO userLoginDTO)
		{
			try
			{
				var user = _context.User.FirstOrDefault(u => u.Email == userLoginDTO.Email);

				if (user == null)
				{
					return NotFound(new { result = "user_not_found" });
				}

				bool isCorrectPassword = _passwordHasher.VerifyPassword(userLoginDTO.Password, user.HashedPassword);
				if (!isCorrectPassword)
				{
					return Unauthorized();
				}


				var loginResponse = new LoginResponse
				{
					Email = user.Email,
					Token = "haha"
				};

				return Ok(loginResponse);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Error retrieving log in credentials: " + ex.Message);
				return StatusCode(500, "An error occurred while retrieving log in credentials.");
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_passed_user_registration");
			}

			//need to add user repository
			User? existingUserByEmail = await _userRepository.GetUserByEmail(userRegisterDTO.Email);
			if (existingUserByEmail != null)
			{
				return Conflict("email_already_exist");
			}

			var newUser = new User
			{
				FirstName = userRegisterDTO.FirstName,
				LastName = userRegisterDTO.LastName,
				Email = userRegisterDTO.Email,
				HashedPassword = _passwordHasher.HashPassword(userRegisterDTO.Password),
				BirthDate = userRegisterDTO.BirthDate,
				Sex = userRegisterDTO.Sex,
				PhoneNumber = userRegisterDTO.PhoneNumber
			};

			_userRepository.InsertUser(newUser);
			return Ok(new {result = "user_registered_successfully"});
		}
		
	}
}
