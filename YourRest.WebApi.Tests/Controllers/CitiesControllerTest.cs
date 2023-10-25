using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;
using SystemJson = System.Text.Json;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class CitiesControllerTest
    {
        private SingletonApiTest fixture;
        public CitiesControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetAllCities_ReturnsExpectedCities_WhenDatabaseHasCities()
        {
            var expectedCountry = new Country()
            {
                Name = "TestCountry"
            };

            expectedCountry = await fixture.InsertObjectIntoDatabase(expectedCountry);

            var expectedRegion = new Region()
            {
                Name = "TestRegion",
                CountryId = expectedCountry.Id

            };

            expectedRegion = await fixture.InsertObjectIntoDatabase(expectedRegion);

            var expectedCity1 = new City { Name = "Moscow", RegionId = expectedRegion.Id /*, Id = 1*/ };
            var expectedCity2 = new City { Name = "TestCity", RegionId = expectedRegion.Id /*, Id = 2*/ };


            expectedCity1 = await fixture.InsertObjectIntoDatabase(expectedCity1);
            expectedCity2 = await fixture.InsertObjectIntoDatabase(expectedCity2);
            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Equal(expectedCity1.Name, resultCities.FirstOrDefault(c => c.Id == expectedCity1.Id)?.Name);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GetAllCities_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var count = fixture.DbContext.Cities.Count();
            var resultCities = await GetCitiesFromApi();

            Assert.NotNull(resultCities);
            Assert.Equal(count, resultCities.Count);
        }

        [Fact]
        public async Task GetCityById_ReturnsExpectedCity_WhenDatabaseHasCitiesByNeedId()
        {
            var countryEntity = new Country { Name = "Russia" };
            var country = await fixture.InsertObjectIntoDatabase(countryEntity);

            var regionEntity = new Region { Name = "test", CountryId = country.Id };
            var region = await fixture.InsertObjectIntoDatabase(regionEntity);

            var expectedCity1 = new City { Name = "Moscow", RegionId = region.Id };
            var expectedCity2 = new City { Name = "TestCity", RegionId = region.Id };

            expectedCity1 = await fixture.InsertObjectIntoDatabase(expectedCity1);
            expectedCity2 = await fixture.InsertObjectIntoDatabase(expectedCity2);

            var resultCity1 = await GetCityByIdFromApi(expectedCity1.Id);
            var resultCity2 = await GetCityByIdFromApi(expectedCity2.Id);

            Assert.NotNull(resultCity1);
            Assert.NotNull(resultCity2);

            Assert.Equal("Moscow", expectedCity1.Name);
            Assert.Equal("TestCity", expectedCity2.Name);
        }

        [Fact]
        public async Task GetCityById_ReturnsExpectedNull_WhenDatabaseHasNoCitiesByNeedId()
        {
            var maxId = 1000;
            if (await fixture.DbContext.Cities.AnyAsync())
            {
                maxId += await fixture.DbContext.Cities.Select(c => c.Id).MaxAsync();
            }

            var invalidCity = new CityDTO
            {
                Id = maxId,
                Name = "Test"
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidCity), Encoding.UTF8, "application/json");
            var response = await fixture.Client.GetAsync($"/api/cities/{invalidCity.Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCityByCountryId_ReturnsExpectedCityList_WhenDatabaseHasCitiesByNeedCountryId()
        {
            var countryEntity = new Country { Name = "Test" };
            var country = await fixture.InsertObjectIntoDatabase(countryEntity);

            var regionEntity = new Region { Name = "Test", CountryId = country.Id };
            var region = await fixture.InsertObjectIntoDatabase(regionEntity);

            var expectedCity1 = new City { Name = "Moscow666", RegionId = region.Id };
            var expectedCity2 = new City { Name = "TestCity666", RegionId = region.Id };

            await fixture.DbContext.Cities.AddAsync(expectedCity1);
            await fixture.DbContext.Cities.AddAsync(expectedCity2);
            await fixture.DbContext.SaveChangesAsync();

            var response = await fixture.Client.GetAsync($"/api/cities?country_id={country.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);

            var city1 = cities?.FirstOrDefault(x => x.Name == "Moscow666");
            var city2 = cities?.FirstOrDefault(x => x.Name == "TestCity666");

            Assert.NotNull(city1);
            Assert.NotNull(city2);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GetCityByCountryId_Returns404_WhenDatabaseDoesntHaveCitiesByNeedCountryId()
        {
            int countryId = int.MaxValue;
            var response = await fixture.Client.GetAsync($"/api/cities?countryId={countryId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCityByRegionId_ReturnsExpectedCityList_WhenDatabaseHasCitiesByNeedRegionId()
        {
            var countryEntity = new Country { Name = "Test" };
            var country = await fixture.InsertObjectIntoDatabase(countryEntity);

            var regionEntity = new Region { Name = "Test", CountryId = country.Id };
            var region = await fixture.InsertObjectIntoDatabase(regionEntity);

            var expectedCity1 = new City { Name = "Moscow666", RegionId = region.Id };
            var expectedCity2 = new City { Name = "TestCity666", RegionId = region.Id };

            await fixture.DbContext.Cities.AddAsync(expectedCity1);
            await fixture.DbContext.Cities.AddAsync(expectedCity2);
            await fixture.DbContext.SaveChangesAsync();

            var response = await fixture.Client.GetAsync($"/api/cities?regionId={region.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);

            var city1 = cities?.FirstOrDefault(x => x.Name == "Moscow666");
            var city2 = cities?.FirstOrDefault(x => x.Name == "TestCity666");

            Assert.NotNull(city1);
            Assert.NotNull(city2);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GetCityByRegionId_Returns200Ok_WhenDatabaseDoesntHaveCitiesByGivenRegionId()
        {
            var response = await fixture.Client.GetAsync($"/api/cities?regionId=200");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var cities = JsonConvert.DeserializeObject<IEnumerable<CityDTO>>(content);
            Assert.NotNull(cities);
            Assert.Empty(cities);
        }

        private async Task<List<City>> GetCitiesFromApi()
        {
            var response = await fixture.Client.GetAsync("/api/cities");
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
            var response = await fixture.Client.GetAsync($"/api/cities/{id}");

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

        private async Task<List<City>> GetCitiesByRegionId(int regionId)
        {
            var response = await fixture.Client.GetAsync($"/api/cities?regionId={regionId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);

            return cities;
        }

        private async Task<List<City>> GetCitiesByCountryId(int countryId)
        {
            var response = await fixture.Client.GetAsync($"/api/cities?countryId={countryId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var cities = SystemJson.JsonSerializer.Deserialize<List<City>>(content, options);

            return cities;
        }
    }
}
