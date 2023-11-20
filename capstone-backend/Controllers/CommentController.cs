using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace capstone_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{

		private readonly UserRepository _userRepository;
		private readonly PostRepository _postRepository;
		private readonly TimelineRepository _timelineRepository;
		private readonly CommentRepository _commentRepository;

		public CommentController(UserRepository userRepository, PostRepository postRepository, TimelineRepository timelineRepository, CommentRepository commentRepository)
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_timelineRepository = timelineRepository;
			_commentRepository = commentRepository;
		}

		[HttpPost("add-comment")]
		public async Task<IActionResult> AddComment([FromBody] CommentDTO commentDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_comment");
			}

			Post? post = await _postRepository.GetPostById(commentDTO.PostId);

			if (post == null)
			{
				return BadRequest("invalid_post_id");
			}

			Comment comment = new Comment
			{
				CommentContent = commentDTO.CommentContent,
				PostId = commentDTO.PostId,
				Post = post
			};


			return Ok(new { result = "comment_added"});
		}


		[HttpPut("update-comment/{commentId}")]
		public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentDTO commentDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_comment");
			}

			Comment? existingComment = await _commentRepository.GetCommentById(commentId);

			if (existingComment == null)
			{
				return NotFound("comment_not_found");
			}

			existingComment.CommentContent = commentDTO.CommentContent;


			_commentRepository.UpdateComment(existingComment);

			return Ok(existingComment);
		}

		[HttpDelete("delete-comment/{commentId}")]
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			Comment? existingComment = await _commentRepository.GetCommentById(commentId);

			if (existingComment == null)
			{
				return NotFound("comment_not_found");
			}

			_commentRepository.DeleteComment(existingComment);

			return Ok(new { result = "comment_deleted" });
		}

	}
}
