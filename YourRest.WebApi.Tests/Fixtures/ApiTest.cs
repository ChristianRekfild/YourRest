using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.WebApi.Tests.Fixtures
{
    public abstract class ApiTest : IClassFixture<ApiFixture>, IDisposable
    {
        protected readonly ApiFixture Fixture;
        protected readonly HttpClient Client;

        protected ApiTest(ApiFixture fixture)
        {
            Fixture = fixture;
            Client = Fixture.Server.CreateClient();
            
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            context.Database.Migrate();
        }

        protected async Task<T> InsertObjectIntoDatabase<T>(T entity) where T : class
        {
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            var result = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            
            return result.Entity;
        }

        protected void CleanDatabase()
        {
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            
            context.ClearAllTables();
        }

        public virtual void Dispose()
        {
            CleanDatabase();
        }
    }

}