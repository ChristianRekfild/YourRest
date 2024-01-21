using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using YourRest.Producer.Infrastructure.DbContexts;

namespace YourRest.Producer.Infrastructure.Tests.Fixtures
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
            builder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            SharedDbContext defaultDbContext = new SharedDbContext(builder.Options);
            defaultDbContext.Database.Migrate();
            return defaultDbContext;
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await _postgreSqlContainer.StopAsync();
                await _postgreSqlContainer.DisposeAsync();
            }).Wait();
        }
    }
}
