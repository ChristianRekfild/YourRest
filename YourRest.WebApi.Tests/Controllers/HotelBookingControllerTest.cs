using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Xml.Serialization;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
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
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodationEntity = new Accommodation
            {
                Name = "Test",
                AccommodationType = accommodationType
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;
            var firstRoom = await fixture.InsertObjectIntoDatabase(new Room { Name = "310", AccommodationId = accommodationId, Capacity = 1, SquareInMeter = 20, RoomType = "Luxe" });


            var hotelBooking = new HotelBookingDto {
                AccommodationId = accommodationId, 
                DateFrom = new DateTime(2025, 10, 5),  
                DateTo = new DateTime(2025, 10, 15),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2 
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<HotelBookingDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(hotelBooking.AccommodationId, hotelBookingResponse.AccommodationId);
            Assert.Equal(hotelBooking.DateTo, hotelBookingResponse.DateTo);
            Assert.Equal(hotelBooking.DateFrom, hotelBookingResponse.DateFrom);
            Assert.Equal(hotelBooking.RoomId, hotelBookingResponse.RoomId);
            Assert.Equal(hotelBooking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(hotelBooking.AdultNr, hotelBookingResponse.AdultNr);
            Assert.Equal(hotelBooking.ChildrenNr, hotelBookingResponse.ChildrenNr);
        }

        [Fact]
        public async Task AddHotelBooking_WhenDataoccupied_ReturnsStatusCodeError()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodationEntity = new Accommodation
            {
                Name = "Test",
                AccommodationType = accommodationType
            };
            var hotelBookingEntity = new HotelBooking
            {
                AccommodationId = 1,
                DateFrom = new DateTime(2025, 10, 2),
                DateTo = new DateTime(2025, 10, 12),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            await fixture.InsertObjectIntoDatabase(hotelBookingEntity);
            await fixture.InsertObjectIntoDatabase(new Room { Name = "310", AccommodationId = accommodationId, Capacity = 1, SquareInMeter = 20, RoomType = "Luxe" });

            var hotelBooking = new HotelBookingDto
            {
                AccommodationId = accommodationId,
                DateFrom = new DateTime(2025, 10, 5),
                DateTo = new DateTime(2025, 10, 15),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var hotelBooking2 = new HotelBookingDto
            {
                AccommodationId = accommodationId,
                DateFrom = new DateTime(2025, 9, 20),
                DateTo = new DateTime(2025, 10, 15),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };


            content = new StringContent(JsonConvert.SerializeObject(hotelBooking2), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var hotelBooking3 = new HotelBookingDto
            {
                AccommodationId = accommodationId,
                DateFrom = new DateTime(2025, 10, 5),
                DateTo = new DateTime(2025, 10, 10),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };


            content = new StringContent(JsonConvert.SerializeObject(hotelBooking3), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var hotelBooking4 = new HotelBookingDto
            {
                AccommodationId = accommodationId,
                DateFrom = new DateTime(2025, 9, 1),
                DateTo = new DateTime(2025, 10, 10),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2
            };


            content = new StringContent(JsonConvert.SerializeObject(hotelBooking4), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            //var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            //var response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            //var errorData = await response.Content.ReadAsStringAsync();

            //Assert.Equal("Бронирование на выбранные даты невозможно. Комната занята.", errorData);
        }
    }
}
