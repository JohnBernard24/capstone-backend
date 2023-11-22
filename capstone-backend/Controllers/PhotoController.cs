using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace capstone_backend.Controllers
{
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly PhotoRepository _photoRepository;
        private readonly AlbumRepository _albumRepository;

        public PhotoController(UserRepository userRepository, PhotoRepository photoRepository, AlbumRepository albumRepository)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _albumRepository = albumRepository;
        }

        [HttpPost("add-photo/{userId}")]
        public async Task<IActionResult> AddPhoto(int userId, [FromBody] PhotoDTO photoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { result = "invalid_photo" });
            }

            User? poster = await _userRepository.GetUserById(userId);
            if (poster == null)
            {
                return BadRequest(new { result = "invalid_user_id" });
            }

            Album? album = await _albumRepository.GetAlbumByAlbumId(userId);
            if (album == null)
            {
                return BadRequest(new { result = "album_not_found" });
            }
            var photo = new Photo
            {
                PhotoImage = photoDTO.PhotoImage,
                UploadDate = DateTime.Now,
                AlbumId = album.Id 
            };

            _photoRepository.InsertPhoto(photo);

            return Ok(photo);
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            Photo? existingPhoto = await _photoRepository.GetPhotoById(photoId);

            if (existingPhoto == null)
            {
                return NotFound(new { result = "photo_not_found" });
            }

            _photoRepository.DeletePhoto(existingPhoto);

            return Ok(new { result = "photo_deleted" });
        }
    }
}
