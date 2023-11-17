using capstone_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace capstone_backend.Data
{
	public class DataSeeder
	{
	

		public static void SeedDatabase(ApplicationDbContext context)
		{
			context.Database.Migrate();


			if (!context.User.Any())
			{
				context.User.AddRange(
					new User
					{
						FirstName = "John Bernard",
						LastName = "Tinio",
						Email = "bernard.tinio@pointwest.com.ph",
						HashedPassword = "$2a$12$.0YFntkwK219DLxXK9LXsuaajQzKO98umjrd/fIMqkErT52l.mldq",
						BirthDate = DateTime.Now,
						Sex = "Male",
						PhoneNumber = "09069141416"
					},
					new User
					{
						
						FirstName = "Blessie",
						LastName = "Balagtas",
						Email = "blessie.balagtas@pointwest.com.ph",
						HashedPassword = "$2a$12$.0YFntkwK219DLxXK9LXsuaajQzKO98umjrd/fIMqkErT52l.mldq",
						BirthDate = DateTime.Now,
						Sex = "Female",
						PhoneNumber = "09069141416"
					},
					new User
					{
						
						FirstName = "Tim",
						LastName = "Dy",
						Email = "tim.dy@pointwest.com.ph",
						HashedPassword = "$2a$12$.0YFntkwK219DLxXK9LXsuaajQzKO98umjrd/fIMqkErT52l.mldq",
						BirthDate = DateTime.Now,
						Sex = "Male",
						PhoneNumber = "09069141416"
					}
				);

				context.SaveChanges();
			}

			/*if (!context.TimeLine.Any())
			{



				context.TimeLine.AddRange(
					new Timeline(
						from u in context.User
						join t in context.TimeLine on u.Id equals t.UserId
						select new
						{
							Id = t.Id,

						}
						)

					); 
			}*/
		}
	}
}
