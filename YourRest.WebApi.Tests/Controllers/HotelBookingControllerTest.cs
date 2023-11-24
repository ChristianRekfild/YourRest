using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Unicode;
using System.Xml.Linq;
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

            Room firstRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };

            var firstRoom = await fixture.InsertObjectIntoDatabase(firstRoomToInsert);

            BookingDto booking = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<int>() { firstRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test",
                LastName = "Test",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(booking.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(booking.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(booking.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(booking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(booking.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(booking.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
            Assert.Equal(booking.FirstName, hotelBookingResponse.FirstName);
            Assert.Equal(booking.MiddleName, hotelBookingResponse.MiddleName);
            Assert.Equal(booking.LastName, hotelBookingResponse.LastName);
            Assert.Equal(booking.DateOfBirth, hotelBookingResponse.DateOfBirth);
        }

        [Fact]
        public async Task AddHotelBooking_WhenDataoccupiedEndDateIn_ReturnsStatusCodeError()
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

            Room firstRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            Room secondRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "SAA",
                RoomType = "ss",
                SquareInMeter = 2,
                Capacity = 20
            };

            Customer customerToInsert = new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            var firstRoom = await fixture.InsertObjectIntoDatabase(firstRoomToInsert);
            var secondRoom = await fixture.InsertObjectIntoDatabase(secondRoomToInsert);
            var testCustomer = await fixture.InsertObjectIntoDatabase(customerToInsert);

            Booking bookingToInsert = new Booking()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<Room>() { firstRoom },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };

            await fixture.InsertObjectIntoDatabase(bookingToInsert);

            BookingDto bookingDateToIn = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 2),
                EndDate = new DateOnly(2025, 10, 12),
                Rooms = new List<int>() { firstRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            BookingDto bookingDateToInWithSecondRoomId = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 2),
                EndDate = new DateOnly(2025, 10, 12),
                Rooms = new List<int>() { secondRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            var content = new StringContent(JsonConvert.SerializeObject(bookingDateToIn), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingDateToInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);


            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(bookingDateToInWithSecondRoomId.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(bookingDateToInWithSecondRoomId.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(bookingDateToInWithSecondRoomId.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(bookingDateToInWithSecondRoomId.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(bookingDateToInWithSecondRoomId.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(bookingDateToInWithSecondRoomId.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
            Assert.Equal(bookingDateToInWithSecondRoomId.FirstName, hotelBookingResponse.FirstName);
            Assert.Equal(bookingDateToInWithSecondRoomId.MiddleName, hotelBookingResponse.MiddleName);
            Assert.Equal(bookingDateToInWithSecondRoomId.LastName, hotelBookingResponse.LastName);
            Assert.Equal(bookingDateToInWithSecondRoomId.DateOfBirth, hotelBookingResponse.DateOfBirth);
        }

        [Fact]
        public async Task AddHotelBooking_WhenDataoccupiedStartDateIn_ReturnsStatusCodeError()
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

            Room firstRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            Room secondRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "SAA",
                RoomType = "ss",
                SquareInMeter = 2,
                Capacity = 20
            };

            Customer customerToInsert = new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            var firstRoom = await fixture.InsertObjectIntoDatabase(firstRoomToInsert);
            var secondRoom = await fixture.InsertObjectIntoDatabase(secondRoomToInsert);
            var testCustomer = await fixture.InsertObjectIntoDatabase(customerToInsert);

            Booking bookingToInsert = new Booking()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<Room>() { firstRoom },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };

            await fixture.InsertObjectIntoDatabase(bookingToInsert);


            BookingDto bookingDateFromIn = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 7),
                EndDate = new DateOnly(2025, 10, 17),
                Rooms = new List<int>() { firstRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };
            BookingDto bookingDateFromInWithSecondRoomId = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 7),
                EndDate = new DateOnly(2025, 10, 17),
                Rooms = new List<int>() { secondRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };


            var content = new StringContent(JsonConvert.SerializeObject(bookingDateFromIn), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingDateFromInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);


            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(bookingDateFromInWithSecondRoomId.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(bookingDateFromInWithSecondRoomId.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(bookingDateFromInWithSecondRoomId.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(bookingDateFromInWithSecondRoomId.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(bookingDateFromInWithSecondRoomId.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(bookingDateFromInWithSecondRoomId.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
            Assert.Equal(bookingDateFromInWithSecondRoomId.FirstName, hotelBookingResponse.FirstName);
            Assert.Equal(bookingDateFromInWithSecondRoomId.MiddleName, hotelBookingResponse.MiddleName);
            Assert.Equal(bookingDateFromInWithSecondRoomId.LastName, hotelBookingResponse.LastName);
            Assert.Equal(bookingDateFromInWithSecondRoomId.DateOfBirth, hotelBookingResponse.DateOfBirth);
        }

        [Fact]
        public async Task AddHotelBooking_WhenDataoccupiedStartAllIn_ReturnsStatusCodeError()
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

            Room firstRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            Room secondRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "SAA",
                RoomType = "ss",
                SquareInMeter = 2,
                Capacity = 20
            };

            Customer customerToInsert = new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            var firstRoom = await fixture.InsertObjectIntoDatabase(firstRoomToInsert);
            var secondRoom = await fixture.InsertObjectIntoDatabase(secondRoomToInsert);
            var testCustomer = await fixture.InsertObjectIntoDatabase(customerToInsert);

            Booking bookingToInsert = new Booking()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<Room>() { firstRoom },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };

            await fixture.InsertObjectIntoDatabase(bookingToInsert);


            BookingDto bookingAllIn = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 1),
                EndDate = new DateOnly(2025, 10, 20),
                Rooms = new List<int>() { firstRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            BookingDto bookingAllInWithSecondRoomId = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 1),
                EndDate = new DateOnly(2025, 10, 20),
                Rooms = new List<int>() { secondRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };


            var content = new StringContent(JsonConvert.SerializeObject(bookingAllIn), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingAllInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);


            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(bookingAllInWithSecondRoomId.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(bookingAllInWithSecondRoomId.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(bookingAllInWithSecondRoomId.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(bookingAllInWithSecondRoomId.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(bookingAllInWithSecondRoomId.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(bookingAllInWithSecondRoomId.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
            Assert.Equal(bookingAllInWithSecondRoomId.FirstName, hotelBookingResponse.FirstName);
            Assert.Equal(bookingAllInWithSecondRoomId.MiddleName, hotelBookingResponse.MiddleName);
            Assert.Equal(bookingAllInWithSecondRoomId.LastName, hotelBookingResponse.LastName);
            Assert.Equal(bookingAllInWithSecondRoomId.DateOfBirth, hotelBookingResponse.DateOfBirth);
        }

        [Fact]
        public async Task AddHotelBooking_WhenDataoccupiedStartEqual_ReturnsStatusCodeError()
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

            Room firstRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            Room secondRoomToInsert = new Room()
            {
                AccommodationId = accommodation.Id,
                Name = "SAA",
                RoomType = "ss",
                SquareInMeter = 2,
                Capacity = 20
            };

            Customer customerToInsert = new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            var firstRoom = await fixture.InsertObjectIntoDatabase(firstRoomToInsert);
            var secondRoom = await fixture.InsertObjectIntoDatabase(secondRoomToInsert);
            var testCustomer = await fixture.InsertObjectIntoDatabase(customerToInsert);

            Booking bookingToInsert = new Booking()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<Room>() { firstRoom },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };

            await fixture.InsertObjectIntoDatabase(bookingToInsert);

            BookingDto bookingEqual = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<int>() { firstRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            BookingDto bookingEqualWithSecondRoomId = new BookingDto()
            {
                StartDate = new DateOnly(2025, 10, 5),
                EndDate = new DateOnly(2025, 10, 15),
                Rooms = new List<int>() { secondRoom.Id },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            };

            var content = new StringContent(JsonConvert.SerializeObject(bookingEqual), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingEqualWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/booking/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<BookingDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(bookingEqualWithSecondRoomId.StartDate, hotelBookingResponse.StartDate);
            Assert.Equal(bookingEqualWithSecondRoomId.EndDate, hotelBookingResponse.EndDate);
            Assert.Equal(bookingEqualWithSecondRoomId.Rooms, hotelBookingResponse.Rooms);
            Assert.Equal(bookingEqualWithSecondRoomId.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(bookingEqualWithSecondRoomId.AdultNumber, hotelBookingResponse.AdultNumber);
            Assert.Equal(bookingEqualWithSecondRoomId.ChildrenNumber, hotelBookingResponse.ChildrenNumber);
            Assert.Equal(bookingEqualWithSecondRoomId.FirstName, hotelBookingResponse.FirstName);
            Assert.Equal(bookingEqualWithSecondRoomId.MiddleName, hotelBookingResponse.MiddleName);
            Assert.Equal(bookingEqualWithSecondRoomId.LastName, hotelBookingResponse.LastName);
            Assert.Equal(bookingEqualWithSecondRoomId.DateOfBirth, hotelBookingResponse.DateOfBirth);
        }

        [Fact]
        public async Task GetOccupiedDatesByRoomId()
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

            Customer customerToInsert = new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5),
            };

            var testCustomer = await fixture.InsertObjectIntoDatabase(customerToInsert);

            Room roomToInsert = new Room()
            {
                Accommodation = accommodation,
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            var room = await fixture.InsertObjectIntoDatabase(roomToInsert);

            Booking bookingInFutureToInsert = new Booking()
            {
                StartDate = new DateOnly(2030, 10, 5),
                EndDate = new DateOnly(2030, 10, 15),
                Rooms = new List<Room>() { room },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };
            await fixture.InsertObjectIntoDatabase(bookingInFutureToInsert);

            Booking bookingCurrentInsert = new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = new DateOnly(2027, 10, 15),
                Rooms = new List<Room>() { room },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };
            await fixture.InsertObjectIntoDatabase(bookingCurrentInsert);

            Booking bookingPastToInsert = new Booking()
            {
                StartDate = new DateOnly(2020, 10, 5),
                EndDate = new DateOnly(2020, 10, 15),
                Rooms = new List<Room>() { room },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            };
            await fixture.InsertObjectIntoDatabase(bookingPastToInsert);

            List<RoomOccupiedDateDto> occupiedDateResult = new List<RoomOccupiedDateDto>()
            {
                new RoomOccupiedDateDto() {
                    StartDate = bookingCurrentInsert.StartDate,
                    EndDate = bookingCurrentInsert.EndDate
                    },
                new RoomOccupiedDateDto() {
                    StartDate = bookingInFutureToInsert.StartDate,
                    EndDate= bookingInFutureToInsert.EndDate
                    }
            };

            var response = await fixture.Client.GetAsync($"api/booking/rooms/{room.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var occupiedDateResponse = JsonConvert.DeserializeObject<List<RoomOccupiedDateDto>>(responseContent);

            Assert.NotNull( occupiedDateResponse );
            Assert.Equal(occupiedDateResult.First().StartDate, occupiedDateResponse.Last().StartDate);
            Assert.Equal(occupiedDateResult.Last().StartDate, occupiedDateResponse.First().StartDate);
            Assert.Equal(occupiedDateResult.First().EndDate, occupiedDateResponse.Last().EndDate);
            Assert.Equal(occupiedDateResult.Last().EndDate, occupiedDateResponse.First().EndDate);
        }
    }
}
