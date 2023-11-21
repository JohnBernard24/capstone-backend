using capstone_backend.Data;
using capstone_backend.Models;
using System;
using System.Threading.Tasks;

namespace capstone_backend.Service
{
	public class PostRepository
	{

		private readonly ApplicationDbContext _context;

		public PostRepository(ApplicationDbContext dbContext)
		{
			_context = dbContext;
		}

		public Task<Post?> GetPostById(int id)
		{
			return Task.FromResult(_context.Post.FirstOrDefault(x => x.Id == id));
		}

		public Task<List<Post>>? GetPostsByUserId(int userId)
		{
			return Task.FromResult(_context.Post.Where(p => p.PosterId == userId).ToList());
		}

		public Task<Like?> getLikeByPostIdAndUserId(int postId, int likerId)
		{
			return Task.FromResult(_context.Like.FirstOrDefault(l => l.PostId == postId && l.LikerId == likerId));
		}

		public Task<List<Post>> GetAllPostsByUserId(int userId)
		{
			return Task.FromResult(_context.Post.Where(p => p.PosterId == userId).ToList());
		}

		public Task<List<Like>> GetLikesByPostId(int postId)
		{
			return Task.FromResult(_context.Like.Where(l => l.PostId == postId).ToList());
		}

		public void InsertPost(Post post)
		{
			_context.Post.Add(post);
			_context.SaveChanges();
		}

		public void UpdatePost(Post post)
		{
			_context.Post.Update(post);
			_context.SaveChanges();
		}

		public void DeletePost(Post post)
		{
			_context.Post.Remove(post);
			_context.SaveChanges();
		}

		public void InsertLike(Like like)
		{
			_context.Like.Add(like);
			_context.SaveChanges();
		}

		public void RemoveLike(Like like)
		{
			_context.Like.Remove(like);
			_context.SaveChanges();
		}

		

	}
}
