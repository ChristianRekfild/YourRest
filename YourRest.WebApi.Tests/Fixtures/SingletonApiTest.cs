using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Text;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class SingletonApiTest : IDisposable
    {
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }
        public SharedDbContext DbContext { get; private set; }

        private readonly DatabaseFixture dbFixture;

        public SingletonApiTest()
        {
            dbFixture = DatabaseFixture.getInstance();

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
            var connectionString = sb.ToString();

            DbContext = dbFixture.GetDbContext(connectionString);

            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var testConfig = new ConfigurationBuilder()
                        .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                        {
                            new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", connectionString),
                            new KeyValuePair<string, string>("JwtSettings:Authority", "http://keycloak:8080/auth/realms/YourRest"),
                            new KeyValuePair<string, string>("JwtSettings:Audience", "your_rest_app"),
                            new KeyValuePair<string, string>("JwtSettings:SymmetricKey", "qBC5V3wc2AYKTcYN1CACo6REU9t1Inrf")
                        })
                        .Build();

                    configBuilder.AddConfiguration(testConfig);
                })
                .UseStartup<Program>();

            Server = new TestServer(builder);

            Client = Server.CreateClient();
        }

        public async Task<T> InsertObjectIntoDatabase<T>(T entity) where T : class
        {
            var item = await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return item.Entity;
        }

        public void CleanDatabase()
        {
            DbContext.ClearAllTables();
        }

        public void Dispose()
        {
            Server?.Dispose();

            //TODO: Подумать как останавливать контейнер
            // и нужно ли это делать
            //dbFixture.Dispose();
        }
    }
}
