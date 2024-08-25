using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.MappingProfiles;
using PlatformService.Repositories;
using PlatformService.SyncDataServices;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Extensions
{
	public static class ServicesExtension
	{
		public static IServiceCollection AddAppServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
		{
			services.AddDbContext<DataContext>(options =>
			{
				if (env.IsDevelopment())
				{
					Console.WriteLine("--> We are currently in development, so we're using an In Memory Database");
					options.UseInMemoryDatabase("InMemory_PlatformDB");
				} else
				{
					Console.WriteLine("--> We are currently in Production, so we're using SQL Server");
					var saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");
					Console.WriteLine($"Environmet variable SA_PASSWORD is ${saPassword}");
					var connString = config.GetConnectionString("Default")?.Replace("{PASSWORD}", saPassword).ToString();
					options.UseSqlServer(connString);
					Console.WriteLine("--> Successfully connected to sql server");
				}
			});

			services.AddScoped<IPlatformRepository, PlatformRepository>();
			services.AddScoped<ICommandDataClient, HttpCommandDataClient>();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			services.AddHttpClient();
			return services;
		}
	}
}
