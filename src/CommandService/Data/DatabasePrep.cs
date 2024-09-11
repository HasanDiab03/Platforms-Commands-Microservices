using CommandService.Models;
using CommandService.Repositories;
using CommandService.SyncDataServices;

namespace CommandService.Data
{
	public static class DatabasePrep
	{
		public static WebApplication PrepPopulate(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var grpcClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
			var platforms = grpcClient.GetAllPlatforms();
			var commandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
			SeedData(commandRepo, platforms).Wait();
			return app;
		}
		private static async Task SeedData(ICommandRepository commandRepo, List<Platform> platforms)
		{
			Console.WriteLine("---> Seeding new platforms that we got from gRPC server");
			List<Task> tasks = new();
			foreach (var platform in platforms)
			{
				if(! await commandRepo.ExternalPlatformExists(platform.ExternalId))
				{
					tasks.Add(commandRepo.CreatePlatform(platform));
				}
			}
			Task.WhenAll(tasks).Wait();
		}
	}
}
