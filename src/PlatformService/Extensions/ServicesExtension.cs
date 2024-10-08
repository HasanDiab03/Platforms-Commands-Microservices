﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformService.AsyncDataServices;
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
			services.AddSingleton<IMessageBusClient, MessageBusClient>();
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			services.AddHttpClient();
			services.Configure<RabbitMqConfig>(config.GetSection("RabbitMQ"));

			services.AddGrpc();

			return services;
		}
	}
}
