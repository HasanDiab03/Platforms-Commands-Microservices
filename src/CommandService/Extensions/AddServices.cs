using CommandService.AsyncDataServices;
using CommandService.Data;
using CommandService.Events;
using CommandService.MappingProfiles;
using CommandService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Extensions
{
	public static class AddServices
	{
		public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseInMemoryDatabase("CommandsDB");
			});
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddScoped<ICommandRepository, CommandRepository>();
			// this has to be singeton, since it will be injected into the Event Listener class, which is a singleton
			services.AddSingleton<IEventProcessor, EventProcessor>();
			services.Configure<RabbitMqConfig>(config.GetSection("RabbitMq"));

			services.AddHostedService<MessageBusSubscriber>();

			return services;
		}
	}
}
