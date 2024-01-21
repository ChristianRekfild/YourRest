using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using YourRest.Application.Dto.Mappers.Profiles;
using YourRest.Application.Dto.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure;
using YourRest.Producer.Infrastructure.Entities;
using YourRest.WebApi.Models;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class AddressControllerTests
    {
        private readonly SingletonApiTest fixture;
        private readonly SharedResourcesFixture _sharedFixture;
        private readonly IMapper mapper;

        public AddressControllerTests(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            var scope = fixture.Server.Host.Services.CreateScope();

            var tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
            _sharedFixture = new SharedResourcesFixture(tokenRepository);

            var config = new MapperConfiguration(
                cfg => {
                    cfg.AddProfile<AddressDtoProfile>();
                    cfg.AddProfile<ProducerInfrastructureMapper>();
                }
            );
            mapper = config.CreateMapper();
        }

        [Fact]
        public async Task AddAddress_WhenApiMethodInvokedWithValidAddress_ThenReturnsCreated()
        {
            var country = fixture.DbContext.Countries.FirstOrDefault(c => c.Name == "Russia");
            if (country == null)
            {
                country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            }
            var region = fixture.DbContext.Regions.FirstOrDefault(c => c.Name == "Московская область");
            if (region == null)
            {
                region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            }
            var city = fixture.DbContext.Cities.FirstOrDefault(c => c.Name == "Moscow");
            if (city == null)
            {
                city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            }
            var addressDto = CreateValidAddressDto(city.Id);

            var token = _sharedFixture.SharedAccessToken;

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/addresses", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressViewModel>(responseString);

            Assert.True(createdAddress?.Id > 0);
        }

        [Fact]
        public async Task GetAddress_WhenApiMethodInvokedId_ThenReturns200Ok()
        {
            var country = fixture.DbContext.Countries.FirstOrDefault(c => c.Name == "Russia");
            if (country == null)
            {
                country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            }
            var region = fixture.DbContext.Regions.FirstOrDefault(c => c.Name == "Московская область");
            if (region == null)
            {
                region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            }
            var city = fixture.DbContext.Cities.FirstOrDefault(c => c.Name == "Moscow");
            if (city == null)
            {
                city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            }
            var addressDto = CreateValidAddressDto(city.Id);
            var address = await fixture.InsertObjectIntoDatabase(mapper.Map<Address>(addressDto));
           
            var response = await fixture.Client.GetAsync($"api/addresses/{address.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<AddressViewModel>(responseString);

            Assert.Equal(createdAddress?.Id, address.Id);
        }

        private AddressDto CreateValidAddressDto(int cityId)
        {
            return new AddressDto
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = cityId
            };
        }
    }
}
