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

namespace YourRest.WebApi.Tests.BookingContext.Infrastructure.Adapters.Controllers
{
    public class CountriesControllerTest : ApiTest
    {
        public CountriesControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllCountries_ReturnsExpectedCountries_WhenDatabaseHasCountries()
        {
            var expectedCountry1 = new Country { Name = "Russia" };
            var expectedCountry2 = new Country { Name = "Test" };
            await InsertCountryIntoDatabase(expectedCountry1);
            await InsertCountryIntoDatabase(expectedCountry2);

            var resultCountries = await GetCountriesFromApi();

            Assert.NotNull(resultCountries);
            Assert.Collection(resultCountries,
                country => Assert.Equal("Russia", country.Name),
                country => Assert.Equal("Test", country.Name));
        }

        [Fact]
        public async Task GetAllCountries_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var resultCountries = await GetCountriesFromApi();

            Assert.NotNull(resultCountries);
            Assert.Empty(resultCountries);
        }

        private async Task<List<CountryDto>> GetCountriesFromApi()
        {
            var response = await Client.GetAsync("/api/countries");
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
