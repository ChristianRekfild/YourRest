using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Unicode;
using System.Xml.Serialization;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
using YourRest.WebApi.Tests.Fixtures;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


            var hotelBooking = new BookingDto
            {
                StartDate = new DateTime(2025, 10, 5),
                EndDate = new DateTime(2025, 10, 15),
                Rooms = new List<RoomWithIdDto> { new RoomWithIdDto() { Id = 1 } },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(hotelBooking.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(hotelBooking.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(hotelBooking.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(hotelBooking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(hotelBooking.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(hotelBooking.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
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
            var hotelBookingEntity = new Booking
            {               
                StartDate = new DateTime(2025, 10, 2),
                EndDate = new DateTime(2025, 10, 12),
                Rooms = new List<Room> { new Room() { Id = 1 } },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            await fixture.InsertObjectIntoDatabase(hotelBookingEntity);
            await fixture.InsertObjectIntoDatabase(new Room { Name = "310", AccommodationId = accommodationId, Capacity = 1, SquareInMeter = 20, RoomType = "Luxe" });

            var hotelBooking = new BookingDto
            {
                StartDate = new DateTime(2025, 10, 5),
                EndDate = new DateTime(2025, 10, 15),
                Rooms = new List<RoomWithIdDto> { new RoomWithIdDto(){ Id = 1} },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2
            };


            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/hotelbooking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
