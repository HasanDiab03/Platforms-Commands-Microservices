using CommandService.Models;

namespace CommandService.Repositories
{
	public interface ICommandRepository
	{
		Task<List<Platform>> GetAllPlatforms();
		Task CreatePlatform(Platform platform);
		Task<bool> PlatformExists(int platformId);

		Task<List<Command>> GetCommandsForPlatform(int platformId);
		Task<Command?> GetCommandForPlatform(int platformId, int commandId);
		Task CreateCommandForPlatform(int platformId, Command command);
	}
}
