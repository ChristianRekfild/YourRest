using System.Text.Json;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class RegionsControllerTest : IClassFixture<SingletonApiTest> //ApiTest
    {
        //public RegionsControllerTest(ApiFixture fixture) : base(fixture)
        //{
        //}

        private readonly SingletonApiTest fixture;
        public RegionsControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetAllRegions_ReturnsExpectedRegions_WhenDatabaseHasRegions()
        {
            var country1 = new Country { Name = "Russia" };
            var expectedCountry1 = await fixture.InsertObjectIntoDatabase(country1);
            var expectedRegion1 = new Region();
            expectedRegion1.Name = "Region1";
            expectedRegion1.CountryId = expectedCountry1.Id;
            await fixture.InsertObjectIntoDatabase(expectedRegion1);
            
            var country2 = new Country { Name = "Test" };
            var expectedCountry2 = await fixture.InsertObjectIntoDatabase(country2);
            var expectedRegion2 = new Region();
            expectedRegion2.Name = "Region2";
            expectedRegion2.CountryId = expectedCountry2.Id;
            await fixture.InsertObjectIntoDatabase(expectedRegion2);

            var resultRegions = await GetRegionFromApi();

            Assert.NotNull(resultRegions);
            Assert.Contains(resultRegions, region => region.Name == "Region1" && region.CountryId == expectedRegion1.CountryId);
            Assert.Contains(resultRegions, region => region.Name == "Region2" && region.CountryId == expectedRegion2.CountryId);
        }

        [Fact]
        public async Task GetAllRegions_ReturnsEmptyList_WhenDatabaseIsEmpty()
        {
            var regions = fixture.DbContext.Regions.ToList();
            fixture.DbContext.Regions.RemoveRange(regions);
            await fixture.DbContext.SaveChangesAsync();
            var resultCountries = await GetRegionFromApi();

            Assert.NotNull(resultCountries);
            Assert.Empty(resultCountries);
        }

        private async Task<List<RegionDto>> GetRegionFromApi()
        {
            var response = await fixture.Client.GetAsync("/api/regions");
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


