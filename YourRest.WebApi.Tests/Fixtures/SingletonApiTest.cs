using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace YourRest.WebApi.Tests.Fixtures
{

    public class SingletonApiTest : IAsyncLifetime
    {
        public HttpClient Client { get; private set; }
        public TestServer Server { get; private set; }

        public readonly DatabaseFixture dbFixture;
        public SingletonApiTest()
        {
            dbFixture = new DatabaseFixture();
        }

        public async Task DisposeAsync()
        {
            await dbFixture.DisposeAsync();
            Server?.Dispose();
        }

        public async Task InitializeAsync()
        {
            await dbFixture.InitializeAsync();
            //dbFixture.DbContext.Database.Migrate();
            
            var testConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", dbFixture.ConnectionString),
                    new KeyValuePair<string, string>("JwtSettings:Authority", "http://keycloak:8080/auth/realms/YourRest"),
                    new KeyValuePair<string, string>("JwtSettings:Audience", "your_rest_app"),
                    new KeyValuePair<string, string>("JwtSettings:SymmetricKey", "qBC5V3wc2AYKTcYN1CACo6REU9t1Inrf")
                })
                .Build();
            
            Program.SetConfiguration(testConfig);

            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder.AddConfiguration(testConfig);
                })
                .UseStartup<Program>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }
    }
}
