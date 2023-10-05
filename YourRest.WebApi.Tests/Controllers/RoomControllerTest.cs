using Newtonsoft.Json;
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


        //[Fact]
        //public async Task AddRoom_ReturnsStatusCodeOk()
        //{

        //    var accommodationEntity = new Accommodation { Name = "Test" };
        //    var accommodation = await InsertObjectIntoDatabase(accommodationEntity);
        //    var accommodationId = accommodation.Id;

        //    var roomEntity = new Room { Name = "Lyxar", AccommodationId = accommodationId, Capacity = 20, Id = 20, SquareInMeter = 30, RoomType = "Lyx" };
        //    var content = new StringContent(JsonConvert.SerializeObject(roomEntity), Encoding.UTF8, "application/json");
        //    var response = await Client.PostAsync($"api/rooms/", content);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}



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

    }
}


