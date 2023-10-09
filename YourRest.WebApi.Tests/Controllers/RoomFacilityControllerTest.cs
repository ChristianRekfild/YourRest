
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
    public class RoomFacilityControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        private int CityId { get; set; }
        private int AddressId { get; set; }
        private int AccommodationId { get; set; }
        private int RoomId { get; set; }
        public RoomFacilityControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task CreateRoomFacilityTest_WhenPostCalledAddMethod_ReturnsMessageOfSuccsessfulyCreated()
        {
            var roomFacility = await CreateRoomFacilityAsync();
            var content = new StringContent(JsonConvert.SerializeObject(roomFacility.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facility/add", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"Service \"{roomFacility.Name}\" has been added to the current room", response.Content.ReadAsStringAsync().Result);
        }
        [Fact]
        public async Task UpdatedRoomFacilityTest_WhenPutCalledEditMethod_ReturnsMessageOfSuccsessfulyEdited()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var editedRoom = new RoomFacility
            {
                Id = roomFacility.Id,
                RoomId = RoomId,
                Name = "Minibar"
            };
            var content = new StringContent(JsonConvert.SerializeObject(editedRoom.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facility/edit", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"RoomFacility id:{roomFacility.Id} has been successfully changed in the current issue", response.Content.ReadAsStringAsync().Result);
            var recivedRoomFacility = await GetByIdAsync(roomFacility.Id);
            Assert.Equal(editedRoom.Name, recivedRoomFacility.Name);
        }
        [Fact]
        public async Task DeleteRoomFacilityTest_WhenPostCalledRemoveMethod_ReturnsMessageOfSuccsessfulyRemoved()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var content = new StringContent(JsonConvert.SerializeObject(roomFacility.ToViewModel()), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facility/remove", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"RoomFacility id:{roomFacility.Id} has been removed from the current room", response.Content.ReadAsStringAsync().Result);
            response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facility/{roomFacility.Id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetRoomFacilityByIdTest_WhenGetCalledGetByIdMethod_ReturnsRoomFacilityViewModel()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var recivedRoomFacility = await GetByIdAsync(roomFacility.Id);
            Assert.Equal(roomFacility.Id, recivedRoomFacility.Id);
            Assert.Equal(roomFacility.Name, recivedRoomFacility.Name);
        }
        [Fact]
        public async Task GetAllRoomFacilitiesTest_WhenGetCalledGetRoomFacilitiesMethod_ReturnsIEnumerableOfRoomFacilityViewModel()
        {
            var roomFacilityFirst = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var roomFacilitySecond = new RoomFacility 
            {
                RoomId = RoomId,
                Name = "Minibar",
            };
            await fixture.InsertObjectIntoDatabase(roomFacilitySecond);

            var response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facilities");
            var recivedRoomFacility = await response.Content.ReadFromJsonAsync<IEnumerable<RoomFacilityViewModel>>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(recivedRoomFacility);
        }

        private async Task<RoomFacility> CreateRoomFacilityAsync()
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
            var room = await fixture.InsertObjectIntoDatabase(new Room
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom"
            });
           
            CityId = city.Id;
            AddressId = address.Id;
            AccommodationId = accommodation.Id;
            RoomId = room.Id;
            return new RoomFacility
            {
                RoomId = room.Id,
                Name = "Air Conditioner"
            };
        }
        private async Task<RoomFacility> GetByIdAsync(int id)
        {
            var response = await fixture.Client.GetAsync($"api/operator/1/accommodation/{AccommodationId}/room/{RoomId}/facility/{id}");
            var recivedRoomFacility = await response.Content.ReadFromJsonAsync<RoomFacilityViewModel>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoomFacility);
            return recivedRoomFacility.ToEntity();
        }
    }
}


