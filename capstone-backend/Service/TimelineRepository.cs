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
				/*.Select(post => new Post
				{
					Id = post.Id,
					PostTitle = post.PostTitle,
					PhotoId = post.PhotoId,
					Photo = post.Photo,
					Description = post.Description,
					DatePosted = post.DatePosted,
					Comments = post.Comments,
					TimelineId = post.TimelineId,
					Timelines= = new Timeline
					{
						Id = post.Timeline.Id,
						UserId = post.Timeline.UserId,
						User = new User
						{
							Id = post.Timeline.User.Id,
							FirstName = post.Timeline.User.FirstName,
							LastName = post.Timeline.User.LastName,
							Email = post.Timeline.User.Email,
							BirthDate = post.Timeline.User.BirthDate,
							Sex = post.Timeline.User.Sex,
							PhoneNumber = post.Timeline.User.PhoneNumber,
							AboutMe = post.Timeline.User.AboutMe,
							ProfileImageId = post.Timeline.User.ProfileImageId,
							Photo = post.Timeline.User.Photo
						}
					}
				})*/
				.ToList());
		}



	}
}
