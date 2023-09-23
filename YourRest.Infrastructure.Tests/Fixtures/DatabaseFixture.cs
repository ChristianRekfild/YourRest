using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using DoomedDatabases.Postgres;
using YourRest.Infrastructure.DbContexts;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace YourRest.Infrastructure.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public SharedDbContext DbContext { get; }
        public PostgreSqlContainer PostgresContainer { get; private set; }

        private ITestDatabase TestDatabase { get; }
        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            InitializePostgresContainer();
            //var port = PostgresContainer.GetMappedPublicPort();
            TestDatabase = new TestDatabaseBuilder().WithConfiguration(configuration).Build();
            //TestDatabase.Create();

            var builder = new DbContextOptionsBuilder<SharedDbContext>();
            //builder.UseNpgsql(TestDatabase.ConnectionString);
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            
            DbContext = new SharedDbContext(builder.Options);
            DbContext.Database.EnsureCreated();
        }

        private void InitializePostgresContainer()
        {
            PostgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15.4-alpine")
                .WithUsername("admin")
                .WithPassword("admin")
                .WithDatabase("your_rest_postgres_test")
                .WithExposedPort("5432")
                .WithPortBinding("5432")
                .Build();

            PostgresContainer.StartAsync().Wait();
        }

        public void Dispose()
        {
            //TestDatabase.Drop();
            PostgresContainer.StopAsync().Wait();
        }
    }
}
