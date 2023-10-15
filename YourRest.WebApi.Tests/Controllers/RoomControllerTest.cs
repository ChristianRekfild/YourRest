using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class RoomControllerTest : ApiTest
    {
        public RoomControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllRoom_ReturnsExpectedRoom_WhenDatabaseHasRoom()
        {
            var accommodationEntity = new Accommodation{Name = "Test"};
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, Id = 20, SquareInMeter = 30, RoomType = "Lyx" };
            var expectedRoom = await InsertObjectIntoDatabase(roomEntity);

            var response = await Client.GetAsync($"api/rooms/{accommodationId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var roomResponse = JsonConvert.DeserializeObject<List<RoomWithIdDto>>(content);

            Assert.Equal(roomResponse?.First().Id, expectedRoom.Id);
            Assert.Equal(roomResponse?.First().Name, expectedRoom.Name);
            Assert.Equal(roomResponse?.First().AccommodationId, expectedRoom.AccommodationId);
            Assert.Equal(roomResponse?.First().RoomType, expectedRoom.RoomType);
            Assert.Equal(roomResponse?.First().Capacity, expectedRoom.Capacity);
            Assert.Equal(roomResponse?.First().SquareInMeter, expectedRoom.SquareInMeter);
        }


        [Fact]
        public async Task GetAllRoom_ReturnsVoid_WhenDatabaseHasNoRoom()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var response = await Client.GetAsync($"api/rooms/{accommodationId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("[]", content);

        }

        [Fact]
        public async Task AddRoom_ReturnsStatusCodeCreated()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new Room { Name = "Lyxar1", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var roomResponse = JsonConvert.DeserializeObject<RoomWithIdDto>(responseContent);

            Assert.Equal(roomResponse?.Name, roomEntity.Name);
            Assert.Equal(roomResponse?.AccommodationId, roomEntity.AccommodationId);
            Assert.Equal(roomResponse?.RoomType, roomEntity.RoomType);
            Assert.Equal(roomResponse?.Capacity, roomEntity.Capacity);
            Assert.Equal(roomResponse?.SquareInMeter, roomEntity.SquareInMeter);
        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequest400_WhenAddVoid()
        {
            var content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseDict>(responseContent);
            var errMsg = errorResponse?.Errors[""].First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); 
            Assert.Equal("A non-empty request body is required.", errMsg);
        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequest400_WhenAddRoomIncomplete()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new Room { Name = "Lyxar" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorData>(errorResponseString);

            Assert.Equal("Capacity should be more than zero.", errorResponse?.Errors["Capacity"].First());
            Assert.Equal("The RoomType field is required.", errorResponse?.Errors["RoomType"].First());
            Assert.Equal("SquareInMeter should be more than zero.", errorResponse?.Errors["SquareInMeter"].First());
            Assert.Equal("AccommodationId should be more than zero.", errorResponse?.Errors["AccommodationId"].First());

        }

        [Fact]
        public async Task AddRoom_ReturnsNotFound_WhenAddRoomWitFakeAccommodation()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id + 100;

            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId , Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {accommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(errorMassage, expectedMessageJson);
        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequset_WhenAddFakeRoom()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new FakeRoom { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = "ten", RoomType = "231" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse1 = JsonConvert.DeserializeObject<ErrorResponseDict>(responseContent);
            var error = errorResponse1?.Errors["roomDto"].First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The roomDto field is required.", error);
            //Такая ошибка т.к. не отрабатывает JSON и не переводит текст в double
        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequset_WhenInvalidData()
        {
            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new FakeRoom { Name = "Lyxar", AccommodationId = -100, Capacity = -200, SquareInMeter = "-20", RoomType = "asd" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorData>(errorResponseString);

            Assert.Equal("Capacity should be more than zero.", errorResponse?.Errors["Capacity"].First());
            Assert.Equal("SquareInMeter should be more than zero.", errorResponse?.Errors["SquareInMeter"].First());
            Assert.Equal("AccommodationId should be more than zero.", errorResponse?.Errors["AccommodationId"].First());
        }

        private AddressDto CreateValidAddressDto(int cityId)
        {
            return new AddressDto
            {
                Street = "StreetTest",
                ZipCode = "654321",
                Longitude = 0,
                Latitude = 0,
                CityId = cityId
            };
        }
        private class FakeRoom : IntBaseEntity
        {
            public string Name { get; set; }
            public string SquareInMeter { get; set; }

            public string RoomType { get; set; }

            public int Capacity { get; set; }
            public int AccommodationId { get; set; }
            public virtual Accommodation Accommodation { get; set; }
        }
    }
}


