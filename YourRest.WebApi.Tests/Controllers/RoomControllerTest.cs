//using Newtonsoft.Json;
//using System.Collections.Generic;
//using System.Net;
//using System.Security.AccessControl;
//using System.Text;
//using System.Text.Json;
//using System.Xml.Linq;
//using YourRest.Application.Dto;
//using YourRest.Domain.Entities;
//using YourRest.WebApi.Responses;
//using System.Net.Http.Json;
//using YourRest.WebApi.Tests.Fixtures;
//using YourRest.Application.Dto.Mappers;
//using YourRest.Application.Dto.Models;
//
//namespace YourRest.WebApi.Tests.Controllers
//{
//    public class RoomControllerTest : IClassFixture<SingletonApiTest>
//    {
//        private readonly SingletonApiTest fixture;
//        public RoomControllerTest(SingletonApiTest fixture)
//        {
//            this.fixture = fixture;
//        }
//
//        [Fact]
//        public async Task GetAllRoom_ReturnsExpectedRoom_WhenDatabaseHasRoom()
//        {
//            var accommodationEntity = new Accommodation{Name = "Test"};
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, Id = 20, SquareInMeter = 30, RoomType = "Lyx" };
//            var expectedRoom = await fixture.InsertObjectIntoDatabase(roomEntity);
//
//            var response = await fixture.Client.GetAsync($"api/rooms/{accommodationId}");
//
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            var content = await response.Content.ReadAsStringAsync();
//
//            var roomResponse = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(content);
//
//            Assert.Equal(roomResponse?.First().Id, expectedRoom.Id);
//            Assert.Equal(roomResponse?.First().Name, expectedRoom.Name);
//            Assert.Equal(roomResponse?.First().AccommodationId, expectedRoom.AccommodationId);
//            Assert.Equal(roomResponse?.First().RoomType, expectedRoom.RoomType);
//            Assert.Equal(roomResponse?.First().Capacity, expectedRoom.Capacity);
//            Assert.Equal(roomResponse?.First().SquareInMeter, expectedRoom.SquareInMeter);
//        }
//
//
//        [Fact]
//        public async Task GetAllRoom_ReturnsVoid_WhenDatabaseHasNoRoom()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var response = await fixture.Client.GetAsync($"api/rooms/{accommodationId}");
//
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            var content = await response.Content.ReadAsStringAsync();
//            Assert.Equal("[]", content);
//
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsStatusCodeCreated()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var roomEntity = new Room { Name = "Lyxar1", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
//            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//
//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var roomResponse = JsonConvert.DeserializeObject<RoomWithIdDto>(responseContent);
//
//            Assert.Equal(roomResponse?.Name, roomEntity.Name);
//            Assert.Equal(roomResponse?.AccommodationId, roomEntity.AccommodationId);
//            Assert.Equal(roomResponse?.RoomType, roomEntity.RoomType);
//            Assert.Equal(roomResponse?.Capacity, roomEntity.Capacity);
//            Assert.Equal(roomResponse?.SquareInMeter, roomEntity.SquareInMeter);
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsBadRequest400_WhenAddVoid()
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDict>(responseContent);
//            var errMsg = errorResponse?.Errors[""].First();
//
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//            Assert.Equal("A non-empty request body is required.", errMsg);
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsBadRequest400_WhenAddRoomIncomplete()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var roomEntity = new Room { Name = "Lyxar" };
//            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//
//            var errorResponseString = await response.Content.ReadAsStringAsync();
//            var errorResponse = JsonConvert.DeserializeObject<ErrorData>(errorResponseString);
//
//            Assert.Equal("Capacity should be more than zero.", errorResponse?.Errors["Capacity"].First());
//            Assert.Equal("The RoomType field is required.", errorResponse?.Errors["RoomType"].First());
//            Assert.Equal("SquareInMeter should be more than zero.", errorResponse?.Errors["SquareInMeter"].First());
//            Assert.Equal("AccommodationId should be more than zero.", errorResponse?.Errors["AccommodationId"].First());
//
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsNotFound_WhenAddRoomWitFakeAccommodation()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id + 100;
//
//            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId , Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
//            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//
//            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
//            var errorMassage = await response.Content.ReadAsStringAsync();
//            Assert.Equal($"Accommodation with id {accommodationId} not found", errorMassage);
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsBadRequset_WhenAddFakeRoom()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var roomEntity = new FakeRoom { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = "ten", RoomType = "231" };
//            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//
//            var responseContent = await response.Content.ReadAsStringAsync();
//            var errorResponse1 = JsonConvert.DeserializeObject<ErrorResponseDict>(responseContent);
//            var error = errorResponse1?.Errors["roomDto"].First();
//
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//            Assert.Equal("The roomDto field is required.", error);
//            //Такая ошибка т.к. не отрабатывает JSON и не переводит текст в double
//        }
//
//        [Fact]
//        public async Task AddRoom_ReturnsBadRequset_WhenInvalidData()
//        {
//            var accommodationEntity = new Accommodation { Name = "Test" };
//            var accommodation = await fixture.InsertObjectIntoDatabase(accommodationEntity);
//            var accommodationId = accommodation.Id;
//
//            var roomEntity = new FakeRoom { Name = "Lyxar", AccommodationId = -100, Capacity = -200, SquareInMeter = "-20", RoomType = "asd" };
//            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/", content);
//
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//
//            var errorResponseString = await response.Content.ReadAsStringAsync();
//            var errorResponse = JsonConvert.DeserializeObject<ErrorData>(errorResponseString);
//
//            Assert.Equal("Capacity should be more than zero.", errorResponse?.Errors["Capacity"].First());
//            Assert.Equal("SquareInMeter should be more than zero.", errorResponse?.Errors["SquareInMeter"].First());
//            Assert.Equal("AccommodationId should be more than zero.", errorResponse?.Errors["AccommodationId"].First());
//        }
//
//        [Fact]
//        public async Task CreateRoomTest_WhenPostCalledAddMethod_ReturnsMessageOfSuccsessfulyCreated()
//        {
//            var room = await CreateRoomAsync();
//            var content = new StringContent(JsonConvert.SerializeObject(room.ToViewModel()), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms", content);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.Equal("The room has been added", await response.Content.ReadAsStringAsync());
//        }
//
//        [Fact]
//        public async Task UpdatedRoomTest_WhenPutCalledEditMethod_ReturnsMessageOfSuccsessfulyEdited()
//        {
//            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
//            var editedRoom = new Room
//            {
//                Id = room.Id,
//                AccommodationId = room.AccommodationId,
//                Name = "506"
//            };
//
//            var content = new StringContent(JsonConvert.SerializeObject(editedRoom.ToViewModel()), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PutAsync($"api/rooms", content);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.Equal("The room has been edited", await response.Content.ReadAsStringAsync());
//
//            response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
//            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomViewModel>();
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.NotNull(recivedRoom);
//            Assert.Equal(editedRoom.Name, recivedRoom.Name);
//        }
//
//        [Fact]
//        public async Task DeleteRoomTest_WhenDeletCalledRemoveMethod_ReturnsMessageOfSuccsessfulyRemoved()
//        {
//            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
//
//            var response = await fixture.Client.DeleteAsync($"api/rooms/{room.Id}");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.Equal("The room has been removed", await response.Content.ReadAsStringAsync());
//            response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//
//        }
//
//        [Fact]
//        public async Task GetRoomByIdTest_WhenGetCalledGetByIdMethod_ReturnsRoomViewModel()
//        {
//            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
//            var response = await fixture.Client.GetAsync($"api/rooms/{room.Id}");
//            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomViewModel>();
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.NotNull(recivedRoom);
//            Assert.Equal(room.Id, recivedRoom.Id);
//            Assert.Equal(room.AccommodationId, recivedRoom.AccommodationId);
//            Assert.Equal(room.Name, recivedRoom.Name);
//        }
//
//        [Fact]
//        public async Task GetFacilitiesByRoomIdTest_WhenGetCalledGetFacilitiesByRoomIdMethod_ReturnsIEnumerableOfRoomFacilitiyViewModels()
//        {
//            var room = await CreateRoomAsync();
//            room.RoomFacilities = new List<RoomFacility>
//            {
//                new RoomFacility(){ Name = "Air Conditioner" },
//                new RoomFacility(){ Name = "Minibar" },
//                new RoomFacility(){ Name = "Locker" }
//            };
//            room = await fixture.InsertObjectIntoDatabase(room);
//            var response = await fixture.Client.GetAsync($"api/rooms/{room.Id}/facilities");
//            var recivedRoomFacilities = await response.Content.ReadFromJsonAsync<IEnumerable<RoomFacilityViewModel>>();
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.NotNull(recivedRoomFacilities);
//            Assert.NotEmpty(recivedRoomFacilities);
//            Assert.Equal(3, recivedRoomFacilities.Count());
//            Assert.Equivalent(room.RoomFacilities.ToViewModel(), recivedRoomFacilities);
//
//        }
//
//        [Fact]
//        public async Task AddRoomFacilityToCurrentRoomTest_WhenPostCalledAddFacilityToRoomMethod_ReturnsMessageOfSuccsessfulyCreated()
//        {
//            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
//            var roomFacility = new RoomFacility { RoomId = room.Id, Name = "Locker" };
//
//            var content = new StringContent(JsonConvert.SerializeObject(roomFacility.ToViewModel()), Encoding.UTF8, "application/json");
//            var response = await fixture.Client.PostAsync($"api/rooms/{room.Id}/facilities", content);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//            Assert.Equal($"The room facility has been added to current room", await response.Content.ReadAsStringAsync());
//        }
//
//        private async Task<Room> CreateRoomAsync()
//        {
//            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow" });
//            var address = await fixture.InsertObjectIntoDatabase(new Address
//            {
//                CityId = city.Id,
//                Street = "Prospect Mira",
//                ZipCode = "123456",
//                Longitude = 100,
//                Latitude = 500
//            });
//            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "MyHotel" });
//            return new Room
//            {
//                AccommodationId = accommodation.Id,
//                Name = "DeluxeRoom"
//            };
//        }
//
//        private AddressDto CreateValidAddressDto(int cityId)
//        {
//            return new AddressDto
//            {
//                Street = "StreetTest",
//                ZipCode = "654321",
//                Longitude = 0,
//                Latitude = 0,
//                CityId = cityId
//            };
//        }
//        private class FakeRoom : IntBaseEntity
//        {
//            public string Name { get; set; }
//            public string SquareInMeter { get; set; }
//
//            public string RoomType { get; set; }
//
//            public int Capacity { get; set; }
//            public int AccommodationId { get; set; }
//            public virtual Accommodation Accommodation { get; set; }
//        }
//    }
//}
//
//
