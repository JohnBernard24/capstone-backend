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
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly capstone_backendContext _context;
		private readonly BcryptPasswordHasher _passwordHasher;

		public UsersController(capstone_backendContext context, BcryptPasswordHasher passwordHasher)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}

		[HttpPost("login")]
		public ActionResult<LoginResponse> Login(UserloginDTO userLoginDto)
		{
			try
			{
				var user = _context.User.FirstOrDefault(u => u.Email == userLoginDto.Email);

				if (user == null)
				{
					return NotFound(new { result = "user_not_found" });
				}

				bool isCorrectPassword = _passwordHasher.VerifyPassword(userLoginDto.Password, user.PasswordHash);
				if (!isCorrectPassword)
				{
					return Unauthorized();
				}


				var loginResponse = new LoginResponse
				{
					email = user.Email,	
					token = 123
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
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            return await _context.User.ToListAsync();
        }



        private bool UserExists(Guid id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
	}

}
