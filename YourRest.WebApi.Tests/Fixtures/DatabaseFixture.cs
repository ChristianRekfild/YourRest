using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private static DatabaseFixture? instance = null;
        private static readonly object syncObj = new object();

        public string ConnectionString
        {
            get
            {
                if (_postgreSqlContainer.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
                {
                    lock (syncObj)
                    {
                        if (_postgreSqlContainer.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
                        {
                            _postgreSqlContainer.ConfigureAwait(false);
                            Task.Run(() =>  _postgreSqlContainer.StartAsync()).GetAwaiter().GetResult();
                        }
                    }
                }
                return _postgreSqlContainer.GetConnectionString();
            }
        }

        private PostgreSqlContainer _postgreSqlContainer { get; }
        private DatabaseFixture()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15.4-alpine")
            .WithUsername("postgres")
            .WithPassword("postgres")
            //.WithPortBinding("5433") // Для просмотра в PgAdmin
            .WithDatabase("postgres")
            .WithCleanUp(true)
            .Build();
        }

        public static DatabaseFixture getInstance()
        {
            if (instance == null)
            {
                lock (syncObj)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseFixture();
                    }
                }
            }
            return instance;
        }

        public SharedDbContext GetDbContext(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<SharedDbContext>();
            var migrationsAssembly = typeof(ProducerInfrastructureDependencyInjections).Assembly.GetName().Name;
            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            SharedDbContext defaultDbContext = new SharedDbContext(builder.Options);
            defaultDbContext.Database.Migrate();
            return defaultDbContext;
        }

        public void Dispose()
        {
            Task.Run(async () => await _postgreSqlContainer.StopAsync()).Wait();
            Task.Run(async () => await _postgreSqlContainer.DisposeAsync()).Wait();
        }
    }
}
