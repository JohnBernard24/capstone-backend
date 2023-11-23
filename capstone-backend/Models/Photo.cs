using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Photo
	{
		public int Id { get; set; }
		public byte[] PhotoImage { get; set; } = null!;

		[DataType(DataType.Date)]
		public DateTime UploadDate { get; set; } = DateTime.Now;



		public int? AlbumId { get; set; }
		public Album? Album { get; set; }
	}

	public class PhotoDTO
	{
		public int? Id { get; set; }
		public byte[] PhotoImage { get; set; } = null!;
		public int? AlbumId { get; set; }
		public DateTime UploadDate { get; set; } = DateTime.Now;

	}
}
