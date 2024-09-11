using CommandService.Models;

namespace CommandService.SyncDataServices
{
	public interface IPlatformDataClient
	{
		List<Platform> GetAllPlatforms();
	}
}
