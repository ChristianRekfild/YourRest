using System.Text;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.Producer.Infrastructure.Tests.Fixtures
{
public class SingletonApiTest : IDisposable
    {
        public SharedDbContext DbContext { get; private set; }

        private DatabaseFixture dbFixture;

        public SingletonApiTest() { 
            dbFixture = DatabaseFixture.getInstance();
            Console.WriteLine($"dbFixture is {(dbFixture == null ? "null" : "not null")}");

            var connectionString = BuildConnectionString();
            Console.WriteLine($"connectionString is {(connectionString == null ? "null" : "not null")}");

            DbContext = dbFixture.GetDbContext(connectionString);
            Console.WriteLine($"DbContext is {(DbContext == null ? "null" : "not null")}");
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

        public void Dispose()
        {
            dbFixture?.Dispose();
        } 
    }
}