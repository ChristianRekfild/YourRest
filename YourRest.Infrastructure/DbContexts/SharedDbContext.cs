using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Domain.Entities;

namespace YourRest.Infrastructure.DbContexts
{
    public class SharedDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Country> Countries { get; set; }

        public SharedDbContext(DbContextOptions<SharedDbContext> options) : base(options)
        {
        }

        public void ClearAllTables()
        {
            Countries.RemoveRange(Countries);
            
            // Add other DbSet removals here
            // Example: 
            // Rooms.RemoveRange(Rooms);
            // Hotels.RemoveRange(Hotels);
            // ... and so on

            SaveChanges();
        }
    }
    
}