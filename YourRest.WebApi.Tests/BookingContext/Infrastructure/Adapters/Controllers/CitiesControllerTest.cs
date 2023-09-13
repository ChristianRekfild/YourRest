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
using YourRest.WebApi.BookingContext.Application.Dto;
using YourRest.WebApi.Tests;

namespace YourRest.WebApi.Tests.BookingContext.Infrastructure.Adapters.Controllers
{
    public class CitiesControllerTest : ApiTest
    {
        public CitiesControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllcities_ReturnsExpectedCities_WhenDatabaseHasCities()
        {
            var expectedCity1 = new City { Name = "Moscow" };
            var expectedCity2 = new City { Name = "TestCity" };
            await InsertCityIntoDatabase(expectedCity1);
            await InsertCityIntoDatabase(expectedCity2);

            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Collection(resultCities,
                city => Assert.Equal("Moscow", city.Name),
                city => Assert.Equal("TestCity", city.Name));
        }

        [Fact]
        public async Task GetAllCities_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Empty(resultCities);
        }

        [Fact]
        public async Task GetCityById_ReturnsExpectedCity_WhenDatabaseHasCitiesByNeedId()
        {
            var expectedCity1 = new City 
            {
                Id = 1,
                Name = "Moscow"
            };
            var expectedCity2 = new City 
            {
                Id = 2,
                Name = "TestCity" 
            };
            await InsertCityIntoDatabase(expectedCity1);
            await InsertCityIntoDatabase(expectedCity2);

            var resultCity1 = await GetCityByIdFromApi(1);
            var resultCity2 = await GetCityByIdFromApi(2);

            Assert.NotNull(resultCity1);
            Assert.NotNull(resultCity2);

            Assert.Equal("Moscow", expectedCity1.Name);
            Assert.Equal("TestCity", expectedCity2.Name);
        }

        [Fact]
        public async Task GetCityById_ReturnsExpectedNull_WhenDatabaseHasNoCitiesByNeedId()
        {
            var resultCity = await GetCityByIdFromApi(1);

            Assert.Null(resultCity);
        }

        private async Task<List<City>> GetCitiesFromApi()
        {
            var response = await Client.GetAsync("/api/cities");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = JsonSerializer.Deserialize<List<City>>(content, options);
            
            return cities;
        }

        private async Task<City> GetCityByIdFromApi(int id)
        {
            var response = await Client.GetAsync($"/api/cities/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            var context = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var city = JsonSerializer.Deserialize<City>(context, options);

            return city;
        }
    }
}
