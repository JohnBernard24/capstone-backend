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
		public string AlbumName { get; set; } = null!;

		public int UserId { get; set; }
	}

    public class AlbumWithFirstPhoto
    {
        public Album? Album { get; set; }
        public Photo? FirstPhoto { get; set; }
    }
}
