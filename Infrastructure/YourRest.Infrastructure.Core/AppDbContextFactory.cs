using Microsoft.EntityFrameworkCore;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.Infrastructure.Core
{
    public class AppDbContextFactory : IDbContextFactory<SharedDbContext>
    {
        private readonly string _connectionString;

        public AppDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SharedDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SharedDbContext>();
            optionsBuilder.UseNpgsql(
                _connectionString,
                x => x.MigrationsAssembly("YourRest.Producer.Infrastructure"));

            return new SharedDbContext(optionsBuilder.Options);
        }
    }
}