using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.WebApi.Tests.Fixtures;
using YourRest.WebApi.Responses;
using YourRest.Application.Dto.ViewModels;
using System.Net.Http.Json;
//using Docker.DotNet.Models;

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

            var countryEntity = new Country() { Name = "Russia" };
            var country = await InsertObjectIntoDatabase(countryEntity);

            var regionEntity = new Region() { Name = "Московская область", CountryId = country.Id };
            var region = await InsertObjectIntoDatabase(regionEntity);

            var cityEntity = new City { Name = "Moscow", RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);
            var addressDto = CreateValidAddressDto(city.Id);

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);

            Assert.True(createdAddress?.Id > 0);
        }

        [Fact]
        public async Task GivenAccommodationAndExistAddressInDB_WhenApiMethodInvokedWithTheSameAddress_ThenReturns200Ok()
        {
            var countryEntity = new Country { Name = "Russia" };
            var country = await InsertObjectIntoDatabase(countryEntity);
            var regionEntity = new Region { Name = "Московская область", CountryId = country.Id };
            var region = await InsertObjectIntoDatabase(regionEntity);

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

            Assert.True(createdAddress?.Id > 0);
        }

        [Fact]
        public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var countryEntity = new Country { Name = "Russia" };
            var country = await InsertObjectIntoDatabase(countryEntity);
            var regionEntity = new Region { Name = "Московская область", CountryId = country.Id };
            var region = await InsertObjectIntoDatabase(regionEntity);

            var accommodationId = -1;
            var cityEntity = new City { Name = "Moscow" , RegionId = region.Id };
            var city = await InsertObjectIntoDatabase(cityEntity);

            var addressDto = CreateValidAddressDto(city.Id);

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);
            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {accommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorMassage, expectedMessageJson);
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
            var expectedMessage = new { message = "City with id 100 not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorResponseString, expectedMessageJson);
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
                Longitude = 190,
                Latitude = 190,
                CityId = -1
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{id}/address", content);
            var errorData = await response.Content.ReadFromJsonAsync<ErrorViewModel>();
           

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("'Street' должно быть заполнено.", errorData?.ValidationErrors[nameof(addressDto.Street)][0]);
            Assert.Equal("'Zip Code' должно быть заполнено.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][0]);
            Assert.Equal("'Zip Code' имеет неверный формат.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][1]);
            Assert.Equal("'Latitude' должно быть в диапазоне от -90 до 90. Введенное значение: 190.", errorData?.ValidationErrors[nameof(addressDto.Latitude)][0]);
            Assert.Equal("'Longitude' должно быть в диапазоне от -180 до 180. Введенное значение: 190.", errorData?.ValidationErrors[nameof(addressDto.Longitude)][0]);
            Assert.Equal("'City Id' должно быть больше '0'.", errorData?.ValidationErrors[nameof(addressDto.CityId)][0]);
        }

        [Fact]
        public async Task GivenAccommodationWithAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenThrows422()
        {
            var countryEntity = new Country { Name = "Russia" };
            var country = await InsertObjectIntoDatabase(countryEntity);
            var regionEntity = new Region { Name = "Московская область", CountryId = country.Id };
            var region = await InsertObjectIntoDatabase(regionEntity);

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

            Assert.Equal($"Address for accommodation with id {accommodationId} already exists", errorResponse?.Message);
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
