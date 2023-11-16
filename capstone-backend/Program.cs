using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using capstone_backend.Data;
using System;
using capstone_backend.Service;
using Microsoft.AspNetCore.Identity;

namespace capstone_backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddSingleton<BcryptPasswordHasher>();


			string connectionString = "Server=localhost;port=3306;Database=pastebookdb;User=root;";
			builder.Services.AddDbContext<capstone_backendContext>(options => options.UseMySQL(connectionString));


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<capstone_backendContext>();
			DataSeeder.SeedDatabase(context);

			app.Run();
		}
	}
}