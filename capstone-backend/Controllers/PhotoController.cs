﻿using capstone_backend.Models;
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
		public async Task<IActionResult> AddPhoto([FromForm] int albumId, [FromForm] IFormFile file)
		{
			Console.WriteLine(file);

			if (file == null || file.Length == 0)
			{
				return BadRequest(new { result = "invalid_file" });
			}

			Album? album = await _albumRepository.GetAlbumByAlbumId(albumId);
			if (album == null)
			{
				return BadRequest(new { result = "album_not_found" });
			}

			using (var memoryStream = new MemoryStream())
			{
				await file.CopyToAsync(memoryStream);

				Photo photo = new Photo
				{
					PhotoImage = memoryStream.ToArray(),
					UploadDate = DateTime.Now,
					AlbumId = album.Id,
					Album = album
				};

				_photoRepository.InsertPhoto(photo);

				return Ok(new { result = "success", PhotoId = photo.Id });
			}
		}

		[HttpGet("get-photo/{photoId}")]
		public async Task<IActionResult> GetPhoto(int photoId)
		{
			Photo? existingPhoto = await _photoRepository.GetPhotoById(photoId);

			if (existingPhoto == null)
			{
				return NotFound(new { result = "photo_not_found" });
			}

			var photo = new PhotoDTO()
			{
				Id = existingPhoto.Id,
				PhotoImage = $"data:image/png;base64,{Convert.ToBase64String(existingPhoto.PhotoImage)}",
				UploadDate = existingPhoto.UploadDate,
				AlbumId = existingPhoto.AlbumId
			};


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
