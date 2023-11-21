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
		private readonly PhotoRepository _photoRepository;


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

		[HttpGet("get-all-albums/{userId}")]
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
			Album? album = await _albumRepository.GetAlbumByAlbumId(albumId);

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

        [HttpPut("update-album/{albumId}")]
        public async Task<IActionResult> UpdateAlbum(int albumId, [FromBody] AlbumDTO albumDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid_album");
            }

            Album? existingAlbum= await _albumRepository.GetAlbumByAlbumId(albumId);

            if (existingAlbum== null)
            {
                return NotFound("album_not_found");
            }

            existingAlbum.AlbumName = albumDTO.AlbumName;

            _albumRepository.UpdateAlbum(existingAlbum);

            return Ok(existingAlbum);
        }


        [HttpDelete("delete-album/{albumId}")]
        public async Task<IActionResult> DeletePost(int albumId)
        {
            Album? existingAlbum = await _albumRepository.GetAlbumByAlbumId(albumId);

            if (existingAlbum == null)
            {
                return NotFound("album_not_found");
            }

            _albumRepository.DeleteAlbum(existingAlbum);

            return Ok(new { result = "album_deleted" });
        }

        // This method is for the Mini Album
        // Boss JB please add the limit function for this method
        [HttpGet("get-mini-album")]
        public async Task<ActionResult<IEnumerable<AlbumWithFirstPhoto>>> GetMiniAlbum(int userId)
        {
            User? user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return BadRequest("user_invalid");
            }

            List<Album>? albums = await _albumRepository.LimitBySixAlbums(user.Id);

            if (albums == null)
            {
                return NotFound("no_albums_found");
            }

            // Create a new list to store AlbumWithFirstPhoto objects
            List<AlbumWithFirstPhoto> albumsWithFirstPhoto = new List<AlbumWithFirstPhoto>();

            // Iterate through each album to fetch the first photo and create AlbumWithFirstPhoto objects
            foreach (Album album in albums)
            {
                // Fetch the first photo for the current album
                Photo? firstPhoto = await _photoRepository.GetFirstPhotoForAlbum(album.Id);

                // Create an AlbumWithFirstPhoto object that includes album information and the first photo
                AlbumWithFirstPhoto albumWithFirstPhoto = new AlbumWithFirstPhoto
                {
                    Album = album,
                    FirstPhoto = firstPhoto
                };

                // Add the AlbumWithFirstPhoto object to the list
                albumsWithFirstPhoto.Add(albumWithFirstPhoto);
            }

            return Ok(albumsWithFirstPhoto);
        }

        



    }
}
