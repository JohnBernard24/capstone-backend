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
        private readonly UserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly NotificationRepository _notificationRepository;
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;

        public NotificationController(UserRepository userRepository, ApplicationDbContext context, NotificationRepository notificationRepository, PostRepository postRepository, CommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _context = context;
            _notificationRepository = notificationRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;

        }

        [HttpGet("get-notifications/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotifications(int userId)
        {
            List<Notification>? notifications = await _notificationRepository.GetAllNotificationsByUserId(userId);

            if (notifications == null)
            {
                return NotFound("no_notifications_found");
            }

            return notifications;

            
        }

        [HttpGet("get-notification-context/{userId}")]
        public async Task<ActionResult<Object>> GetNotificationContext(int notificationId)
        {
            Notification? notification = await _notificationRepository.GetNotificationByNotificationId(notificationId);


            if(notification == null)
            {
                return BadRequest("no_notification_found");
            }


            if (notification.NotificationType.Equals("like"))
            {
                // Looks for the like the notification is connected to using the contextId.
                var like = await _notificationRepository.GetLikeByContextId(notification.ContextId);
                
                // Returns the Post the like is from (the "liked post")
                return Ok(like?.Post);
            }

            else if (notification.NotificationType.Equals("comment"))
            {
                var comment = await _notificationRepository.GetCommentByContextId(notification.ContextId);

                return Ok(comment?.Post);
            }

            else if (notification.NotificationType.Equals("add-friend-request") || notification.NotificationType.Equals("accept-friend-request"))
            {
                var friend = await _notificationRepository.GetFriendRequestByContextId(notification.ContextId);

                return Ok(friend);
            }
            

            return BadRequest("notification_type_invalid");



        }



        







    }
}
