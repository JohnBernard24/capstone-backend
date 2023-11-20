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
        public async Task<IActionResult> GetNotificationContext(Notification notification)
        {


            if (notification.NotificationType.Equals("like"))
            {
                Like? like = _postRepository.getLike





                Post? post = await _postRepository.GetPostById(notification.ContextId);
                return Ok(post);
            }

            /*else if (notification.NotificationType.Equals("friend request"))
            {
                churva
            }*/

            return BadRequest("context_id_invalid");

            

        } 





    }
}
