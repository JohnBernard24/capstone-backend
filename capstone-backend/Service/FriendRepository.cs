using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace capstone_backend.Service
{
	public class FriendRepository
	{

		private readonly ApplicationDbContext _context;

		public FriendRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<User>> GetAllFriendsByUserId(int userId)
		{
			var friendIds = await _context.Friend
				.Where(f => (f.ReceiverId == userId || f.SenderId == userId) && f.isFriend == true)
				.Select(f => f.SenderId)
				.Distinct()
				.ToListAsync();

			var friends = await _context.User
				.Where(u => friendIds.Contains(u.Id))
				.ToListAsync();

			return friends;
		}


		public async Task<List<Friend>> GetFriendRequests(int userId)
		{
			var friendRequests = await _context.Friend
				.Where(f => f.ReceiverId == userId && f.isFriend == false)
				.Include(f => f.Sender)
				.ToListAsync();

			return friendRequests;
		}


		public async Task<bool> FriendRequestExists(int senderId, int receiverId)
		{
			return await _context.Friend
				.AnyAsync(f =>
					(f.SenderId == senderId && f.ReceiverId == receiverId) ||
					(f.SenderId == receiverId && f.ReceiverId == senderId)
				);
		}

		public async Task<Friend?> GetFriendRequestByFriendId(int requestId)
		{
			return await _context.Friend.FindAsync(requestId);
		}

		public void InsertFriend(Friend friend)
		{
			_context.Friend.Add(friend);
			_context.SaveChanges();
		}

		public void UpdateFriend(Friend friend)
		{
			_context.Friend.Update(friend);
			_context.SaveChanges();
		}
		public void DeleteFriend(Friend friend)
		{
			_context.Friend.Remove(friend);
			_context.SaveChanges();
		}
	}
}