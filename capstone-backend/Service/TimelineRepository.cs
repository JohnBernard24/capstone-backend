using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;
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

		public Task<List<Post>> GetPostsByTimelineId(int id)
		{
			return Task.FromResult(_context.Post
				.Where(post => post.TimelineId == id)
				.Include(post => post.Poster)
				.Include(post => post.Timeline)
				.Include(post => post.Photo)
				.ToList());
		}



	}
}
