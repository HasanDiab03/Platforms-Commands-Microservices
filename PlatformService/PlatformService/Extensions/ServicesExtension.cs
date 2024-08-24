using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.MappingProfiles;
using PlatformService.Repositories;

namespace PlatformService.Extensions
{
	public static class ServicesExtension
	{
		public static IServiceCollection AddAppServices(this IServiceCollection services)
		{
			services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("InMemory_PlatformDB"));
			services.AddScoped<IPlatformRepository, PlatformRepository>();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			return services;
		}
	}
}
