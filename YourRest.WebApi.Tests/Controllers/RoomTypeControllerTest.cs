using Newtonsoft.Json;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class RoomTypeControllerTest : ApiTest
    {
        public RoomTypeControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllRoomTypes() 
        {
            var roomLux = new RoomType{ Id = 1, Name = "Lux" };
            var roomStandart = new RoomType { Id = 2, Name = "Standart" };
            var roomApart = new RoomType { Id = 3, Name = "Apart" };

            var lux = await InsertObjectIntoDatabase(roomLux);
            var standart = await InsertObjectIntoDatabase(roomStandart);
            var apart = await InsertObjectIntoDatabase(roomApart);

            var response = await Client.GetAsync($"api/roomTypes");
            var content = await response.Content.ReadAsStringAsync();
            var types = JsonConvert.DeserializeObject<List<RoomTypeDto>>(content);
            Assert.Equal(types[0].Name, lux.Name);
            Assert.Equal(types[0].Id, lux.Id);

            Assert.Equal(types[1].Name, standart.Name);
            Assert.Equal(types[1].Id, standart.Id);

            Assert.Equal(types[2].Name, apart.Name);
            Assert.Equal(types[2].Id, apart.Id);

        }
    }
}


