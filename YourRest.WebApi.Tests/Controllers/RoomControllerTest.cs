using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class RoomControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public RoomControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task CreateRoomTest_WhenPostCalledAddMethod_ReturnsMessageOfSuccsessfulyCreated()
        {
            var room = await CreateRoomAsync();
            var content = new StringContent(JsonConvert.SerializeObject(room.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/add", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The room has been added", response.Content.ReadAsStringAsync().Result);
        }
        [Fact]
        public async Task UpdatedRoomTest_WhenPutCalledEditMethod_ReturnsMessageOfSuccsessfulyEdited()
        {
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
            var editedRoom = new Room
            {
                Id = room.Id,
                AccommodationId = room.AccommodationId,
                Name = "506"
            };

            var content = new StringContent(JsonConvert.SerializeObject(editedRoom.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/{room.Id}/edit", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The room has been edited", response.Content.ReadAsStringAsync().Result);
           
            response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/{room.Id}");
            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomViewModel>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoom);
            Assert.Equal(editedRoom.Name, recivedRoom.Name);
        }
        [Fact]
        public async Task DeleteRoomTest_WhenDeletCalledRemoveMethod_ReturnsMessageOfSuccsessfulyRemoved()
        {
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
            
            var content = new StringContent(JsonConvert.SerializeObject(room.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/remove", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("The room has been removed", response.Content.ReadAsStringAsync().Result);
            response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/{room.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
        [Fact]
        public async Task GetRoomByIdTest_WhenGetCalledGetByIdMethod_ReturnsRoomViewModel()
        {
            var room = await fixture.InsertObjectIntoDatabase(await CreateRoomAsync());
            var response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{room.AccommodationId}/room/{room.Id}");
            var recivedRoom = await response.Content.ReadFromJsonAsync<RoomViewModel>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoom);
            Assert.Equal(room.Id, recivedRoom.Id);
            Assert.Equal(room.AccommodationId, recivedRoom.AccommodationId);
            Assert.Equal(room.Name, recivedRoom.Name);
        }


        private async Task<Room> CreateRoomAsync()
        {
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow" });
            var address = await fixture.InsertObjectIntoDatabase(new Address
            {
                CityId = city.Id,
                Street = "Prospect Mira",
                ZipCode = "123456",
                Longitude = 100,
                Latitude = 500
            });
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "MyHotel" });
            return new Room
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom"
            };
        }
    }
}
