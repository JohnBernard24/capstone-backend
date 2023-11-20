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
                return BadRequest("invalid_photo");
            }

            User? poster = await _userRepository.GetUserById(userId);
            if (poster == null)
            {
                return BadRequest("invalid_user_id");
            }

            Album? album = await _albumRepository.GetAlbumByUserId(userId);
            if (album == null)
            {
                return BadRequest("album_not_found");
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
    }
}
