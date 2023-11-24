using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using capstone_backend.Data;
using System;
using capstone_backend.Service;
using Microsoft.AspNetCore.Identity;
using capstone_backend.AuthenticationService.Repository;
using capstone_backend.AuthenticationService.Authenticator;
using capstone_backend.AuthenticationService.TokenGenerators;
using capstone_backend.AuthenticationService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace capstone_backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			

			// Add services to the container.

			builder.Services.AddControllers();

			/*builder.Services.Configure<IISServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});*/

			AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();

			builder.Configuration.Bind("Authentication", authenticationConfiguration);
			builder.Services.AddSingleton(authenticationConfiguration);

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();


			builder.Services.AddScoped<BcryptPasswordHasher>();
			builder.Services.AddScoped<UserRepository>();
			builder.Services.AddScoped<PostRepository>();
			builder.Services.AddScoped<TimelineRepository>();
			builder.Services.AddScoped<CommentRepository>();
			builder.Services.AddScoped<FriendRepository>();
			builder.Services.AddScoped<NotificationRepository>();
			builder.Services.AddScoped<AlbumRepository>();
			builder.Services.AddScoped<PhotoRepository>();

			builder.Services.AddScoped<AccessTokenRepository>();
			builder.Services.AddScoped<TokenGenerator>();
			builder.Services.AddScoped<AccessTokenGenerator>();
			builder.Services.AddScoped<TokenGenerator>();
			builder.Services.AddScoped<Authenticator>();
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
			{


				o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
					ValidIssuer = authenticationConfiguration.Issuer,
					ValidAudience = authenticationConfiguration.Audience,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
					builder =>
					{
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					});
			});

			string connectionString = "Server=localhost;port=3306;Database=pastebookdb;User=root;";
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// Enable CORS
			app.UseCors("AllowAllOrigins");


			app.UseAuthorization();


			app.MapControllers();

			/*var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
			DataSeeder.SeedDatabase(context);*/

			app.Run();
		}

		
		
	}
}