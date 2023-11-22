using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Album
	{
		public int Id { get; set; }
		public string AlbumName { get; set; } = null!;
		public int? UserId { get; set; }
		public User? User { get; set; } = null!;
	}

	public class AlbumDTO
	{
		public int? AlbumId { get; set; }
		public string AlbumName { get; set; } = null!;

		public int? UserId { get; set; }
	}

	public class AlbumWithFirstPhoto
	{
		public AlbumDTO? AlbumDTO { get; set; }
		public PhotoDTO? FirstPhoto { get; set; }
	}
}
