using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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

        private static int index = 0;
        private static readonly object syncObj = new object();
        public SingletonApiTest()
        {
            dbFixture = DatabaseFixture.getInstance();

            string connectionString = string.Empty;

            if (index == 0)
            {
                lock (syncObj)
                {
                    if (index == 0)
                    {
                        connectionString = dbFixture.ConnectionString;
                    }
                    index++;
                }
            }
            if (string.IsNullOrEmpty(connectionString))
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
                connectionString = sb.ToString();
            }

            DbContext = dbFixture.GetDbContext(connectionString);

            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    var testConfig = new ConfigurationBuilder()
                        .AddInMemoryCollection(new[]
                        {
                            new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", connectionString)
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

            if (index > 1)
            {
                lock (syncObj)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                }
            }
            if (index <= 0)
            {
                dbFixture.Dispose();
            }
        }
    }
}
