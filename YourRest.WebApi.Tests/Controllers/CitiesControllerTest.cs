using System.Net;
using System.Text;
using SystemJson = System.Text.Json;
using Newtonsoft.Json;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class CitiesControllerTest : ApiTest
    {
        public CitiesControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllcities_ReturnsExpectedCities_WhenDatabaseHasCities()
        {
            var expectedCity1 = new City { Name = "Moscow", Id = 1 };
            var expectedCity2 = new City { Name = "TestCity", Id = 2  };
            await InsertObjectIntoDatabase(expectedCity1);
            await InsertObjectIntoDatabase(expectedCity2);

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
            await InsertObjectIntoDatabase(expectedCity1);
            await InsertObjectIntoDatabase(expectedCity2);

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
            var invalidCity = new CityDTO
            {
                Id = 13,
                Name = "Test"
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidCity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"/api/cities/{invalidCity.Id}", content);

            //Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

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
