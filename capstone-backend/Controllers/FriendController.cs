using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Asn1.Ocsp;

namespace capstone_backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FriendController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		private readonly FriendRepository _friendRepository;
		private readonly NotificationRepository _notificationRepository;

		public FriendController(UserRepository userRepository, FriendRepository friendRepository, NotificationRepository notificationRepository)
		{
			_userRepository = userRepository;
			_friendRepository = friendRepository;
			_notificationRepository = notificationRepository;
		}


		[HttpGet("get-all-friends/{userId}")]
		public async Task<ActionResult<IEnumerable<User>>> GetAllFriendsByUserId(int userId)
		{

			var friends = await _friendRepository.GetAllFriendsByUserId(userId);

			if(friends == null)
			{
				return BadRequest("no_friends_found");
			}
			return Ok(friends);
		}


		[HttpGet("get-all-friend-request/{userId}")]
		public async Task<ActionResult<IEnumerable<Friend>>> GetAllFriendRequests (int userId)
		{
			var friendRequests = await _friendRepository.GetFriendRequests(userId);

			if(friendRequests == null)
			{
				return BadRequest("no_friend_requests_found");
			}

			return Ok(friendRequests);
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


            var friendNotif = new Notification
            {
                NotificationType = "add-friend-request",
                NotifiedUserId = friendRequest.ReceiverId,
                NotifiedUser = friendRequest.Receiver,
                ContextId = friendRequest.Id,
                IsRead = false
            };

            _notificationRepository.InsertNotification(friendNotif);

			return Ok(new { result = "friend_request_successfully" });

		}

		[HttpPost("accept-friend/{requestId}")]
		public async Task<IActionResult> AcceptFriendRequest(int requestId)
		{
			var friendRequest = await _friendRepository.GetFriendRequestByFriendId(requestId);

			if (friendRequest == null)
			{
				return BadRequest("request_id_invalid");
			}

			if (friendRequest.isFriend)
			{
				return BadRequest("friend_request_already_accepted");
			}

			friendRequest.isFriend = true;
			friendRequest.FriendshipDate = DateTime.UtcNow;

			_friendRepository.UpdateFriend(friendRequest);


            var friendNotif = new Notification
            {
                NotificationType = "accept-friend-request",
                NotifiedUserId = friendRequest.SenderId,
                NotifiedUser = friendRequest.Sender,
                ContextId = friendRequest.Id,
                IsRead = false
            };

            _notificationRepository.InsertNotification(friendNotif);


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
