using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Collections.Generic;
using Xunit;
using YourRest.Infrastructure.DbContexts;
using YourRest.WebApi;
using SharedKernel.Domain.Entities;

namespace YourRest.WebApi.Tests
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

        protected async Task InsertObjectIntoDatabase<T>(T entity)
        {
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            context.Add(entity);
            await context.SaveChangesAsync();
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