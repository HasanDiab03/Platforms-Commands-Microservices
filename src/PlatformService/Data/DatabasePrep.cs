using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
	public static class DatabasePrep
	{
		public static void PrepareDatabase(this WebApplication app, IWebHostEnvironment env)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var dbContext = services.GetRequiredService<DataContext>();
			if(env.IsProduction())
			{
				Console.WriteLine("--> Applying Pending Migrations");
				try
				{
					dbContext.Database.Migrate();
					Console.WriteLine("--> Succesfully applied migrations");
				}
				catch (Exception ex) 
				{
					Console.WriteLine($"Failed to apply migrations, {ex.Message}");
				}
			}
			SeedData(dbContext);
		}
		private static void SeedData(DataContext context)
		{
			if (context.Platforms.Any())
			{
				Console.WriteLine("We already have data");
				return;
			}
			Console.WriteLine("-> Seeding Data...");
			List<Platform> platforms = new ()
			{
				new() {Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
				new() {Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
				new() {Name = "Java", Publisher = "Oracle", Cost = "Free"},
			};
			context.Platforms.AddRange(platforms);
			context.SaveChanges();
		}
	}
}
