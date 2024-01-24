using System.Text.Json;
using YourRest.Application.Dto.Models;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class CountriesControllerTest
    {
        private readonly SingletonApiTest fixture;
        public CountriesControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetAllCountries_ReturnsExpectedCountries_WhenDatabaseHasCountries()
        {
            var expectedCountry1 = new Country { Name = "Russia" };
            var expectedCountry2 = new Country { Name = "Test" };
            await fixture.InsertObjectIntoDatabase(expectedCountry1);
            await fixture.InsertObjectIntoDatabase(expectedCountry2);

            var resultCountries = await GetCountriesFromApi();

            Assert.NotNull(resultCountries);
            Assert.Contains(resultCountries, country => country.Name == "Russia");
            Assert.Contains(resultCountries, country => country.Name == "Test");
            fixture.CleanDatabase();
        }

        [Fact(Skip = "После перехода на общую базу этот тест в общем потоке мешает")]
        public async Task GetAllCountries_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            //fixture.DbContext.Countries.RemoveRange(fixture.DbContext.Countries);
            await fixture.DbContext.SaveChangesAsync();
            var resultCountries = await GetCountriesFromApi();

            Assert.NotNull(resultCountries);
            Assert.Empty(resultCountries);
        }

        private async Task<List<CountryDto>> GetCountriesFromApi()
        {
            var response = await fixture.Client.GetAsync("/api/countries");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var countries = JsonSerializer.Deserialize<List<CountryDto>>(content, options);
            
            return countries;
        }
    }
}
