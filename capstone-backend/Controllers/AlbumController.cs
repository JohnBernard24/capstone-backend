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

		[HttpPost("add-album")]
		public async Task<IActionResult> AddAlbum(int userId, [FromBody] AlbumDTO albumDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid_album");
			}

			User? user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				return BadRequest("invalid_user_id");
			}

			var album = new Album
			{
				AlbumName = albumDTO.AlbumName,
				UserId = user.Id
			};

			_albumRepository.InsertAlbum(album);

			return Ok(album);
		}

		/*[HttpPost("update-album")]
		public async */

		[HttpGet("get-all-albums{userId}")]
		public async Task<ActionResult<IEnumerable<Album>>> GetAllAlbumsByUserId(int userId)
		{
			var user = await _userRepository.GetUserById(userId);
			if(user == null)
			{
				return BadRequest("user_id_invalid");
			}

			List<Album>? albums = await _albumRepository.GetAlbumsByUserId(userId);

			if(albums == null)
			{
				return BadRequest("no_albums_found");
			}

			return Ok(albums);

		}

		[HttpGet("get-all-photos/{albumId}")]
		public async Task<ActionResult<IEnumerable<Photo>>> GetAllPhotos(int albumId)
		{
			Album? album = await _albumRepository.GetAlbumByUserId(albumId);

			if (album == null)
			{
				return BadRequest("album_invalid");
			}

			List<Photo>? photos = await _albumRepository.GetPhotosByAlbumId(album.Id);

			if (photos == null)
			{
				return NotFound("no_photos_found");
			}

			return Ok(photos);
		}

	}
}
