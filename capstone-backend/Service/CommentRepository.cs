using capstone_backend.Data;
using capstone_backend.Models;

namespace capstone_backend.Service
{
	public class CommentRepository
	{
		private readonly ApplicationDbContext _context;

		public CommentRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public Task<List<Comment>> GetAllCommentsByPostId(int postId)
		{
			return Task.FromResult(_context.Comment.Where(c => c.PostId == postId).ToList());
		}


		public Task<Comment?> GetCommentById(int id)
		{
			return Task.FromResult(_context.Comment.FirstOrDefault(x => x.Id == id));
		}

		public void InsertComment(Comment comment)
		{
			_context.Comment.Add(comment);
			_context.SaveChanges();
		}

		public void UpdateComment(Comment comment)
		{
			_context.Comment.Update(comment);
			_context.SaveChanges();
		}

		public void DeleteComment(Comment comment)
		{
			_context.Comment.Remove(comment);
			_context.SaveChanges();
		}

	}
}
