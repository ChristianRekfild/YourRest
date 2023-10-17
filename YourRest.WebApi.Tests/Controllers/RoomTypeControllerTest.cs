using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
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
            var roomLux = new RoomTypeDto{ Name = "Lux" };
            var roomStandart = new RoomTypeDto { Name = "Standart" };
            var roomApart = new RoomTypeDto { Name = "Apart" };


            var lux = await InsertObjectIntoDatabase(roomLux);
            var standart = await InsertObjectIntoDatabase(roomStandart);
            var apart = await InsertObjectIntoDatabase(roomApart);

            var response = await Client.GetAsync($"api/rooms/roomTypes");
            var content = await response.Content.ReadAsStringAsync();
            var types = JsonConvert.DeserializeObject<List<RoomTypeDto>>(content);

            Assert.Equal(types[0].Name, lux.Name);
            Assert.Equal(types[1].Name, standart.Name);
            Assert.Equal(types[2].Name, apart.Name);
        }
    }
}


