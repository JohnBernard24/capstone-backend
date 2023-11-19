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
		private readonly PostRepository _postRepository;
		private readonly TimelineRepository _timelineRepository;

		public PostController(UserRepository userRepository, PostRepository postRepository, TimelineRepository timelineRepository)
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_timelineRepository = timelineRepository;
		}

		[HttpPost("add-post/{userId}")]
		public async Task<IActionResult> AddPost(int userId, [FromBody] PostDTO postDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_post");
			}

			User? poster = await _userRepository.GetUserById(postDTO.PosterId);
			if (poster == null)
			{
				return BadRequest("invalid_user_id");
			}

			Timeline? timeline = await _timelineRepository.GetTimelineByUserId(userId);
			if (timeline == null)
			{
				return BadRequest("timeline_not_found");
			}

			Post post = new Post
			{
				PostTitle = postDTO.PostTitle,
				Description = postDTO.Description,
				TimelineId = timeline.Id,
				Timeline = timeline,
				PhotoId = postDTO.Photo?.Id,
				Photo = postDTO.Photo,
				PosterId = poster.Id,
				Poster = poster
			};

			_postRepository.InsertPost(post);

			var postResponse = new PostViewResponse
			{
				PostId = post.Id,
				PostTitle = post.PostTitle,
				Description = post.Description,
				DatePosted = DateTime.Now,
				Photo = post.Photo,
				Poster = post.Poster,
				Timeline = post.Timeline
			};

			return Ok(postResponse);

		}


		[HttpPut("update-post/{postId}")]
		public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDTO postDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_post");
			}

			Post? existingPost = await _postRepository.GetPostById(postId);

			if (existingPost == null)
			{
				return NotFound("post_not_found");
			}

			existingPost.PostTitle = postDTO.PostTitle;
			existingPost.Description = postDTO.Description;
			existingPost.Photo = postDTO.Photo;

			_postRepository.UpdatePost(existingPost);

			var postResponse = new PostViewResponse
			{
				PostId = existingPost.Id,
				PostTitle = existingPost.PostTitle,
				Description = existingPost.Description,
				DatePosted = existingPost.DatePosted,
				Photo = existingPost.Photo,
				Poster = existingPost.Poster,
				Timeline = existingPost.Timeline
			};

			return Ok(postResponse);
		}

		
		[HttpDelete("delete-post/{postId}")]
		public async Task<IActionResult> DeletePost(int postId)
		{
			Post? existingPost = await _postRepository.GetPostById(postId);

			if (existingPost == null)
			{
				return NotFound("post_not_found");
			}

			_postRepository.DeletePost(existingPost);

			return Ok(new {result = "post_deleted"});
		}


		[HttpPost("like-post")]
		public async Task<IActionResult> LikePost([FromBody] LikeDTO likeDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_like_to_post");
			}

			Post? post = await _postRepository.GetPostById(likeDTO.PostId);
			if(post == null)
			{
				return NotFound("post_not_found");
			}

			User? liker = await _userRepository.GetUserById(likeDTO.LikerId);

			if(liker == null)
			{
				return NotFound("user_not_found");
			}

			Like? existingLike = await _postRepository.getLikeByPostIdAndUserId(likeDTO.PostId, likeDTO.LikerId);

			if (existingLike == null)
			{
				Like like = new Like
				{
					PostId = likeDTO.PostId,
					Post = post,
					LikerId = likeDTO.LikerId,
					Liker = liker
				};


				_postRepository.InsertLike(like);

				return Ok("like_added");
			}

			_postRepository.RemoveLike(existingLike);
			return Ok(new { result = "like_deleted" });

		}

	}
}
