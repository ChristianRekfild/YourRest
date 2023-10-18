using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.WebApi.Tests.Fixtures;
using YourRest.WebApi.Responses;
using YourRest.Application.Dto.ViewModels;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace YourRest.WebApi.Tests.Controllers
{

    public class AccommodationControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public AccommodationControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GivenAccommodation_WhenApiMethodInvokedWithValidAddress_ThenReturns200Ok()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "FirstHotel" });
            var addressDto = CreateValidAddressDto(city.Id);
            
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodation.Id}/address", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);
            
            Assert.True(createdAddress?.Id > 0);
        }

        [Fact]
        public async Task GivenAccommodationAndExistAddressInDB_WhenApiMethodInvokedWithTheSameAddress_ThenReturns200Ok()
        {

            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var address = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "First street",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 80,
            });
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "SecondHotel" });
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Second street";
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodation.Id}/address", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdAddress = await response.Content.ReadFromJsonAsync<ResultDto>();

            Assert.True(createdAddress?.Id > 0);
        }

        [Fact]
        public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var accommodationId = -1;
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Thrid street";

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);
           
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {accommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorMassage, expectedMessageJson);
        }

        [Fact]
        public async Task GivenNonexistentCity_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns400()
        {
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation  { Name = "ThirdHotel" });
            var addressDto = new AddressDto
            {
                Street = "Fourth Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodation.Id}/address", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = "City with id 100 not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorResponseString, expectedMessageJson);
        }

        [Fact]
        public async Task GivenInvalidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "FourthHotel" });
            var addressDto = new AddressDto
            {
                Street = "",
                ZipCode = "",
                Longitude = 190,
                Latitude = 190,
                CityId = -1
            };
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodation.Id}/address", content);
            var errorData = await response.Content.ReadFromJsonAsync<ErrorViewModel>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("'Street' must not be empty.", errorData?.ValidationErrors[nameof(addressDto.Street)][0]);
            Assert.Equal("'Zip Code' must not be empty.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][0]);
            Assert.Equal("'Zip Code' is not in the correct format.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][1]);
            Assert.Equal("'Latitude' must be between -90 and 90. You entered 190.", errorData?.ValidationErrors[nameof(addressDto.Latitude)][0]);
            Assert.Equal("'Longitude' must be between -180 and 180. You entered 190.", errorData?.ValidationErrors[nameof(addressDto.Longitude)][0]);
            Assert.Equal("'City Id' must be greater than '0'.", errorData?.ValidationErrors[nameof(addressDto.CityId)][0]);
        }

        [Fact]
        public async Task GivenAccommodationWithAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenThrows422()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var addressEntity = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Тестовая улица",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 90,
            });
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FifthHotel",
                AddressId = addressEntity.Id
            });
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Fifth street";

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/accommodation/{accommodation.Id}/address", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal($"Address for accommodation with id {accommodation.Id} already exists", errorResponse?.Message);
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
