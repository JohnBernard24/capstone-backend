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
	[Route("api/post")]
	[ApiController]
	public class PostController : Controller
	{
		private readonly UserRepository _userRepository;
		private readonly ApplicationDbContext _context;
		private readonly PostRepository _postRepository;


		public PostController(ApplicationDbContext context, UserRepository userRepository, PostRepository postRepository)
		{
			_context = context;
			_userRepository = userRepository;
			_postRepository = postRepository;
		}

		[HttpPost("add-post/{userId}")]
		public IActionResult AddPost(int userId, [FromBody] PostAddDTO postAddDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_post");
			}


			User? poster = _context.User?.FirstOrDefault(u => u.Id == postAddDTO.PosterId);
			if(poster == null)
			{
				return BadRequest("invalid_user_id");
			}
			var timeline = _context.TimeLine?.FirstOrDefault(t => t.UserId == userId);
			if (timeline == null)
			{
				return BadRequest("timeline_not_found");
			}

			Post post = new Post
			{
				PostTitle = postAddDTO.PostTitle,
				Description = postAddDTO.Description,
				DatePosted = postAddDTO.DatePosted,
				TimelineId = timeline.Id,
				Timeline = timeline,
				PhotoId = postAddDTO.Photo?.Id,
				Photo = postAddDTO.Photo,
				PosterId = poster.Id,
				Poster = poster
			};

			_postRepository.InsertPost(post);

			return Ok(new { result = "post_added_successfully" });

		}


		


	}
}
