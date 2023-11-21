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
		private readonly FriendRepository _friendRepository;
		private readonly PostRepository _postRepository;


		public TimelineController(ApplicationDbContext context, UserRepository userRepository, TimelineRepository timelineRepository, FriendRepository friendRepository, PostRepository postRepository)
		{
			_context = context;
			_userRepository = userRepository;
			_timelineRepository = timelineRepository;
			_friendRepository = friendRepository;
			_postRepository = postRepository;
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

		[HttpGet("get-newsfeed-posts/{userId}")]
		public async Task<ActionResult<IEnumerable<Post>>> GetAllNewsfeedPostsByUserId(int userId)
		{
			List<Friend> friends = await _friendRepository.GetAllFriendsObjectByUserId(userId);

			if(friends == null)
			{
				return BadRequest("no_friends_found");
			}

			List<Post> friendsPosts = new List<Post>();
			
			foreach(Friend friend in friends)
			{
				List<Post> individualPost = await _postRepository.GetAllPostsByUserId(friend.SenderId);

				foreach(Post post in individualPost)
				{
					friendsPosts.Add(post);
				}
			}

			List<Post> usersPosts = await _postRepository.GetAllPostsByUserId(userId);

			List<Post> allPosts = friendsPosts.Concat(usersPosts).ToList();

			allPosts = allPosts.OrderByDescending(post => post.DatePosted).ToList();

			return allPosts;
		}

		

	}


}
