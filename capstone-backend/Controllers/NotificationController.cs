using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;

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


		//might need a revision, not working in swagger or postman
		[HttpGet("get-notification-context/{notificationId}")]
		public async Task<ActionResult<Object>> GetNotificationContext(int notificationId)
		{
			Notification? notification = await _notificationRepository.GetNotificationByNotificationId(notificationId);


			if(notification == null)
			{
				return BadRequest(new { result = "no_notification_found" });
			}


			if (notification.NotificationType.Equals("like"))
			{
				// Looks for the like the notification is connected to using the contextId.
				Like? like = await _notificationRepository.GetLikeByContextId(notification.ContextId);
				
				// Returns the Post the like is from (the "liked post")
				return Ok(like?.Post);
			}

			else if (notification.NotificationType.Equals("comment"))
			{
				Comment? comment = await _notificationRepository.GetCommentByContextId(notification.ContextId);

				return Ok(comment?.Post);
			}

			else if (notification.NotificationType.Equals("add-friend-request") || notification.NotificationType.Equals("accept-friend-request"))
			{
				Friend? friend = await _notificationRepository.GetFriendRequestByContextId(notification.ContextId);

				return Ok(friend);
			}
			return BadRequest(new { result = "notification_type_invalid" });
		}




	}
}
