using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
using YourRest.WebApi.Tests.Fixtures;


namespace YourRest.WebApi.Tests.Controllers
{
    public class AccommodationControllerTest : ApiTest
    {
        public AccommodationControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GivenAccommodation_WhenApiMethodInvokedWithValidAddress_ThenReturns200Ok()
        {
            var accommodationEntity = new Accommodation
            {
                Name = "Test",
            };

            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var country = await InsertObjectIntoDatabase(new Country() { Name = "TestCountry" });

            var region = await InsertObjectIntoDatabase(new Region() { Name = "TestRegion", CountryId = country.Id });

            var cityEntity = new City { Name = "Moscow", RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);
            var addressDto = CreateValidAddressDto(city.Id);

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);

            Assert.True(createdAddress.Id > 0);
        }

        [Fact]
        public async Task GivenAccommodationAndExistAddressInDB_WhenApiMethodInvokedWithTheSameAddress_ThenReturns200Ok()
        {
            var country = await InsertObjectIntoDatabase(new Country() { Name = "TestCountry" });

            var region = await InsertObjectIntoDatabase(new Region() { Name = "TestRegion", CountryId = country.Id });

            var cityEntity = new City { Name = "Moscow", RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);
            var addressDto = CreateValidAddressDto(city.Id);

            var addressEntity = new Address
            {
                Street = "Тестовая улица",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 100,
            };
            await InsertObjectIntoDatabase(addressEntity);

            var accommodationEntity = new Accommodation
            {
                Name = "Test",
            };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);

            Assert.True(createdAddress.Id > 0);
        }

        [Fact]
        public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var accommodationId = -1;
            var country = await InsertObjectIntoDatabase(new Country() { Name = "TestCountry" });

            var region = await InsertObjectIntoDatabase(new Region() { Name = "TestRegion", CountryId = country.Id });

            var cityEntity = new City { Name = "Moscow", RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);

            var addressDto = CreateValidAddressDto(city.Id);

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);
            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal($"Accommodation with id {accommodationId} not found", errorResponse.Message);
        }

        [Fact]
        public async Task GivenNonexistentCity_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns400()
        {
            var accommodationEntity = new Accommodation
            {
                Name = "Test",
            };

            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var addressDto = new AddressDto
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal("City with id 100 not found", errorResponse.Message);
        }

        [Fact]
        public async Task GivenInvalidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var accommodation = new Accommodation
            {
                Name = "Test2",
            };

            var entity = await InsertObjectIntoDatabase(accommodation);
            var id = entity.Id;
            var addressDto = new AddressDto
            {
                Street = "",
                ZipCode = "",
                Longitude = 200,
                Latitude = -200,
                CityId = -1
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{id}/address", content);
            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorData = JsonConvert.DeserializeObject<ErrorData>(errorResponseString);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The Street field is required.", errorData.Errors["Street"][0]);
            Assert.Equal("The ZipCode field is required.", errorData.Errors["ZipCode"][0]);
            Assert.Equal("The field Longitude must be between -180 and 180.", errorData.Errors["Longitude"][0]);
            Assert.Equal("The field Latitude must be between -90 and 90.", errorData.Errors["Latitude"][0]);
            Assert.Equal("City Id should be more than zero.", errorData.Errors["CityId"][0]);
        }

        [Fact]
        public async Task GivenAccommodationWithAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenThrows422()
        {
            var country = await InsertObjectIntoDatabase(new Country() { Name = "TestCountry" });

            var region = await InsertObjectIntoDatabase(new Region() { Name = "TestRegion", CountryId = country.Id });

            var cityEntity = new City { Name = "Moscow", RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);
            var addressDto = CreateValidAddressDto(city.Id);

            var addressEntity = new Address
            {
                Street = "Тестовая улица",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 100,
            };
            await InsertObjectIntoDatabase(addressEntity);

            var accommodationEntity = new Accommodation
            {
                Name = "Test",
                AddressId = addressEntity.Id
            };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal($"Address for accommodation with id {accommodationId} already exists", errorResponse.Message);
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
