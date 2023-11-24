using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace capstone_backend.Controllers
{


	[Route("api/notification")]
	[ApiController]
	public class NotificationController : Controller
	{
		
		private readonly NotificationRepository _notificationRepository;
		

		public NotificationController(NotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		[HttpGet("get-notifications/{userId}")]
		public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetAllNotifications(int userId)
		{
			List<Notification>? notifications = await _notificationRepository.GetAllNotificationsByUserId(userId);

			if (notifications == null)
			{
				return NotFound(new { result = "no_notifications_found" });
			}

			List<NotificationDTO> notificationDTOs = new List<NotificationDTO>();
			foreach(Notification notification in notifications)
			{
				if(notification.NotifiedUser == null)
				{
					continue;
				}
				notificationDTOs.Add(new NotificationDTO
				{
					Id = notification.Id,
					NotifiedUserId = notification.NotifiedUserId,
					NotifiedUser = new MiniProfileDTO
					{
						Id = notification.NotifiedUser.Id,
						FirstName = notification.NotifiedUser.FirstName,
						LastName = notification.NotifiedUser.LastName,
						Photo = notification.NotifiedUser.Photo
					},
					NotificationType = notification.NotificationType,
					ContextId = notification.ContextId,
					IsRead = notification.IsRead
				});
			}

			return notificationDTOs;
		}



		
		[HttpGet("get-notification-context/{notificationId}")]
		public async Task<IActionResult> GetNotificationContext(int notificationId)
		{
			Notification? notification = await _notificationRepository.GetNotificationByNotificationId(notificationId);

			if (notification == null)
			{
				return NotFound(new { result = "no_notification_found" });
			}

			switch (notification.NotificationType)
			{
				case "like":
					Like? like = await _notificationRepository.GetLikeByContextId(notification.ContextId);
					return SerializeAndReturn(like?.Post);

				case "comment":
					Comment? comment = await _notificationRepository.GetCommentByContextId(notification.ContextId);
					return SerializeAndReturn(comment?.Post);

				case "add-friend-request":
				case "accept-friend-request":
					Friend? friend = await _notificationRepository.GetFriendRequestByContextId(notification.ContextId);
					return SerializeAndReturn(friend);

				default:
					return BadRequest(new { result = "notification_type_invalid" });
			}
		}

		private ActionResult SerializeAndReturn(object data)
		{
			string jsonData = JsonConvert.SerializeObject(data);
			return new ContentResult
			{
				Content = jsonData,
				ContentType = "application/json",
				StatusCode = 200
			};
		}






	}
}
