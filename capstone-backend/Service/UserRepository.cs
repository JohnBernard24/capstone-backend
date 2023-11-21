using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace capstone_backend.Service
{
	public class UserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext dbContext)
		{
			_context = dbContext;
		}

		public Task<List<User>> GetAllUser()
		{
			return Task.FromResult(_context.User.ToList());
		}

		public Task<User?> GetUserById(int id)
		{
			return Task.FromResult(_context.User?.FirstOrDefault(u => u.Id == id));
		}

		public Task<User?> GetUserByEmail(string email)
		{
			return Task.FromResult(_context.User.FirstOrDefault(u => u.Email == email));
		}

		public Task<List<User>> GetUsersBySearchName(string name)
		{
			return Task.FromResult(_context.User
			   .Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name))
			   .ToList());
		}

		public void InsertUser(User user)
		{
			
			_context.User.Add(user);
			_context.SaveChanges();
		}

		public void UpdateUser(User user)
		{
			_context.User.Update(user);
			_context.SaveChanges();
		}

		public void DeleteUser(User user)
		{
			_context.User.Remove(user);
			_context.SaveChanges();
		}
	}
}
