using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.Producer.Infrastructure.Keycloak.Http;
using YourRest.Producer.Infrastructure.Keycloak.Repositories;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class SingletonApiTest : IDisposable
    {
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public SharedDbContext DbContext { get; private set; }

        private DatabaseFixture dbFixture;
        private KeycloakFixture keycloakFixture;

        public SingletonApiTest()
        {
            var projectTask = Task.Run(() =>
            {
                dbFixture = DatabaseFixture.getInstance();
                Console.WriteLine($"dbFixture is {(dbFixture == null ? "null" : "not null")}");

                var connectionString = BuildConnectionString();
                Console.WriteLine($"connectionString is {(connectionString == null ? "null" : "not null")}");

                DbContext = dbFixture.GetDbContext(connectionString);
                Console.WriteLine($"DbContext is {(DbContext == null ? "null" : "not null")}");

                InitializeWebHost(connectionString);
            });

            var keycloakTask = Task.Run(() =>
            {
                keycloakFixture = KeycloakFixture.Instance();
                Console.WriteLine($"keycloakFixture is {(keycloakFixture == null ? "null" : "not null")}");
            });

            Task.WaitAll(projectTask, keycloakTask);
            //keycloakFixture.EnsureInitialized();
            //keycloakFixture.Start();


        }


        private string BuildConnectionString()
        {
            var originalConnectionString = dbFixture.ConnectionString;
            StringBuilder sb = new StringBuilder();
            foreach (var part in originalConnectionString.Split(';'))
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

        public async Task<T> InsertObjectIntoDatabase<T>(T entity) where T : class
        {
            var item = await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return item.Entity;
        }
        
        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public void CleanDatabase()
        {
            DbContext.ClearAllTables();
        }

        public void Dispose()
        {
            Server.Dispose();
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
                            new("KeycloakSetting:Authority", "http://localhost:8083/auth/realms/YourRest"),
                            new("KeycloakSetting:ClientId",  "your_rest_app"),
                            new("KeycloakSetting:RealmName",  "YourRest"),
                            new("KeycloakSetting:KeycloakUrl",  "http://localhost:8083"),
                            new("KeycloakSetting:ClientSecret", "qBC5V3wc2AYKTcYN1CACo6REU9t1Inrf"),
                            new("AWS:AccessKey",  "test"),
                            new("AWS:SecretKey",  "test"),
                            new("AWS:ServiceURL",  "http://test:8081"),
                            new("AWS:BucketName", "test")
                        })
                        .Build();

                    configBuilder.AddConfiguration(testConfig);
                }).UseStartup<Program>();

            builder.ConfigureServices(services =>
            {
                services.AddHttpClient();
                services.AddScoped<ITokenRepository, TokenRepository>();
                services.AddSingleton<ICustomHttpClientFactory, TestHttpClientFactory>();

            });

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public async Task<Accommodation?> GetAccommodationById(int createdAccommodationId)
        {
            return await DbContext
                .Set<Accommodation>()
                .Include(x => x.UserAccommodations)
                .Include(x => x.AccommodationFacilities)
                .FirstOrDefaultAsync(x => x.Id.Equals(createdAccommodationId));
        }
    }
}
