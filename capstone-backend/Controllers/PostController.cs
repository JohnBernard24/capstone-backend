using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Controllers
{
	[Route("api/post")]
	[ApiController]
	public class PostController : Controller
	{
		private readonly UserRepository _userRepository;
		private readonly PostRepository _postRepository;
		private readonly TimelineRepository _timelineRepository;
		private readonly NotificationRepository _notificationRepository;
		private readonly PhotoRepository _photoRepository;

		public PostController(UserRepository userRepository, PostRepository postRepository, TimelineRepository timelineRepository, NotificationRepository notificationRepository, PhotoRepository photoRepository )
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_timelineRepository = timelineRepository;
			_notificationRepository = notificationRepository;
			_photoRepository = photoRepository;
		}


		//*******************CRUD FUNCTION START******************************//
		[HttpPost("add-post/{userId}")]
		public async Task<IActionResult> AddPost(int userId, [FromBody] PostDTO postDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_post" });
			}

			User? poster = await _userRepository.GetUserById(postDTO.PosterId);
			if (poster == null)
			{
				return BadRequest(new { result = "invalid_user_id" });
			}

			Timeline? timeline = await _timelineRepository.GetTimelineByUserId(userId);
			if (timeline == null)
			{
				return BadRequest(new { result = "timeline_not_found" });
			}

			Photo? photo = await _photoRepository.GetPhotoById(postDTO.PhotoId);

			Post post = new Post
			{
				PostTitle = postDTO.PostTitle,
				Description = postDTO.Description,
				TimelineId = timeline.Id,
				Timeline = timeline,
				PhotoId = postDTO.PhotoId,
				Photo = photo,
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
				return BadRequest(new { result = "invalid_post" });
			}

			Post? existingPost = await _postRepository.GetPostById(postId);

			if (existingPost == null)
			{
				return NotFound(new { result = "post_not_found" });
			}

			existingPost.PostTitle = postDTO.PostTitle;
			existingPost.Description = postDTO.Description;
			Photo? photo = await _photoRepository.GetPhotoById(postDTO.PhotoId);
			existingPost.Photo = photo;

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
				return NotFound(new { result = "post_not_found" });
			}

			_postRepository.DeletePost(existingPost);

			return Ok(new { result = "post_deleted" });
		}
		//*******************CRUD FUNCTION END******************************//



		//*******************GETTERS FUNCTION START******************************//
		[HttpGet("get-post-likes/{postId}")]
		public async Task<ActionResult<IEnumerable<MiniProfileDTO>>> GetPostLikesByPostId(int postId)
		{
			List<Like> likes = await _postRepository.GetLikesByPostId(postId);

			if(likes == null)
			{
				return NotFound(new { result = "no_post_likes_found" });
			}

			List<MiniProfileDTO?> users = new List<MiniProfileDTO?>();

			foreach(Like like in likes)
			{
				User? user = await _userRepository.GetUserById(like.LikerId);

				MiniProfileDTO MiniProfileDTO = new MiniProfileDTO
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Photo = user.Photo
				};

				users.Add(MiniProfileDTO);
			}

			if(users.Count == 0)
			{
				return NotFound(new { result = "no_likers_found" });
			}

			return Ok(users);
		}
		//*******************GETTERS FUNCTION END******************************//


		//*******************LIKE FUNCTION START******************************//
		[HttpPost("like-post")]
		public async Task<IActionResult> LikePost([FromBody] LikeDTO likeDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_like_to_post" });
			}

			Post? post = await _postRepository.GetPostById(likeDTO.PostId);
			if (post == null)
			{
				return NotFound(new { result = "post_not_found" });
			}

			User? liker = await _userRepository.GetUserById(likeDTO.LikerId);

			if (liker == null)
			{
				return NotFound(new { result = "user_not_found" });
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

				var likeNotif = new Notification
				{
					NotificationType = "like",
					NotifiedUserId = post.PosterId,
					NotifiedUser = post.Poster,
					ContextId = like.Id,
					IsRead = false
				};

				_notificationRepository.InsertNotification(likeNotif);

				return Ok(new { result = "like_added" });
			}

			_postRepository.RemoveLike(existingLike);
			return Ok(new { result = "like_deleted" });

		}
		//*******************LIKE FUNCTION END******************************//




	}
}
