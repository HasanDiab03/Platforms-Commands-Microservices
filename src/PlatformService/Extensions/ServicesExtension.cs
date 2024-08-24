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
		public static IServiceCollection AddAppServices(this IServiceCollection services)
		{
			services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("InMemory_PlatformDB"));
			services.AddScoped<IPlatformRepository, PlatformRepository>();
			services.AddScoped<ICommandDataClient, HttpCommandDataClient>();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			services.AddHttpClient();
			return services;
		}
	}
}
