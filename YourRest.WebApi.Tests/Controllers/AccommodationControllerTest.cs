using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Domain.Entities;
using YourRest.Application.Dto;
using YourRest.WebApi.Tests.Fixtures;
using YourRest.WebApi.Responses;
using Xunit;
using Xunit.Abstractions;

namespace YourRest.WebApi.Tests.Controllers
{
    public class AccommodationControllerTest : ApiTest
    {
        private readonly ITestOutputHelper _output;
        public AccommodationControllerTest(ApiFixture fixture, ITestOutputHelper output) : base(fixture)
        {
             _output = output;
        }

        [Fact]
        public async Task GivenValidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturnsOk()
        {
            var accommodationId = await AddAccommodationToDatabase();
            var addressDto = CreateValidAddressDto();

            var response = await PostAddressToAccommodation(accommodationId, addressDto);

            AssertOkResponse(response);
        }

       [Fact]
       public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsync_InvokedThenReturnsNotFound()
       {
           var accommodationId = -1;
           var addressDto = CreateValidAddressDto();

           var response = await PostAddressToAccommodation(accommodationId, addressDto);

           AssertNotFoundResponse(response, $"Accommodation with id {accommodationId} not found");
       }

       [Fact]
       public async Task Given_Nonexistent_City_When_AddAddressToAccommodationAsync_Invoked_Then_Returns_NotFound()
       {
       var accommodationId = await AddAccommodationToDatabase();
       var addressDto = CreateAddressDtoWithInvalidCity();

       var response = await PostAddressToAccommodation(accommodationId, addressDto);

       AssertNotFoundResponse(response, "City not found");
       }

       [Fact]
       public async Task GivenInvalidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturnsBadRequest()
       {
           var accommodationId = await AddAccommodationToDatabase();
           var addressDto = new AddressDto
           {
               Street = "",
               ZipCode = "",
               Longitude = 0,
               Latitude = 0,
               Type = "Home",
               CityId = 1
           };

           var response = await PostAddressToAccommodation(accommodationId, addressDto);

           AssertBadRequestResponse(response);
       }

        private async Task<int> AddAccommodationToDatabase()
        {
            var accommodation = new Accommodation
            {
                Name = "Test",
            };

            return await InsertObjectIntoDatabase(accommodation);
        }

        private AddressDto CreateValidAddressDto()
        {
            return new AddressDto
            {
                Street = "Test Street",
                ZipCode = "12345",
                Longitude = 0,
                Latitude = 0,
                Type = "Fact",
                CityId = 1
            };
        }

        private AddressDto CreateAddressDtoWithInvalidCity()
        {
            return new AddressDto
            {
                Street = "Test Street",
                ZipCode = "12345",
                Longitude = 0,
                Latitude = 0,
                Type = "Fact",
                CityId = -1
            };
        }

        private async Task<HttpResponseMessage> PostAddressToAccommodation(int accommodationId, AddressDto addressDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            return await Client.PostAsync($"api/operator/accommodation/{accommodationId}/address", content);
        }

        private async void AssertOkResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();           
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);

            Assert.True(createdAddress.Id > 0);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async void AssertNotFoundResponse(HttpResponseMessage response, string expectedErrorMessage)
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();           
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            _output.WriteLine("---------------------------------------------" + errorResponse.Message);
            Assert.Equal(expectedErrorMessage, errorResponse.Message);
        }

        private async void AssertBadRequestResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();           
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("City not found", errorResponse.Message);
        }
    }
}
