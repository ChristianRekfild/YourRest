using AutoMapper;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Amazon.Runtime.Internal;
using YourRest.Application.Dto.Mappers.Profiles;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class RoomControllerTest
    {
        private readonly SingletonApiTest fixture;

        public RoomControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task UpdatedRoomTest_WhenPutCalledEditMethod_ReturnsMessageOfSuccsessfulyEdited()
        {
            var roomType = new RoomType { Name = "Test Type" };
            await fixture.InsertObjectIntoDatabase(roomType);
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
            var editedRoom = new Room
            {
                AccommodationId = room.AccommodationId,
                Name = "305",
                RoomType = roomType,
                SquareInMeter = 5,
                Capacity = 2
            };
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RoomDtoProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var content = new StringContent(JsonConvert.SerializeObject(mapper.Map<RoomDto>(editedRoom)), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/accommodations/{room.AccommodationId}/rooms/{room.Id}", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The room has been edited", await response.Content.ReadAsStringAsync());
            response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomWithIdDto>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoom);
            Assert.Equal(editedRoom.Name, recivedRoom.Name);
        }
        [Fact]
        public async Task DeleteRoomTest_WhenDeletCalledRemoveMethod_ReturnsMessageOfSuccsessfulyRemoved()
        {
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());

            var response = await fixture.Client.DeleteAsync($"api/rooms/{room.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The room has been removed", await response.Content.ReadAsStringAsync());
            response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
        [Fact]
        public async Task GetRoomByIdTest_WhenGetCalledGetByIdMethod_ReturnsRoomViewModel()
        {
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
            var response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomExtendedDto>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoom);
            Assert.Equal(room.Id, recivedRoom.Id);
            Assert.Equal(room.Name, recivedRoom.Name);
        }
        [Fact]
        public async Task GetFacilitiesByRoomIdTest_WhenGetCalledGetFacilitiesByRoomIdMethod_ReturnsIEnumerableOfRoomFacilitiyViewModels()
        {
            var room = await CreateRoomAsync();
            room.RoomFacilities = new List<RoomFacility>
            {
                new RoomFacility(){ Name = "Air Conditioner" },
                new RoomFacility(){ Name = "Minibar" },
                new RoomFacility(){ Name = "Locker" }
            };
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RoomFacilityDtoProfile());
            });
            var mapper = mockMapper.CreateMapper();
            room = await fixture.InsertObjectIntoDatabase(room);
            var response = await fixture.Client.GetAsync($"api/rooms/{room.Id}/facilities");
            var recivedRoomFacilities = await response.Content.ReadFromJsonAsync<IEnumerable<RoomFacilityDto>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoomFacilities);
            Assert.NotEmpty(recivedRoomFacilities);
            Assert.Equal(3, recivedRoomFacilities.Count());
            Assert.Equivalent(mapper.Map<IEnumerable<RoomFacilityDto>>(room.RoomFacilities), recivedRoomFacilities);

        }
        private async Task<Room> CreateRoomAsync()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "MO", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var address = await fixture.InsertObjectIntoDatabase(new Address
            {
                CityId = city.Id,
                Street = "Prospect Mira",
                ZipCode = "123456",
                Longitude = 100,
                Latitude = 500
            });
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var roomType = new RoomType { Name = "Test Type" };

            var accommodation = await fixture.InsertObjectIntoDatabase(
                new Accommodation
                {
                    Name = "MyHotel",
                    AccommodationType = accommodationType
                }
                );
            return new Room
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = roomType,
                SquareInMeter = 1,
                Capacity = 1
            };
        }
        [Fact]
        public async Task GetAllRoom_ReturnsExpectedRoom_WhenDatabaseHasRoom()
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
            var roomType = new RoomType { Name = "Test Type" };
            await fixture.InsertObjectIntoDatabase(roomType);

            var firstRoom = await fixture.InsertObjectIntoDatabase(new Room { Name = "310", AccommodationId = accommodationId, Capacity = 1,SquareInMeter = 20, RoomType = roomType });
            var secondRoom = await fixture.InsertObjectIntoDatabase(new Room { Name = "305", AccommodationId = accommodationId, Capacity = 2, SquareInMeter = 30, RoomType = roomType });
            List<RoomWithIdDto> rooms = new()
            {
                new() {
                    Id = firstRoom.Id,
                    Name = firstRoom.Name,
                    Capacity = firstRoom.Capacity,
                    RoomTypeId = firstRoom.RoomType.Id,
                    SquareInMeter = firstRoom.SquareInMeter
                },
                new()
                {
                    Id = secondRoom.Id,
                    Name = secondRoom.Name,
                    Capacity = secondRoom.Capacity,
                    RoomTypeId = firstRoom.RoomType.Id,
                    SquareInMeter = secondRoom.SquareInMeter
                }
            };

            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodation.Id}/rooms");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var roomResponse = await response.Content.ReadFromJsonAsync<IEnumerable<RoomWithIdDto>>();
            Assert.NotNull(roomResponse);
            Assert.NotEmpty(roomResponse);
            Assert.Equivalent(roomResponse, rooms);
        }
        [Fact]
        public async Task GetAllRoom_ReturnsVoid_WhenDatabaseHasNoRoom()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodationEntity = new Accommodation { Name = "Test", AccommodationType = accommodationType};
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodation.Id}/rooms");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("[]", content);
        }
        [Fact]
        public async Task AddRoom_ReturnsStatusCodeCreated()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var roomType = new RoomType { Name = "Test Type" };
            await fixture.InsertObjectIntoDatabase(roomType);

            var accommodationEntity = new Accommodation { Name = "Test", AccommodationType = accommodationType};
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new RoomDto { Name = "Lyxar1", Capacity = 20, SquareInMeter = 30, RoomTypeId = roomType.Id };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodations/{accommodation.Id}/rooms", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var roomResponse = JsonConvert.DeserializeObject<RoomWithIdDto>(responseContent);

            Assert.Equal(roomResponse?.Name, roomEntity.Name);
            Assert.Equal(roomResponse?.RoomTypeId, roomEntity.RoomTypeId);
            Assert.Equal(roomResponse?.Capacity, roomEntity.Capacity);
            Assert.Equal(roomResponse?.SquareInMeter, roomEntity.SquareInMeter);
        }
        [Fact]
        public async Task AddRoom_ReturnsNotFound_WhenAddRoomWitFakeAccommodation()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodationEntity = new Accommodation { Name = "Test", AccommodationType = accommodationType};
            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id + 100;
            var roomType = new RoomType { Name = "Test Type" };
            await fixture.InsertObjectIntoDatabase(roomType);
            
            var roomEntity = new RoomDto { Name = "Lyxar", Capacity = 20, SquareInMeter = 30, RoomTypeId = roomType.Id };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodations/{accommodationId}/rooms", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {accommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(errorMassage, expectedMessageJson);
        }
    }
}
