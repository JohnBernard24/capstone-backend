using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace capstone_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{

		private readonly PostRepository _postRepository;
		private readonly CommentRepository _commentRepository;
		private readonly NotificationRepository _notificationRepository;
		private readonly UserRepository _userRepository;

		public CommentController(UserRepository userRepository, PostRepository postRepository, TimelineRepository timelineRepository, CommentRepository commentRepository, NotificationRepository notificationRepository)
		{
			_postRepository = postRepository;
			_commentRepository = commentRepository;
			_notificationRepository = notificationRepository;
			_userRepository = userRepository;
		}


		//*******************CRUD FUNCTION START******************************//
		[HttpPost("add-comment")]
		public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_comment" });
			}

			Post? post = await _postRepository.GetPostById(commentDTO.PostId);

			if (post == null)
			{
				return BadRequest(new { result = "invalid_post_id" });
			}

			User? commenter = await _userRepository.GetUserById(commentDTO.CommenterId);

			Comment comment = new Comment
			{
				CommentContent = commentDTO.CommentContent,
				PostId = commentDTO.PostId,
				Post = post,
				CommenterId = commentDTO.CommenterId,
				Commenter = commenter
			};

			_commentRepository.InsertComment(comment);

			commentDTO.Id = comment.Id;
			
			
			Notification commentNotif = new Notification
			{
				NotificationType = "comment",
				NotifiedUserId = comment.Post.PosterId,
				NotifiedUser = comment.Post.Poster,
				ContextId = comment.Id,
				IsRead = false
			};

			_notificationRepository.InsertNotification(commentNotif);

			return Ok(commentDTO);
		}

		[HttpPut("update-comment")]
		public async Task<IActionResult> UpdateComment([FromBody] CommentDTO commentDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_comment" });
			}

			Comment? existingComment = await _commentRepository.GetCommentById(commentDTO.Id);

			if (existingComment == null)
			{
				return NotFound(new { result = "comment_not_found" });
			}

			existingComment.CommentContent = commentDTO.CommentContent;

			_commentRepository.UpdateComment(existingComment);

			commentDTO.DateCommented = existingComment.DateCommented;

			return Ok(commentDTO);
		}

		[HttpDelete("delete-comment/{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			Comment? existingComment = await _commentRepository.GetCommentById(commentId);

			if (existingComment == null)
			{
				return NotFound(new { result = "comment_not_found" });
			}

			_commentRepository.DeleteComment(existingComment);

			return Ok(new { result = "comment_deleted" });
		}
		//*******************CRUD FUNCTION END******************************//


		//*******************GETTERS FUNCTION START******************************//
		[HttpGet("get-post-comments/{postId}")]
		public async Task<IActionResult> GetCommentsByPostId(int postId)
		{
			Post? post = await _postRepository.GetPostById(postId);

			if(post == null)
			{
				return NotFound(new { result = "invalid_post_id" });
			}

			List<Comment> commentList = await _commentRepository.GetAllCommentsByPostId(post.Id);

			if(commentList == null)
			{
				return NotFound(new { result = "no_comments_found" });
			}

			List<CommentDTO> commentDTOList = new List<CommentDTO>();
			foreach(Comment comment in commentList)
			{
				commentDTOList.Add(new CommentDTO
				{
					Id = comment.Id,
					CommentContent = comment.CommentContent,
					DateCommented = comment.DateCommented,
					PostId = comment.PostId,
					CommenterId = comment.CommenterId
				});
			}

			return Ok(commentDTOList);
		}
		//*******************GETTERS FUNCTION END******************************//


	}
}
