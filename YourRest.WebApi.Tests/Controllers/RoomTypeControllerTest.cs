using Newtonsoft.Json;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class RoomTypeControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public RoomTypeControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetAllRoomTypes() 
        {
            var roomLux = new RoomType{ Name = "Lux" };
            var roomStandart = new RoomType { Name = "Standart" };
            var roomApart = new RoomType { Name = "Apart" };

            var lux = await fixture.InsertObjectIntoDatabase(roomLux);
            var standart = await fixture.InsertObjectIntoDatabase(roomStandart);
            var apart = await fixture.InsertObjectIntoDatabase(roomApart);

            var response = await fixture.Client.GetAsync($"api/roomTypes");
            var content = await response.Content.ReadAsStringAsync();
            var types = JsonConvert.DeserializeObject<List<RoomTypeDto>>(content);
            Assert.NotNull(types);
            
            Assert.Contains(types, type => type.Name == roomLux.Name);
            Assert.Contains(types, type => type.Name == roomStandart.Name);
            Assert.Contains(types, type => type.Name == roomApart.Name);
        }
    }
}


