using capstone_backend.Data;
using capstone_backend.Models;
using capstone_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace capstone_backend.Controllers
{
    [Route("api/album")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly AlbumRepository _albumRepository;


        public AlbumController(ApplicationDbContext context, UserRepository userRepository, AlbumRepository albumRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _albumRepository = albumRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAlbum(int userId, [FromBody] AlbumDTO albumDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_album");
            }

            User? poster = await _userRepository.GetUserById(userId);
            if (poster == null)
            {
                return BadRequest("invalid_user_id");
            }

            var album = new Album
            {
                AlbumName = albumDTO.AlbumName,
                UserId = poster.Id
            };

            _albumRepository.InsertAlbum(album);

            return Ok(album);
        }


        [HttpGet("get-all-photos/{albumId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetAllPhotos(int userId)
        {
            Album? album = await _albumRepository.GetAlbumByUserId(userId);

            if (album == null)
            {
                return BadRequest("user_invalid");
            }

            List<Photo>? photos = await _albumRepository.GetPhotosByAlbumId(album.Id);

            if (photos == null)
            {
                return NotFound("no_photos_found");
            }

            return photos;
        }

    }
}
