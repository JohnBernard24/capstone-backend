using capstone_backend.Data;
using capstone_backend.Models;
using System;
using System.Threading.Tasks;


namespace capstone_backend.Service
{
    public class TimelineRepository
    {

        private readonly ApplicationDbContext _context;

        public TimelineRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<Timeline?> GetTimelineByUserId(int userId)
        {
            return Task.FromResult(_context.TimeLine.FirstOrDefault(t => t.UserId == userId));
        }

        public List<Post> GetPostsByTimelineId(int id)
        {
            return _context.Post.Where(p => p.TimelineId == id).ToList();
        }

       
    }
}
