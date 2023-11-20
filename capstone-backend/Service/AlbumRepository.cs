using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_backend.Service
{
    public class AlbumRepository
    {
        private readonly ApplicationDbContext _context;

        public AlbumRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<Album?> GetAlbumByUserId(int userId)
        {
            return Task.FromResult(_context.Album.FirstOrDefault(a => a.UserId == userId));
        }

        public Task<List<Photo>> GetPhotosByAlbumId(int id)
        {
            return Task.FromResult(_context.Photo
                .Where(photo => photo.Id == id).ToList());
        }

        public void InsertAlbum(Album album)
        {
            _context.Album.Add(album);
            _context.SaveChanges();
        }

        public void UpdateAlbum(Album album)
        {
            _context.Album.Update(album);
            _context.SaveChanges();
        }

        public void DeleteAlbum(Album album)
        {
            _context.Album.Remove(album);
            _context.SaveChanges();
        }
    }
}
