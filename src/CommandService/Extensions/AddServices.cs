using CommandService.Data;
using CommandService.MappingProfiles;
using CommandService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Extensions
{
	public static class AddServices
	{
		public static IServiceCollection AddAppServices(this IServiceCollection services)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseInMemoryDatabase("CommandsDB");
			});
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddScoped<ICommandRepository, CommandRepository>();

			return services;
		}
	}
}
