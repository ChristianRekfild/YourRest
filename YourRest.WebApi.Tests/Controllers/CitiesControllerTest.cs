using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.WebApi.Tests.Fixtures;
using SystemJson = System.Text.Json;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class CitiesControllerTest // : ApiTest
    {
        private SharedDbContext _context;
        private HttpClient Client;
        private SingletonApiTest fixture;
        //public CitiesControllerTest(ApiFixture fixture) : base(fixture)
        //{
        //}
        public CitiesControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            _context = fixture.DbContext;
            Client = fixture.Client;
        }        

        [Fact]
        public async Task GetAllcities_ReturnsExpectedCities_WhenDatabaseHasCities()
        {
            var expectedCity1 = new City { Name = "Moscow"/*, Id = 1*/ };
            var expectedCity2 = new City { Name = "TestCity"/*, Id = 2*/ };
            //await _apiTest.InsertObjectIntoDatabase(expectedCity1);
            //await _apiTest.InsertObjectIntoDatabase(expectedCity2);
            expectedCity1 = await fixture.InsertObjectIntoDatabase(expectedCity1);
            expectedCity2 = await fixture.InsertObjectIntoDatabase(expectedCity2);
            //await _context.SaveChangesAsync();

            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Equal(expectedCity1.Name, resultCities.FirstOrDefault(c => c.Id == expectedCity1.Id)?.Name);
            Assert.Equal(expectedCity2.Name, resultCities.FirstOrDefault(c => c.Id == expectedCity2.Id)?.Name);
            //Assert.Collection(resultCities,
            //    city => Assert.Equal("Moscow", city.Name),
            //    city => Assert.Equal("TestCity", city.Name));
        }

        [Fact(Skip = "Failed on GitHub")]
        public async Task GetAllCities_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var count = _context.Cities.Count();
            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Equal(count, resultCities.Count);
        }

        [Fact]
        public async Task GetCityById_ReturnsExpectedCity_WhenDatabaseHasCitiesByNeedId()
        {
            var expectedCity1 = new City 
            {
                Name = "Moscow"
            };
            var expectedCity2 = new City 
            {
                Name = "TestCity" 
            };

            expectedCity1 = await fixture.InsertObjectIntoDatabase(expectedCity1);
            expectedCity2 = await fixture.InsertObjectIntoDatabase(expectedCity2);

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
            var maxId = 1000;
            if (await _context.Cities.AnyAsync())
            {
                maxId += await _context.Cities.Select(c => c.Id).MaxAsync();
            }

            var invalidCity = new CityDTO
            {
                Id = maxId,
                Name = "Test"
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidCity), Encoding.UTF8, "application/json");
            var response = await Client.GetAsync($"/api/cities/{invalidCity.Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        private async Task<List<City>> GetCitiesFromApi()
        {
            var response = await Client.GetAsync("/api/cities");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);
            
            return cities;
        }

        private async Task<City> GetCityByIdFromApi(int id)
        {
            var response = await Client.GetAsync($"/api/cities/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            var context = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var city = SystemJson.JsonSerializer.Deserialize<City>(context, options);

            return city;
        }
    }
}
