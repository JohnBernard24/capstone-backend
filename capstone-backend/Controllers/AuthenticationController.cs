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
using System.Net;
using System.Net.Mail;

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
				return BadRequest(new { result = "invalid_passed_user_login_" });
			}

			User? user = await _userRepository.GetUserByEmail(userLoginDTO.Email);
			if(user == null)
			{
				return Unauthorized(new { result = "no_user_found" });
			}

			bool isCorrectPassword = _passwordHasher.VerifyPassword(userLoginDTO.Password, user.HashedPassword);
			if (!isCorrectPassword)
			{
				return Unauthorized(new { result = "invalid_credentials" });
			}

			var loginResponse = new LoginResponse
			{
				UserId = user.Id,
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
				return BadRequest(new { result = "invalid_user_registration" });
			}

			User? existingUserByEmail = await _userRepository.GetUserByEmail(userRegisterDTO.Email);
			if (existingUserByEmail != null)
			{
				return Conflict(new { result = "email_already_exist" });
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

			var newTimeline = new Timeline
			{
				UserId = newUser.Id,
				User = newUser
			};

			_context.TimeLine.Add(newTimeline);
			_context.SaveChanges();

			_userRepository.InsertUser(newUser);
			return Ok(new {result = "user_registered_successfully"});
		}

		[HttpPost("verify-email")]
		public async Task<IActionResult> SendEmail([FromBody] EmailVerifyDTO emailVerifyDTO)
		{
			var senderEmail = "teametivacpastebook@gmail.com";
			var senderPassword = "nbci cmzt wqds krbv";

			var message = new MailMessage(senderEmail, emailVerifyDTO.RecipientEmail)
			{
				Subject = $"Verify your email, {emailVerifyDTO.RecipientEmail}!",
				IsBodyHtml = true, 
				Body = $@"
					<html>
					<head>
						<title>Pastebook Email Verification</title>
					</head>
					<body>
						<h2>Hello! {emailVerifyDTO.RecipientEmail}</h2>
						<p>Click the button below to verify your email:</p>
						<a href=""http://localhost:4200/forgot-password/{emailVerifyDTO.UserId}"" style=""display:inline-block;padding:10px 20px;background-color:#007BFF;color:#ffffff;text-decoration:none;border-radius:5px;"">Confirm Email</a>
					</body>
					</html>"
			};

			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(senderEmail, senderPassword),
				EnableSsl = true,
			};

			try
			{
				await smtpClient.SendMailAsync(message);
				return Ok(new { result = "Email sent successfully!" });
			}
			catch (Exception)
			{
				return BadRequest(new { result = $"Error sending email." });
			}
		}
	}
}
