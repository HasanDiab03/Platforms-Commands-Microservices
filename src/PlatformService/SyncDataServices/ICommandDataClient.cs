using PlatformService.DTOs;

namespace PlatformService.SyncDataServices
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommandService(PlatformDTO platform);
    }
}
