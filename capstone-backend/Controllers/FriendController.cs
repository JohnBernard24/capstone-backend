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
	[Route("api/friend")]
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

		//*******************CRUD FUNCTION START******************************//
		[HttpPost("add-friend")]
		public async Task<IActionResult> AddFriendRequest([FromBody] FriendDTO friendDTO)
		{
			User? receiver = await _userRepository.GetUserById(friendDTO.ReceiverId);

			if (receiver == null)
			{
				return BadRequest(new { result = "receiver_not_valid" });
			}

			User? sender = await _userRepository.GetUserById(friendDTO.SenderId);

			if (sender == null)
			{
				return BadRequest(new { result = "sender_not_valid" });
			}

			bool exisitingRequest = await _friendRepository.FriendRequestExists(friendDTO.SenderId, friendDTO.ReceiverId);

			if (exisitingRequest)
			{
				return BadRequest(new { result = "request_already_exists" });
			}

			Friend friendRequest = new Friend
			{
				SenderId = friendDTO.SenderId,
				Sender = sender,
				ReceiverId = friendDTO.ReceiverId,
				Receiver = receiver,
				FriendshipDate = null
			};

			_friendRepository.InsertFriend(friendRequest);

			Notification friendNotif = new Notification
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
			Friend? friendRequest = await _friendRepository.GetFriendRequestByFriendId(requestId);

			if (friendRequest == null)
			{
				return BadRequest(new { result = "request_id_invalid" });
			}

			if (friendRequest.isFriend)
			{
				return BadRequest(new { result = "friend_request_already_accepted" });
			}

			friendRequest.isFriend = true;
			friendRequest.FriendshipDate = DateTime.UtcNow;

			_friendRepository.UpdateFriend(friendRequest);


			Notification friendNotif = new Notification
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
		public async Task<IActionResult> RejectFriendRequest(int requestId)
		{
			Friend? friendRequest = await _friendRepository.GetFriendRequestByFriendId(requestId);

			if (friendRequest == null)
			{
				return BadRequest(new { result = "friend_request_id_invalid" });
			}

			_friendRepository.DeleteFriend(friendRequest);

			Notification? notification = await _notificationRepository.GetNotificationByContextIdAndNotificationType(requestId, "add-friend-request");

			if (notification == null)
			{
				return NotFound(new { result = "notification_not_found" });
			}

			_notificationRepository.DeleteNotification(notification);

			return Ok(new { result = "friend_request_rejected" });
		}
		//*******************CRUD FUNCTION END******************************//



		//*******************GETTERS FUNCTION START******************************//
		[HttpGet("get-all-friends")]
		public async Task<ActionResult<IEnumerable<User>>> GetAllFriendsByUserId()

		{
			string token = Request.Headers["Authorization"];
			User? checkingForUser = await _userRepository.GetUserByToken(token);

			List<User> friends = await _friendRepository.GetAllFriendsByUserId(checkingForUser.Id);

			if(friends == null)
			{
				return BadRequest(new { result = "no_friends_found" });
			}
			return Ok(friends);
		}

		[HttpGet("get-all-friend-request")]
		public async Task<ActionResult<IEnumerable<Friend>>> GetAllFriendRequests ()
		{
			string token = Request.Headers["Authorization"];
			User? checkingForUser = await _userRepository.GetUserByToken(token);
			List<Friend> friendRequests = await _friendRepository.GetFriendRequests(checkingForUser.Id);

			if(friendRequests == null)
			{
				return BadRequest(new { result = "no_friend_requests_found" });
			}

			return Ok(friendRequests);
		}
		//*******************GETTERS FUNCTION END******************************//

	}
}
