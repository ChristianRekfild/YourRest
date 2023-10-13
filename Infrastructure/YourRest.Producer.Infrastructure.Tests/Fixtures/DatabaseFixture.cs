using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;

namespace YourRest.Infrastructure.Tests.Fixtures
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public SharedDbContext? DbContext { get; private set; }
        private PostgreSqlContainer _postgreSqlContainer { get; }
        public DatabaseFixture()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15.4-alpine")
                .WithUsername("admin")
                .WithPassword("admin")
            //.WithPortBinding("5432") // ��� ��������� � PgAdmin
            .WithDatabase("db")
            .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            var migrationsAssembly = typeof(YourRest.Producer.Infrastructure.DependencyInjections).Assembly.GetName().Name;
            await _postgreSqlContainer.StartAsync();

            var builder = new DbContextOptionsBuilder<SharedDbContext>();
            builder.UseNpgsql(_postgreSqlContainer.GetConnectionString(), sql => sql.MigrationsAssembly(migrationsAssembly));
            DbContext = new SharedDbContext(builder.Options);
            DbContext.Database.MigrateAsync().Wait();
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
