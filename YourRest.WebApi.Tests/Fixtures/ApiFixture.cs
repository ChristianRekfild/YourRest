using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class ApiFixture : IDisposable
    {
        public PostgreSqlContainer PostgresContainer { get; private set; }
        public TestServer Server { get; private set; }
        public string ConnectionString => PostgresContainer.GetConnectionString();

        public ApiFixture()
        {
            InitializePostgresContainer();
            StartApplication();
        }

        private async void InitializePostgresContainer()
        {
            PostgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15.4-alpine")
                .WithUsername("admin")
                .WithPassword("admin")
                .WithDatabase("your_rest_postgres_test")
                .Build();

            PostgresContainer.StartAsync().Wait();
        }

        private void StartApplication()
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var testConfig = new ConfigurationBuilder()
                        .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                        {
                            new KeyValuePair<string, string?>("ConnectionStrings:DefaultConnection", ConnectionString)
                        })
                        .Build();

                    configBuilder.AddConfiguration(testConfig);
                })
                .UseStartup<Program>();

            Server = new TestServer(builder);
        }

        public async void Dispose()
        {
            await PostgresContainer.StopAsync();
            Server?.Dispose();
        }
    }
}
