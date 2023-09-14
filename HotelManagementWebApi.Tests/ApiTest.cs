using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Collections.Generic;
using Xunit;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using HotelManagementWebApi;

namespace HotelManagementWebApi.Tests
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
            var context = scope.ServiceProvider.GetRequiredService<HotelManagementDbContext>();
            context.Database.Migrate();
        }

        protected async Task<int> InsertObjectIntoDatabase<T>(T entity) where T : class
        {
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelManagementDbContext>();
            context.Add(entity);
            
            return await context.SaveChangesAsync();
        }

        protected void CleanDatabase()
        {
            using var scope = Fixture.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelManagementDbContext>();
            
            context.ClearAllTables();
        }

        public virtual void Dispose()
        {
            CleanDatabase();
        }
    }

}