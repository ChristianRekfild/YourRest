using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.WebApi.Tests.Fixtures;
using YourRest.WebApi.Responses;

namespace YourRest.WebApi.Tests.Controllers
{
    public class AccommodationControllerTest : ApiTest
    {
        public AccommodationControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GivenValidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturnsOk()
        {     
            var accommodation = new Accommodation
            {
                Name = "Test",
            };

            var accommodationId = await InsertObjectIntoDatabase(accommodation);

            var city = new City { Name = "Moscow"};
            var cityId = await InsertObjectIntoDatabase(city);
            var addressDto = CreateValidAddressDto(cityId);

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);
            var errorResponseString = await response.Content.ReadAsStringAsync();            
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
            Assert.Equal($"Accommodation with id {accommodationId} not found", errorResponse.Message);

            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //var responseString = await response.Content.ReadAsStringAsync();           
            //var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);

            //Assert.True(createdAddress.Id > 0);
        }

       [Fact]
       public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsync_InvokedThenReturnsNotFound()
       {
           var accommodationId = -1;
           var city = new City { Name = "Moscow"};
           var cityId = await InsertObjectIntoDatabase(city);

           var addressDto = CreateValidAddressDto(cityId);

           var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
           var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);          
           var errorResponseString = await response.Content.ReadAsStringAsync();            
           var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

           Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
           Assert.Equal($"Accommodation with id {accommodationId} not found", errorResponse.Message);
       }

       [Fact]
       public async Task GivenNonexistentCityWhen_AddAddressToAccommodationAsyncInvoked_ThenReturnsNotFound()
       {
            var accommodation = new Accommodation
            {
                Name = "Test",
            };

            var accommodationId = await InsertObjectIntoDatabase(accommodation);

            var addressDto = new AddressDto
            {
                Street = "Test Street",
                ZipCode = "12345",
                Longitude = 0,
                Latitude = 0,
                Type = "Fact",
                CityId = -1
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal("City with id -1 not found", errorResponse.Message);
        }

       [Fact]
       public async Task GivenInvalidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturnsBadRequest()
       {
           var accommodation = new Accommodation
           {
                Name = "Test2",
           };

           var id = await InsertObjectIntoDatabase(accommodation);
           var addressDto = new AddressDto
           {
               Street = "",
               ZipCode = "",
               Longitude = 0,
               Latitude = 0,
               Type = "Fact",
               CityId = 1
           };

           var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
           var response = await Client.PostAsync($"api/operator/accommodation/{id}/address", content);

           //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

           var errorResponseString = await response.Content.ReadAsStringAsync();
           var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

           Assert.Equal("sergy", errorResponse.Message);
        }

        private AddressDto CreateValidAddressDto(int cityId)
        {
            return new AddressDto
            {
                Street = "Test Street",
                ZipCode = "12345",
                Longitude = 0,
                Latitude = 0,
                Type = "Fact",
                CityId = cityId
            };
        }

        private async void AssertBadRequestResponse(HttpResponseMessage response, string expectedErrorMessage)
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal(expectedErrorMessage, errorResponse.Message);
        }
    }
}
