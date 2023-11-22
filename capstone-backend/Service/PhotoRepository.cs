using capstone_backend.Data;
using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_backend.Service
{
    public class PhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public PhotoRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<Photo?> GetPhotoById(int id)
        {
            return Task.FromResult(_context.Photo.FirstOrDefault(p => p.Id == id));
        }

        public async Task<Photo?> GetFirstPhotoForAlbum(int? albumId)
        {
            // Assuming you have a DbSet<YourPhotoEntity> in YourDbContext
            // and YourPhotoEntity has properties like Id, AlbumId, and others

            return await _context.Photo
                .Where(photo => photo.AlbumId == albumId)
                .OrderBy(photo => photo.Id)
                .FirstOrDefaultAsync();
        }

        public void InsertPhoto(Photo photo)
        {
            _context.Photo.Add(photo);
            _context.SaveChanges();
        }

        public void UpdatePhoto(Photo photo)
        {
            _context.Photo.Update(photo);
            _context.SaveChanges();
        }

        public void DeletePhoto(Photo photo)
        {
            _context.Photo.Remove(photo);
            _context.SaveChanges();
        }
    }
}
