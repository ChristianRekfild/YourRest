using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Infrastructure.Core.DbContexts;

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
            //.WithPortBinding("5432") // Для просмотра в PgAdmin
            .WithDatabase("db")
            .WithCleanUp(true)
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            var builder = new DbContextOptionsBuilder<SharedDbContext>();
            builder.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            DbContext = new SharedDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
