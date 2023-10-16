using System.Text.Json;
using YourRest.Application.Dto;
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

        [Fact(Skip = "Тест зависания")]
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

            await fixture.InsertObjectIntoDatabase(expectedCountry1);
            await fixture.InsertObjectIntoDatabase(expectedCountry2);


            await fixture.InsertObjectIntoDatabase(expectedRegion1);
            await fixture.InsertObjectIntoDatabase(expectedRegion2);

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

        [Fact(Skip = "Тест зависания")]
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


