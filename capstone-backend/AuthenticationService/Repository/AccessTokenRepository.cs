using capstone_backend.AuthenticationService.Models;
using capstone_backend.Data;
using capstone_backend.Models;

namespace capstone_backend.AuthenticationService.Repository
{
	public class AccessTokenRepository
	{

		private readonly ApplicationDbContext _context;


		public AccessTokenRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public void InsertToken(AccessToken	token)
		{
			_context.AccessToken.Add(token);
			_context.SaveChanges();
		}

		public void UpdateToken(AccessToken token)
		{
			_context.AccessToken.Update(token);
			_context.SaveChanges();
		}
		public void DeleteToken(AccessToken token)
		{
			_context.AccessToken.Remove(token);
			_context.SaveChanges();
		}


		public void DeleteAllToken(int userId)
		{
			IEnumerable<AccessToken> tokens = _context.AccessToken.ToArray().Where(t => t.UserId == userId).ToList();

			_context.AccessToken.RemoveRange(tokens);
			_context.SaveChanges();
		}
	}
}
