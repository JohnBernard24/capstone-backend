using capstone_backend.Data;
using capstone_backend.Models;

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
            return Task.FromResult(_context.Notification.Where(n => n.UserId == userId).ToList());

        }


        public void InsertNotification(Notification notification)
        {
            _context.Notification.Add(notification);
            _context.SaveChanges();
        }




    }
}
