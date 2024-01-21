using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourRest.Producer.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Core.DbContexts
{
    public class ClientAppIdentityContext : IdentityDbContext
    {
        static ClientAppIdentityContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public ClientAppIdentityContext() : base() { }

        public ClientAppIdentityContext(DbContextOptions<SharedDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn = "Host=localhost;Database=your_rest_client_identity;Username=admin;Password=admin;Port=5433";
                optionsBuilder.UseNpgsql(conn, sql => sql.MigrationsAssembly("YourRest.ClientIdentity.Infrastructure"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
