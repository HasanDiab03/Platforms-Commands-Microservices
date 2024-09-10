using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Repositories
{
	public class PlatformRepository : IPlatformRepository
	{
		private readonly DataContext _context;

		public PlatformRepository(DataContext context)
        {
			_context = context;
		}
        public async Task CreatePlatform(Platform platform)
		{
			if (platform == null)
				throw new ArgumentNullException("Platform object is required");
			_context.Platforms.Add(platform);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Platform>> GetAllPlatforms()
		{
			return await _context.Platforms.ToListAsync();
		}

		public async Task<Platform> GetPlatformById(int id)
		{
			return await _context.Platforms.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
