using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Text;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class SingletonApiTest : IDisposable, IAsyncLifetime
    {
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public SharedDbContext DbContext { get; private set; }

        private DatabaseFixture dbFixture;
        private KeycloakFixture keycloakFixture;

        public SingletonApiTest() { }

        public async Task InitializeAsync()
        {
            dbFixture = DatabaseFixture.getInstance();
            var connectionString = BuildConnectionString();

            DbContext = dbFixture.GetDbContext(connectionString);
        
            keycloakFixture = KeycloakFixture.GetInstanceAsync();
            await keycloakFixture.InitializeAsync();

            InitializeWebHost(connectionString);
        }
        public async Task<T> InsertObjectIntoDatabase<T>(T entity) where T : class
        {
            var item = await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return item.Entity;
        }
        
        public async Task CreateGroup(int accommodationId)
        {
            await keycloakFixture.CreateGroup(accommodationId);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await keycloakFixture.GetAccessTokenAsync();
        }

        public void CleanDatabase()
        {
            DbContext.ClearAllTables();
        }

        public void Dispose()
        {
            Server.Dispose();
        }
        
        public Task DisposeAsync()
        {
            Server.Dispose();
            return Task.CompletedTask;
        }
        
        private string BuildConnectionString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var part in dbFixture.ConnectionString.Split(';'))
            {
                if (part.StartsWith("Database"))
                {
                    sb.Append(part + "_" + Guid.NewGuid().ToString().Replace("-", string.Empty));
                }
                else
                {
                    sb.Append(part);
                }
                sb.Append(";");
            }
            return sb.ToString();
        }

        private void InitializeWebHost(string connectionString)
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var testConfig = new ConfigurationBuilder()
                        .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                        {
                            new("ConnectionStrings:DefaultConnection", connectionString),
                            new("Authority", keycloakFixture.GetTestRealmUrl()),
                            new("ClientId", keycloakFixture.GetTestAudience()),
                            new("ClientSecret", keycloakFixture.GetTestSymmetricKey())
                        })
                        .Build();

                    configBuilder.AddConfiguration(testConfig);
                })
                .UseStartup<Program>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

    }
}
