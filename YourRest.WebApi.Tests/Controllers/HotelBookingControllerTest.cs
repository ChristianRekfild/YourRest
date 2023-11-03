using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class HotelBookingControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public HotelBookingControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task AddHotelBooking_ReturnsStatusCodeCreated()
        {
            var hotelBooking = new HotelBookingDto {
                HotelId = 1, 
                DateFrom = new DateTime(2023, 10, 5),  
                DateTo = new DateTime(2023, 10, 15),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2 
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/AgeRange", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<AgeRangeDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(hotelBooking.HotelId, hotelBookingResponse.HotelId);
            Assert.Equal(hotelBooking.DateTo, hotelBookingResponse.DateTo);
            Assert.Equal(hotelBooking.DateFrom, hotelBookingResponse.DateFrom);
            Assert.Equal(hotelBooking.RoomId, hotelBookingResponse.RoomId);
            Assert.Equal(hotelBooking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(hotelBooking.AdultNr, hotelBookingResponse.AdultNr);
            Assert.Equal(hotelBooking.ChildrenNr, hotelBookingResponse.ChildrenNr);
        }
    }
}
