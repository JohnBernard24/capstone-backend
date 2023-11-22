using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace capstone_backend.Controllers
{
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoRepository _photoRepository;
        private readonly AlbumRepository _albumRepository;

        public PhotoController(PhotoRepository photoRepository, AlbumRepository albumRepository)
        {
            _photoRepository = photoRepository;
            _albumRepository = albumRepository;
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto([FromBody] PhotoDTO photoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { result = "invalid_photo" });
            }

            Album? album = await _albumRepository.GetAlbumByAlbumId(photoDTO.AlbumId);
            if (album == null)
            {
                return BadRequest(new { result = "album_not_found" });
            }

            Photo photo = new Photo
            {
                PhotoImage = photoDTO.PhotoImage,
                AlbumId = album.Id,
                Album = album
            };

            _photoRepository.InsertPhoto(photo);

            return Ok(photoDTO);
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
