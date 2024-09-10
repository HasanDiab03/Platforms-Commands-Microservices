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
        public async Task CreateCommandForPlatform(int platformId, Command command)
		{
			if (command is null)
				throw new ArgumentNullException("Please provide a command");
			command.PlatformId = platformId;
			_context.Commands.Add(command);
			await _context.SaveChangesAsync();
		}

		public async Task CreatePlatform(Platform platform)
		{
			if (platform is null)
				throw new ArgumentNullException("Please provide a platform");
			_context.Platforms.Add(platform);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Platform>> GetAllPlatforms()
			=> await _context.Platforms.ToListAsync();

		public async Task<Command> GetCommandForPlatform(int platformId, int commandId)
			=> await _context.Commands.FirstOrDefaultAsync(x => x.PlatformId == platformId && x.Id == commandId);

		public async Task<List<Command>> GetCommandsForPlatform(int platformId)
			=> await _context.Commands
						.Where(c => c.PlatformId == platformId)
						.OrderBy(c => c.Platform.Name)
						.ToListAsync();

		public async Task<bool> PlatformExists(int platformId)
			=> await _context.Platforms.FindAsync(platformId) is not null;
		public async Task<bool> ExternalPlatformExists(int externalPlatformId)
			=> await _context.Platforms.AnyAsync(x => x.ExternalId == externalPlatformId);
	}
}
