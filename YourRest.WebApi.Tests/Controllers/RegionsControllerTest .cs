using System.Text.Json;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
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
            expectedRegion1.Name = "Region1";
            expectedRegion1.CountryId = 1;
            var expectedRegion2 = new Region();
            expectedRegion2.Name = "Region2";
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
                    Assert.Equal("Region1", region.Name);
                    Assert.Equal(1, region.CountryId);
                },
            region =>
            {
                Assert.Equal("Region2", region.Name);
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


