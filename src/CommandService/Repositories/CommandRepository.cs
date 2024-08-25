using CommandService.Data;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Repositories
{
	public class CommandRepository : ICommandRepository
	{
		private readonly DataContext _context;

		public CommandRepository(DataContext context)
        {
			_context = context;
		}
        public Task CreateCommandForPlatform(int platformId, Command command)
		{
			throw new NotImplementedException();
		}

		public Task CreatePlatform(Platform platform)
		{
			throw new NotImplementedException();
		}

		public Task<List<Platform>> GetAllPlatforms()
		{
			throw new NotImplementedException();
		}

		public async Task<Command?> GetCommandForPlatform(int platformId, int commandId)
			=> await _context.Commands.FirstOrDefaultAsync(x => x.PlatformId == platformId && x.Id == commandId);

		public async Task<List<Command>> GetCommandsForPlatform(int platformId)
			=> await _context.Commands
						.Where(c => c.PlatformId == platformId)
						.OrderBy(c => c.Platform.Name)
						.ToListAsync();

		public async Task<bool> PlatformExists(int platformId)
			=> await _context.Platforms.FindAsync(platformId) is not null;
	}
}
