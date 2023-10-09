using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
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

            //var cityEntity = new City { Name = "Moscow" };
            //var city = await InsertObjectIntoDatabase(cityEntity);
            //var addressDto = CreateValidAddressDto(city.Id);

            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, Id = 20, SquareInMeter = 30, RoomType = "Lyx" };
           
            var expectedRoom = await InsertObjectIntoDatabase(roomEntity);
           

            //var content = new StringContent(JsonConvert.SerializeObject(expectedRoom), Encoding.UTF8, "application/json");
            var response = await Client.GetAsync($"api/rooms/{accommodationId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var roomResponse = JsonConvert.DeserializeObject<List<RoomDto>>(content);

            Assert.Equal(roomResponse.First().Id, expectedRoom.Id);
            Assert.Equal(roomResponse.First().Name, expectedRoom.Name);
            Assert.Equal(roomResponse.First().AccommodationId, expectedRoom.AccommodationId);
            Assert.Equal(roomResponse.First().RoomType, expectedRoom.RoomType);
            Assert.Equal(roomResponse.First().Capacity, expectedRoom.Capacity);
            Assert.Equal(roomResponse.First().SquareInMeter, expectedRoom.SquareInMeter);
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
        }

        [Fact]
        public async Task GetAllRoom_ReturnsExpectedRoom_WhenAddRoom()
        {

            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);


            var roomResponse = await Client.GetAsync($"api/rooms/{accommodationId}");
            var roomResponseContent = await roomResponse.Content.ReadAsStringAsync();


            var roomReturn = JsonConvert.DeserializeObject<List<RoomDto>>(roomResponseContent);

            Assert.Equal(roomReturn.First().Name, roomEntity.Name);
            Assert.Equal(roomReturn.First().AccommodationId, roomEntity.AccommodationId);
            Assert.Equal(roomReturn.First().RoomType, roomEntity.RoomType);
            Assert.Equal(roomReturn.First().Capacity, roomEntity.Capacity);
            Assert.Equal(roomReturn.First().SquareInMeter, roomEntity.SquareInMeter);
        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequest400_WhenAddVoid()
        {
            var content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
        }

        [Fact]
        public async Task AddRoom_ReturnsConflict_WhenAddRoomWithNameDuplication()
        {

            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, SquareInMeter = 30, RoomType = "Lyx" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);


            var roomEntity1 = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 30, SquareInMeter = 30, RoomType = "Lyx" };
            var content1 = new StringContent(JsonConvert.SerializeObject(roomEntity1), Encoding.UTF8, "application/json");
            var response1 = await Client.PostAsync($"api/rooms/", content1);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(HttpStatusCode.Conflict, response1.StatusCode);
            var errorMassage = await response1.Content.ReadAsStringAsync();
            Assert.Equal($"Room with name {roomEntity1.Name} already exist", errorMassage);


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

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            Assert.Equal($"Accommodation with id {accommodationId} not found", errorMassage);

        }

        [Fact]
        public async Task AddRoom_ReturnsBadRequset_WhenAddFakeRoom()
        {


            var accommodationEntity = new Accommodation { Name = "Test" };
            var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
            var accommodationId = accommodation.Id;

            var roomEntity = new FakeRoom { Name = "Lyxar", AccommodationId = accommodationId + 100, Capacity = 20, SquareInMeter = "ten", RoomType = "Lyx" };
            var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync($"api/rooms/", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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


