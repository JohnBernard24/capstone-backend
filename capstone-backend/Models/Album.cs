using System.ComponentModel.DataAnnotations;

namespace capstone_backend.Models
{
	public class Album
	{
		public int Id { get; set; }
		public string AlbumName { get; set; } = null!;
	}
}
