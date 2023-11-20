using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace capstone_backend.Controllers
{
	[Route("api/timeline")]
	[ApiController]
	public class TimelineController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		private readonly ApplicationDbContext _context;
		private readonly TimelineRepository _timelineRepository;


		public TimelineController(ApplicationDbContext context, UserRepository userRepository, TimelineRepository timelineRepository)
		{
			_context = context;
			_userRepository = userRepository;
			_timelineRepository = timelineRepository;
		}

		[HttpGet("get-all-posts/{userId}")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts(int userId)
		{ 
			Timeline? timeline = await _timelineRepository.GetTimelineByUserId(userId);

			if(timeline == null)
			{
				return BadRequest("user_invalid");
			}

			List<Post>? posts = await _timelineRepository.GetPostsByTimelineId(timeline.Id);

			if(posts == null)
			{
				return NotFound("no_posts_found");
			}

			return posts;
		}


		

	}


}
