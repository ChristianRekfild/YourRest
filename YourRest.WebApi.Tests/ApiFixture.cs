using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using YourRest.WebApi;

namespace YourRest.WebApi.Tests
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

        private void InitializePostgresContainer()
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
                        .AddInMemoryCollection(new[]
                        {
                            new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", ConnectionString)
                        })
                        .Build();

                    configBuilder.AddConfiguration(testConfig);
                })
                .UseStartup<Program>();

            Server = new TestServer(builder);
        }
        
        public void Dispose()
        {
            PostgresContainer.StopAsync().Wait();
            Server?.Dispose();
        }
    }
}