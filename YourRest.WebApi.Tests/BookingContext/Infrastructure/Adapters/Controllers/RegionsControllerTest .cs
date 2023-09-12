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
    public class RegionsControllerTest : ApiTest
    {
        public RegionsControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllRegions_ReturnsExpectedRegions_WhenDatabaseHasRegions()
        {
            var expectedRegion1 = new Region();
            expectedRegion1.Name = "Ленинградская";
            expectedRegion1.CountryId = 1;
            var expectedRegion2 = new Region();
            expectedRegion2.Name = "Витебская";
            expectedRegion2.CountryId = 2;

            var expectedCountry1 = new Country { Name = "Russia" };
            var expectedCountry2 = new Country { Name = "Test" };

            await InsertObjectIntoDatabase(expectedCountry1);
            await InsertObjectIntoDatabase(expectedCountry2);


            await InsertObjectIntoDatabase(expectedRegion1);
            await InsertObjectIntoDatabase(expectedRegion2);

            var resultRegions = await GetRegionFromApi();

            Assert.NotNull(resultRegions);
            Assert.Collection(resultRegions,
                region =>
                {
                    Assert.Equal("Ленинградская", region.Name);
                    Assert.Equal(1, region.CountryId);
                },
            region =>
            {
                Assert.Equal("Витебская", region.Name);
                Assert.Equal(2, region.CountryId);
            });
        }

        [Fact]
        public async Task GetAllRegions_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var resultCountries = await GetRegionFromApi();

            Assert.NotNull(resultCountries);
            Assert.Empty(resultCountries);
        }

        private async Task<List<RegionDto>> GetRegionFromApi()
        {
            var response = await Client.GetAsync("/api/regions");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var regions = JsonSerializer.Deserialize<List<RegionDto>>(content, options);

            return regions;
        }

    }
}


