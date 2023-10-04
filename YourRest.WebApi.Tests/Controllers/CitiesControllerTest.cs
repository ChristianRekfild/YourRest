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
            var expectedCountry = new Country()
            {
                Name = "TestCountry"
            };

           

            expectedCountry = (await _context.Countries.AddAsync(expectedCountry)).Entity;
            await _context.SaveChangesAsync();

            var expectedRegion = new Region()
            {
                Name = "TestRegion",
                CountryId = expectedCountry.Id

            };

            expectedRegion = (await _context.Regions.AddAsync(expectedRegion)).Entity;
            await _context.SaveChangesAsync();

            var expectedCity1 = new City { Name = "Moscow", RegionId = expectedRegion.Id /*, Id = 1*/ };
            var expectedCity2 = new City { Name = "TestCity", RegionId = expectedRegion.Id /*, Id = 2*/ };
            //await _apiTest.InsertObjectIntoDatabase(expectedCity1);
            //await _apiTest.InsertObjectIntoDatabase(expectedCity2);
            expectedCity1 = (await _context.Cities.AddAsync(expectedCity1)).Entity;
            expectedCity2 = (await _context.Cities.AddAsync(expectedCity2)).Entity;
            await _context.SaveChangesAsync();

            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Equal(expectedCity1.Name, resultCities.FirstOrDefault(c => c.Id == expectedCity1.Id)?.Name);
            Assert.Equal(expectedCity2.Name, resultCities.FirstOrDefault(c => c.Id == expectedCity2.Id)?.Name);
            //Assert.Collection(resultCities,
            //    city => Assert.Equal("Moscow", city.Name),
            //    city => Assert.Equal("TestCity", city.Name));
        }

        [Fact]
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
            var expectedCountry = new Country()
            {
                Name = "TestCountry"
            };



            expectedCountry = (await _context.Countries.AddAsync(expectedCountry)).Entity;
            await _context.SaveChangesAsync();

            var expectedRegion = new Region()
            {
                Name = "TestRegion",
                CountryId = expectedCountry.Id

            };

            expectedRegion = (await _context.Regions.AddAsync(expectedRegion)).Entity;
            await _context.SaveChangesAsync();

            var expectedCity1 = new City 
            {
                Name = "Moscow",
                RegionId = expectedRegion.Id
            };
            var expectedCity2 = new City 
            {
                Name = "TestCity",
                RegionId = expectedRegion.Id
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

        //[Fact]
        //public async Task GetCityByRegionId_ReturnsExpectedCityList_WhenDatabaseHasCitiesByNeedRegionId()
        //{
        //    var expectedRegion = new Region
        //    {
        //        Id = 1,
        //        Name = "TestRegion"
        //    };

        //    var expectedCity1 = new City
        //    {
        //        Name = "Moscow",
        //        Region = expectedRegion,
        //        RegionId = expectedRegion.Id
        //    };

        //    var expectedCity2 = new City
        //    {
        //        Name = "TestCity",
        //        Region = expectedRegion,
        //        RegionId = expectedRegion.Id
        //    };

        //    await _context.Regions.AddAsync(expectedRegion);

        //    await _context.Cities.AddAsync(expectedCity1);
        //    await _context.Cities.AddAsync(expectedCity2);
        //    await _context.SaveChangesAsync();

        //    var resultCityList = await GetCitiesByRegionId(expectedRegion.Id);

        //    Assert.Contains(expectedCity1, resultCityList);
        //    Assert.Contains(expectedCity2, resultCityList);
        //}

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

        //private async Task<List<City>> GetCitiesByRegionId(int regionId)
        //{
        //    var response = await Client.GetAsync($"/api/cities/regionid={regionId}");

        //    if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

        //    response.EnsureSuccessStatusCode();
        //    var content = await response.Content.ReadAsStringAsync();
        //    var options = new SystemJson.JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };
        //    var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);

        //    return cities;
        //}
    }
}
