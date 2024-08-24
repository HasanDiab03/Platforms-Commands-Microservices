using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Platform> Platforms { get; set; }
    }
}
