using PlatformService.Models;

namespace PlatformService.Repositories
{
	public interface IPlatformRepository
	{
		Task<List<Platform>> GetAllPlatforms();
		Task<Platform?> GetPlatformById(int id);
		Task CreatePlatform(Platform platform);
	}
}
