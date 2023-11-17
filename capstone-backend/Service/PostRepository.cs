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


    }
}
