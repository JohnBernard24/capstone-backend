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


				context.TimeLine.AddRange(
					new Timeline
					{
						UserId = 1,
						User = context.User.FirstOrDefault(u => u.Id == 1)
					},
					new Timeline
					{
						UserId = 2,
						User = context.User.FirstOrDefault(u => u.Id == 2)
					},
					new Timeline
					{
						UserId = 3,
						User = context.User.FirstOrDefault(u => u.Id == 3)
					}

				);

				context.SaveChanges();
			}

			
		}
	}
}
