using System.Text.Json;
using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;

namespace CommandService.Events
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    Console.WriteLine("Could not determine event type");
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage) {
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);
            return eventType?.Event switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined
            };
        }
        private async void AddPlatform(string message) {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
            var platformDTO = JsonSerializer.Deserialize<PlatformPublishDTO>(message);
            try
            {
                var platform = _mapper.Map<Platform>(platformDTO);
                bool exists = await repo.ExternalPlatformExists(platform.ExternalId);
                if(exists)
                {
                    Console.WriteLine("--> Platform already exists");
                    return;
                }
                await repo.CreatePlatform(platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB: {ex.Message}");
            }
        }
    }
    enum EventType {
        PlatformPublished,
        Undetermined
    }
}