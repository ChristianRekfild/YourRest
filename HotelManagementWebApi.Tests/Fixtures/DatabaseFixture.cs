using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace HotelManagementWebApi.Tests.Fixtures
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public HotelManagementDbContext DbContext { get; private set; }
        private PostgreSqlContainer _postgreSqlContainer { get; }
        public DatabaseFixture()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15.4-alpine")
            .WithUsername("admin")
            .WithPassword("admin")
            .WithPortBinding("5432")
            .WithDatabase("db")
            .WithCleanUp(true)
            .Build();
        }       

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();

            var builder = new DbContextOptionsBuilder<HotelManagementDbContext>();
            builder.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            DbContext = new HotelManagementDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }
    }
}
