using PlatformService.Models;

namespace PlatformService.Data
{
	public static class DatabasePrep
	{
		public static void PrepareDatabase(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var dbContext = services.GetRequiredService<DataContext>();
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
