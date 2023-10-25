using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;
using System.Threading.Tasks;
using System.Threading;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private static DatabaseFixture? instance = null;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static readonly Lazy<DatabaseFixture> lazyInstance = new Lazy<DatabaseFixture>(() => new DatabaseFixture());

        public static DatabaseFixture Instance => lazyInstance.Value;

        public async Task<string> GetConnectionStringAsync()
        {
            if (_postgreSqlContainer.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
            {
                await semaphore.WaitAsync();

                try
                {
                    if (_postgreSqlContainer.State != DotNet.Testcontainers.Containers.TestcontainersStates.Running)
                    {
                        await _postgreSqlContainer.StartAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return _postgreSqlContainer.GetConnectionString();
        }

        private readonly PostgreSqlContainer _postgreSqlContainer;

        private DatabaseFixture()
        {
            _postgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15.4-alpine")
                .WithUsername("postgres")
                .WithPassword("postgres")
                //.WithPortBinding("5433") // For viewing in PgAdmin
                .WithDatabase("postgres")
                .WithCleanUp(true)
                .Build();
        }
        public static DatabaseFixture GetInstance() => Instance;

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
            DisposeAsync().Wait();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.StopAsync().ConfigureAwait(false);
            await _postgreSqlContainer.DisposeAsync().ConfigureAwait(false);
        }
    }
}
