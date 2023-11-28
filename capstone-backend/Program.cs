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
using Microsoft.AspNetCore.Http.Features;

namespace capstone_backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			

			// Add services to the container.

			builder.Services.AddControllers();
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


			/*builder.Services.Configure<IISServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});*/

			builder.Services.Configure<FormOptions>(options =>
			{
				options.ValueLengthLimit = int.MaxValue;
				options.MultipartBodyLengthLimit = 512 * 1024 * 1024;
				options.MultipartHeadersLengthLimit = int.MaxValue;
			});
			builder.WebHost.ConfigureKestrel(options =>
			{
				options.Limits.MaxRequestBodySize = 512 * 1024 * 1024;
			});



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

			builder.Services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Debug));

			


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

			
			string connectionString = "Server=localhost;port=3306;Database=pastebookdb;User=root;";
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));


			/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("capstone_backendContext")));*/
			builder.Services.AddLogging(logging =>
			{
				logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Debug);
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			// Logging configuration added here
			


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