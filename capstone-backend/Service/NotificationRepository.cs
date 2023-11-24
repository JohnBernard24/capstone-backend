using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_backend.Service
{
    public class NotificationRepository
    {

        private readonly ApplicationDbContext _context;


        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Notification>> GetAllNotificationsByUserId(int userId)
        {
            return Task.FromResult(_context.Notification.Where(n => n.NotifiedUserId == userId)
                .Include(n => n.NotifiedUser)
                .ToList());

        }

        public Task<Notification?> GetNotificationByNotificationId(int notifId)
        {
            return Task.FromResult(_context.Notification.Include(n => n.NotifiedUser).FirstOrDefault(n => n.Id == notifId));
        }
        
        public Task<Notification?> GetNotificationByContextIdAndNotificationType(int contextId, string type)
        {
            return Task.FromResult(_context.Notification.FirstOrDefault(n => n.ContextId == contextId && n.NotificationType.Equals(type)));
        }


        public Task<Like?> GetLikeByContextId(int contextId)
        {
            return Task.FromResult(_context.Like.Include(l => l.Post).Include(l => l.Liker).FirstOrDefault(l => l.Id == contextId));
        }

        public Task<Comment?> GetCommentByContextId(int contextId)
        {
            return Task.FromResult(_context.Comment.Include(c => c.Post).Include(c => c.Commenter).FirstOrDefault(c => c.Id == contextId));
        }

        public Task<Friend?> GetFriendRequestByContextId(int contextId)
        {
            return Task.FromResult(_context.Friend.Include(f => f.Sender).FirstOrDefault(f => f.Id == contextId));
        }

        public void InsertNotification(Notification notification)
        {
            _context.Notification.Add(notification);
            _context.SaveChanges();
        }

        public void DeleteNotification(Notification notification)
        {
            _context.Notification.Remove(notification);
            _context.SaveChanges();
        }



    }
}
