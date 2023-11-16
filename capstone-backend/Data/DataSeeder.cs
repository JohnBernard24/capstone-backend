using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_backend.Data
{
	public class DataSeeder
	{

		public static void SeedDatabase(capstone_backendContext context)
		{
			context.Database.Migrate();


			if (!context.User.Any())
			{
				context.User.AddRange(
					new User
					{
						Id = new Guid(),
						FirstName = "John Bernard",
						LastName = "Tinio",
						Email = "bernard.tinio@pointwest.com.ph",
						PasswordHash = "hiafhiaiyiwqbriq9fbjfhufebjkfhuksbdfkewubfjsd",
						Birthdate = DateTime.Now,
						Sex = "Male",
						MobileNumber = "09069141416",
						AboutMe = "hahahaha"
					}

				) ;

				context.SaveChanges();
			}
		}
	}
}
