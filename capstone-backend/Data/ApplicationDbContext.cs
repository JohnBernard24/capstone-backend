using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using capstone_backend.Models;
using capstone_backend.AuthenticationService.Models;

namespace capstone_backend.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> User { get; set; } = default!;
		public DbSet<Timeline> TimeLine { get; set; } = default!;
		public DbSet<Post> Post { get; set; } = default!;
		public DbSet<Photo> Photo { get; set; } = default!;
		public DbSet<Like> Like { get; set; } = default!;
		public DbSet<Comment> Comment { get; set; } = default!;
		public DbSet<Friend> Friend { get; set; } = default!;
		public DbSet<Album> Album { get; set; } = default!;
		public DbSet<Notification> Notification { get; set; } = default!;
		public DbSet<AccessToken> AccessToken { get; set; } = default!;


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

	}
}
