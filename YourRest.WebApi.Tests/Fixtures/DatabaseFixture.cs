using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public SharedDbContext DbContext { get; private set; }
        public string ConnectionString { get; private set; }
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
            
            ConnectionString = _postgreSqlContainer.GetConnectionString();

            var builder = new DbContextOptionsBuilder<SharedDbContext>();
            builder.UseNpgsql(ConnectionString);
            DbContext = new SharedDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
