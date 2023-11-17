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
		public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_passed_user_login_");
			}

			User? user = await _userRepository.GetUserByEmail(userLoginDTO.Email);
			if(user == null)
			{
				return Unauthorized("no_user_found");
			}

			bool isCorrectPassword = _passwordHasher.VerifyPassword(userLoginDTO.Password, user.HashedPassword);
			if (!isCorrectPassword)
			{
				return Unauthorized("invalid_credentials");
			}

			var loginResponse = new LoginResponse
			{
				Email = user.Email,
				Token = "haha"
			};

			return Ok(loginResponse);

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
