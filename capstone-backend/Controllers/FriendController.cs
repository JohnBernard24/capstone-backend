using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Asn1.Ocsp;

namespace capstone_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FriendController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		private readonly PostRepository _postRepository;
		private readonly TimelineRepository _timelineRepository;
		private readonly FriendRepository _friendRepository;

		public FriendController(UserRepository userRepository, PostRepository postRepository, TimelineRepository timelineRepository, FriendRepository friendRepository)
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_timelineRepository = timelineRepository;
			_friendRepository = friendRepository;
		}


		[HttpGet("get-all-friends/{userId}")]
		public async Task<ActionResult<IEnumerable<User>>> GetAllFriendsByUserId(int userId)
		{

			var friends = await _friendRepository.GetAllFriendsByUserId(userId);
			return friends;
		}

		[HttpPost("add-friend")]
		public async Task<IActionResult> AddFriendRequest([FromBody] FriendDTO friendDTO)
		{
			User? receiver = await _userRepository.GetUserById(friendDTO.ReceiverId);

			if(receiver == null)
			{
				return BadRequest("receiver_not_valid");
			}

			User? sender = await _userRepository.GetUserById(friendDTO.SenderId);

			if(sender == null)
			{
				return BadRequest("sender_not_valid");
			}

			var exisitingRequest = await _friendRepository.FriendRequestExists(friendDTO.SenderId, friendDTO.ReceiverId);

			if (exisitingRequest)
			{
				return BadRequest("request_already_exists");
			}

			var friendRequest = new Friend
			{
				SenderId = friendDTO.SenderId,
				Sender = sender,
				ReceiverId = friendDTO.ReceiverId,
				Receiver = receiver,
				FriendshipDate = null
			};

			_friendRepository.InsertFriend(friendRequest);

			return Ok(new { result = "friend_request_successfully" });

		}

		[HttpPost("accept-friend/{requestId}")]
		public async Task<IActionResult> AcceptFriendRequest(int requestId)
		{
			var friendRequest = await _friendRepository.GetFriendRequestByFriendId(requestId);

			if (friendRequest == null)
			{
				return BadRequest("friend_id_invalid");
			}

			if (friendRequest.isFriend)
			{
				return BadRequest("friend_already_accepted");
			}

			friendRequest.isFriend = true;
			friendRequest.FriendshipDate = DateTime.UtcNow;

			_friendRepository.UpdateFriend(friendRequest);

			return Ok(new { result = "friend_request_accepted" });
		}


		[HttpDelete("reject-friend/{requestId}")]
		public async Task <IActionResult> RejectFriendRequest(int requestId)
		{
			var friendRequest = await _friendRepository.GetFriendRequestByFriendId(requestId);

			if (friendRequest == null)
			{
				return BadRequest("friend_request_id_invalid");
			}

			_friendRepository.DeleteFriend(friendRequest);

			return Ok(new { result = "friend_request_rejected"});

		}
	}
}
