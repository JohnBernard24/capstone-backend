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
		private readonly AlbumRepository _albumRepository;
		private readonly PhotoRepository _photoRepository;

		public AlbumController(UserRepository userRepository, AlbumRepository albumRepository, PhotoRepository photoRepository)
		{
			_userRepository = userRepository;
			_albumRepository = albumRepository;
			_photoRepository = photoRepository;
		}


		//*******************CRUD FUNCTION START******************************//
		[HttpPost("add-album")]
		public async Task<IActionResult> AddAlbum(int userId, [FromBody] AlbumDTO albumDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_album" });
			}

			User? user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				return BadRequest(new { result = "invalid_user_id" });
			}

			var album = new Album
			{
				AlbumName = albumDTO.AlbumName,
				UserId = user.Id,
				User = user
			};

			_albumRepository.InsertAlbum(album);

			albumDTO.AlbumId = album.Id;

			return Ok(albumDTO);
		}

		[HttpPut("update-album")]
		public async Task<IActionResult> UpdateAlbum([FromBody] AlbumDTO albumDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { result = "invalid_album" });
			}

			Album? existingAlbum = await _albumRepository.GetAlbumByAlbumId(albumDTO.AlbumId);

			if (existingAlbum == null)
			{
				return NotFound(new { result = "album_not_found" });
			}

			existingAlbum.AlbumName = albumDTO.AlbumName;

			_albumRepository.UpdateAlbum(existingAlbum);

			return Ok(albumDTO);
		}

		[HttpDelete("delete-album/{albumId}")]
		public async Task<IActionResult> DeleteAlbum(int albumId)
		{
			Album? existingAlbum = await _albumRepository.GetAlbumByAlbumId(albumId);

			if (existingAlbum == null)
			{
				return NotFound(new { result = "album_not_found" });
			}

			_albumRepository.DeleteAlbum(existingAlbum);

			return Ok(new { result = "album_deleted" });
		}
		//*******************CRUD FUNCTION END******************************//


		//*******************ALBUM GETTER START******************************//
		[HttpGet("get-all-albums/{userId}")]
		public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetAllAlbumsByUserId(int userId)
		{
			User? user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				return BadRequest(new { result = "user_id_invalid" });
			}

			List<Album>? albums = await _albumRepository.GetAlbumsByUserId(user.Id);
			if (albums == null)
			{
				return BadRequest(new { result = "no_albums_found" });
			}

			List<AlbumDTO>? albumsDTO = new List<AlbumDTO>();
			foreach(Album album in albums)
			{
				albumsDTO.Add(new AlbumDTO
				{
					AlbumId = album.Id,
					AlbumName = album.AlbumName,
					UserId = album.UserId
				});
			}

			return Ok(albumsDTO);
		}
		[HttpGet("get-all-photos/{albumId}")]
		public async Task<ActionResult<IEnumerable<PhotoDTO>>> GetAllPhotos(int albumId)
		{
			Album? album = await _albumRepository.GetAlbumByAlbumId(albumId);
			if (album == null)
			{
				return BadRequest(new { result = "album_invalid" });
			}

			List<Photo>? photos = await _albumRepository.GetPhotosByAlbumId(album.Id);

			if (photos == null)
			{
				return NotFound(new { result = "no_photos_found" });
			}

			List<PhotoDTO> photoDTOs = new List<PhotoDTO>();
			foreach(Photo photo in photos)
			{
				photoDTOs.Add(new PhotoDTO
				{
					Id = photo.Id,
					PhotoImage = photo.PhotoImage,
					AlbumId = photo.AlbumId,
					UploadDate = photo.UploadDate
				});
			}

			return Ok(photoDTOs);
		}
		[HttpGet("get-mini-album/{userId}")]
		public async Task<ActionResult<IEnumerable<AlbumWithFirstPhoto>>> GetMiniAlbum(int userId)
		{
			User? user = await _userRepository.GetUserById(userId);

			if (user == null)
			{
				return BadRequest(new { result = "user_invalid" });
			}

			List<Album>? albums = await _albumRepository.LimitBySixAlbums(user.Id);

			if (albums == null)
			{
				return NotFound(new { result = "no_albums_found" });
			}

			List<AlbumDTO>? albumDTOs = new List<AlbumDTO>();

			foreach(Album album in albums)
			{
				albumDTOs.Add(new AlbumDTO
				{
					AlbumId = album.Id,
					AlbumName = album.AlbumName,
					UserId = album.UserId
				});
			}

			// Create a new list to store AlbumWithFirstPhoto objects
			List<AlbumWithFirstPhoto> albumsWithFirstPhotoList = new List<AlbumWithFirstPhoto>();

			// Iterate through each album to fetch the first photo and create AlbumWithFirstPhoto objects
			foreach (AlbumDTO albumDTO in albumDTOs)
			{
				// Fetch the first photo for the current album
				Photo? firstPhoto = await _photoRepository.GetFirstPhotoForAlbum(albumDTO.AlbumId);

				PhotoDTO firstPhotoDTO = new PhotoDTO
				{
					Id = firstPhoto.Id,
					PhotoImage = firstPhoto.PhotoImage,
					AlbumId = firstPhoto.AlbumId,
					UploadDate = firstPhoto.UploadDate,
				};

				// Create an AlbumWithFirstPhoto object that includes album information and the first photo
				AlbumWithFirstPhoto albumWithFirstPhoto = new AlbumWithFirstPhoto
				{
					AlbumDTO = albumDTO,
					FirstPhoto = firstPhotoDTO
				};

				// Add the AlbumWithFirstPhoto object to the list
				albumsWithFirstPhotoList.Add(albumWithFirstPhoto);
			}

			return Ok(albumsWithFirstPhotoList);
		}
		//*******************ALBUM GETTER END******************************//

	}
}
