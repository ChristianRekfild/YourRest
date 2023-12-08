using AutoMapper;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using System.Xml.Serialization;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Mappers.Profiles;
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
            var response = await fixture.Client.PostAsync($"api/bookings/", content);

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
            var response = await fixture.Client.PostAsync($"api/bookings/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingDateToInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/bookings/", content);

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
            var response = await fixture.Client.PostAsync($"api/bookings/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingDateFromInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/bookings/", content);

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
            var response = await fixture.Client.PostAsync($"api/bookings/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingAllInWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/bookings/", content);

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
            var response = await fixture.Client.PostAsync($"api/bookings/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            content = new StringContent(JsonConvert.SerializeObject(bookingEqualWithSecondRoomId), Encoding.UTF8, "application/json");
            response = await fixture.Client.PostAsync($"api/bookings/", content);

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

            var response = await fixture.Client.GetAsync($"api/rooms/{room.Id}/bookings/dates");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var occupiedDateResponse = JsonConvert.DeserializeObject<List<RoomOccupiedDateDto>>(responseContent);

            Assert.NotNull( occupiedDateResponse );
            Assert.Equal(occupiedDateResult.First().StartDate, occupiedDateResponse.Last().StartDate);
            Assert.Equal(occupiedDateResult.Last().StartDate, occupiedDateResponse.First().StartDate);
            Assert.Equal(occupiedDateResult.First().EndDate, occupiedDateResponse.Last().EndDate);
            Assert.Equal(occupiedDateResult.Last().EndDate, occupiedDateResponse.First().EndDate);
        }

        [Fact]
        public async Task GetRoomsByCity_WhenStartDateNewBookingEqualsEndDateBookingInDB_ReturnRoomsList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent2 = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingMskLyxPast = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2020, 10, 5),
                EndDate = new DateOnly(2020, 10, 15),
                Rooms = new List<Room>() { roomMskHern1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));

            //Make request
            var response = await fixture.Client.GetAsync($"api/cities/{cityMoscow.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            var responseRoomsIdList = responseRoomsList.Select(x => x.Id).ToList();
       
            Assert.True(responseRoomsIdList.Contains(roomMskLyx2.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskHern1.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskHern2.Id));
        }

        [Fact]
        public async Task GetRoomsByCity_WhenStartDateNewBookingLaterEndDateBookingInDB_ReturnRoomsList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent2 = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingMskLyxPast = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2020, 10, 5),
                EndDate = new DateOnly(2020, 10, 15),
                Rooms = new List<Room>() { roomMskHern1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request
            var response = await fixture.Client.GetAsync($"api/cities/{cityMoscow.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            var responseRoomsIdList = responseRoomsList.Select(x => x.Id).ToList();
       
            Assert.False(responseRoomsIdList.Contains(roomMskLyx1.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskLyx2.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskHern1.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskHern2.Id));
        }

        [Fact]
        public async Task GetRoomsByCity_WhenNoAvailableRoomsWhenBookingInDbEarly_ReturnEmptyList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1, roomMskLyx2, roomMskHern1 , roomMskHern2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request
            var response = await fixture.Client.GetAsync($"api/cities/{cityMoscow.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            Assert.Empty(responseRoomsList);
        }

        [Fact]
        public async Task GetRoomsByCity_WhenNoAvailableRoomsWhenBookingInDbLater_ReturnEmptyList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(12)),
                Rooms = new List<Room>() { roomMskLyx1, roomMskLyx2, roomMskHern1, roomMskHern2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request
            var response = await fixture.Client.GetAsync($"api/cities/{cityMoscow.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            Assert.Empty(responseRoomsList);
        }


        [Fact]
        public async Task GetRoomsByAccomodation_WhenStartDateNewBookingEqualsEndDateBookingInDB_ReturnRoomsList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent2 = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingMskLyxPast = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2020, 10, 5),
                EndDate = new DateOnly(2020, 10, 15),
                Rooms = new List<Room>() { roomMskHern1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));

            //Make request
            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodationMskLyx.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            var responseRoomsIdList = responseRoomsList.Select(x => x.Id).ToList();

            Assert.False(responseRoomsIdList.Contains(roomMskLyx1.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskLyx2.Id));
            Assert.False(responseRoomsIdList.Contains(roomMskHern1.Id));
            Assert.False(responseRoomsIdList.Contains(roomMskHern2.Id));
        }

        [Fact]
        public async Task GetRoomsByAccomodation_WhenStartDateNewBookingLaterEndDateBookingInDB_ReturnRoomsList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent2 = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingMskLyxPast = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2020, 10, 5),
                EndDate = new DateOnly(2020, 10, 15),
                Rooms = new List<Room>() { roomMskHern1 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request
            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodationMskLyx.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            var responseRoomsIdList = responseRoomsList.Select(x => x.Id).ToList();

            Assert.False(responseRoomsIdList.Contains(roomMskLyx1.Id));
            Assert.True(responseRoomsIdList.Contains(roomMskLyx2.Id));
            Assert.False(responseRoomsIdList.Contains(roomMskHern1.Id));
            Assert.False(responseRoomsIdList.Contains(roomMskHern2.Id));
        }

        [Fact]
        public async Task GetRoomsByAccomodation_WhenNoAvailableRoomsWhenBookingInDbEarly_ReturnEmptyList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = new DateOnly(2021, 10, 5),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Rooms = new List<Room>() { roomMskLyx1, roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request                                 
            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodationMskLyx.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            Assert.Empty(responseRoomsList);
        }

        [Fact]
        public async Task GetRoomsByAccomodation_WhenNoAvailableRoomsWhenBookingInDbLater_ReturnEmptyList()
        {

            //Add country
            var countryRus = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var countryTest = await fixture.InsertObjectIntoDatabase(new Country { Name = "countryTest" });

            //Add region
            var regionMoscow = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = countryRus.Id });
            var regionTest = await fixture.InsertObjectIntoDatabase(new Region { Name = "Test region", CountryId = countryTest.Id });

            //Add city
            var cityMoscow = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = regionMoscow.Id });
            var cityTest = await fixture.InsertObjectIntoDatabase(new City { Name = "Test", RegionId = regionTest.Id });

            //add adress
            var addressMoscowEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street1",
                CityId = cityMoscow.Id,
                ZipCode = "11111",
                Longitude = 121,
                Latitude = 77,
            });

            var addressMoscowEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Moscow Street2",
                CityId = cityMoscow.Id,
                ZipCode = "22222",
                Longitude = 122,
                Latitude = 72,
            });

            var addressTestEntity1 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street1",
                CityId = cityTest.Id,
                ZipCode = "33333",
                Longitude = 123,
                Latitude = 73,
            });

            var addressTestEntity2 = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street2",
                CityId = cityTest.Id,
                ZipCode = "44444",
                Longitude = 124,
                Latitude = 74,
            });

            //AccommodationType
            var accommodationTypeLuxury = new AccommodationType
            {
                Name = "Luxury"
            };
            var accommodationTypeHernia = new AccommodationType
            {
                Name = "Hernia"
            };

            //Add Accommodation
            Accommodation accommodationMskLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelMoscow",
                AddressId = addressMoscowEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationMskHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelMoscow",
                AddressId = addressMoscowEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });
            Accommodation accommodationTestLyx = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotelTest",
                AddressId = addressTestEntity1.Id,
                AccommodationType = accommodationTypeLuxury
            });
            Accommodation accommodationTestHern = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "heroHotelTest",
                AddressId = addressTestEntity2.Id,
                AccommodationType = accommodationTypeHernia
            });

            //Add Customer
            var testCustomer = await fixture.InsertObjectIntoDatabase(new Customer()
            {
                FirstName = "Test",
                MiddleName = "Test1",
                LastName = "Test2",
                DateOfBirth = new DateTime(1950, 11, 5)
            });

            //Add rooms
            Room roomMskLyx1 = await AddRoom(accommodationMskLyx, 1);
            Room roomMskLyx2 = await AddRoom(accommodationMskLyx, 2);
            Room roomMskHern1 = await AddRoom(accommodationMskHern, 3);
            Room roomMskHern2 = await AddRoom(accommodationMskHern, 4);

            Room roomTestLyx1 = await AddRoom(accommodationTestLyx, 5);
            Room roomTestLyx2 = await AddRoom(accommodationTestLyx, 6);
            Room roomTestHern3 = await AddRoom(accommodationTestHern, 7);
            Room roomTestHern4 = await AddRoom(accommodationTestHern, 8);

            //Add bookings
            Booking bookingInMskLyxFuture = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(50)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(60)),
                Rooms = new List<Room>() { roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            Booking bookingInMskLyxCurrent = await fixture.InsertObjectIntoDatabase(new Booking()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(12)),
                Rooms = new List<Room>() { roomMskLyx1, roomMskLyx2 },
                TotalAmount = 5000.0m,
                AdultNumber = 2,
                ChildrenNumber = 2,
                CustomerId = testCustomer.Id
            });

            //New booking dates
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10));

            //Make request
            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodationMskLyx.Id}/rooms/{startDate}/{endDate}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseRoomsList = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(responseContent);

            Assert.NotNull(responseRoomsList);
            Assert.Empty(responseRoomsList);
        }
        private async Task<Room> AddRoom(Accommodation accommodation, int DeluxeRoomId)
        {
            Room roomToInsert = new Room()
            {
                Accommodation = accommodation,
                AccommodationId = accommodation.Id,
                Name = $"DeluxeRoom{DeluxeRoomId}",
                RoomType = "ZBS",
                SquareInMeter = 1,
                Capacity = 20
            };
            Room resultRoom = await fixture.InsertObjectIntoDatabase(roomToInsert);
            return resultRoom;
        }
    }
}
