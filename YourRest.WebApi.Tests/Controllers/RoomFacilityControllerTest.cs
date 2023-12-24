using AutoMapper;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using YourRest.Application.Dto.Mappers.Profiles;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class RoomFacilityControllerTest
    {
        private readonly SingletonApiTest fixture;
        private int CityId { get; set; }
        private int AddressId { get; set; }
        private int AccommodationId { get; set; }
        private int RoomId { get; set; }
        private Room Room { get; set; }
        public RoomFacilityControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public async Task UpdatedRoomFacilityTest_WhenPutCalledEditMethod_ReturnsMessageOfSuccsessfulyEdited()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var editedRoomFacility = new RoomFacility
            {
                Name = "Minibar"
            };
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RoomFacilityDtoProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var content = new StringContent(JsonConvert.SerializeObject(mapper.Map<RoomFacilityDto>(editedRoomFacility)), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/roomfacilities/{roomFacility.Id}", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"RoomFacility id:{roomFacility.Id} has been successfully changed in the current issue", await response.Content.ReadAsStringAsync());
            var recivedRoomFacility = await GetByIdAsync(roomFacility.Id);
            Assert.Equal(editedRoomFacility.Name, recivedRoomFacility.Name);
        }
        [Fact]
        public async Task DeleteRoomFacilityTest_WhenPostCalledRemoveMethod_ReturnsMessageOfSuccsessfulyRemoved()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            var response = await fixture.Client.DeleteAsync($"api/roomfacilities/{roomFacility.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal($"RoomFacility id:{roomFacility.Id} has been removed from the current room", await response.Content.ReadAsStringAsync());
            response = await fixture.Client.GetAsync($"api/roomfacilities/{roomFacility.Id}");
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
        public async Task GetAllRoomFacilitiesTest_WhenGetCalledGetAllRoomFacilitiesMethod_ReturnsListOfRoomFacilityDto()
        {
            var roomFacility = await fixture.InsertObjectIntoDatabase(await CreateRoomFacilityAsync());
            Room.RoomFacilities.Add(new RoomFacility { Name = "Ironing station" });
            Room.RoomFacilities.Add(new RoomFacility { Name = "Locker" });
            await fixture.DbContext.SaveChangesAsync();
            List<RoomFacility> roomFacilities = new(Room.RoomFacilities);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RoomFacilityDtoProfile());
                cfg.AddProfile(new RoomFacilityWithIdProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var response = await fixture.Client.GetAsync($"api/roomfacilities");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(mapper.Map< IEnumerable<RoomFacilityDto>>(roomFacilities), await response.Content.ReadFromJsonAsync<IEnumerable<RoomFacilityDto>>());
        }

        private async Task<RoomFacility> CreateRoomFacilityAsync()
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
            var room = await fixture.InsertObjectIntoDatabase(new Room
            {
                AccommodationId = accommodation.Id,
                Name = "DeluxeRoom",
                RoomType = roomType
            });
           
            CityId = city.Id;
            AddressId = address.Id;
            AccommodationId = accommodation.Id;
            RoomId = room.Id;
            Room = room;
            return new RoomFacility
            {
                Name = "Air Conditioner"
            };
        }
        private async Task<RoomFacilityWithIdDto> GetByIdAsync(int id)
        {
            var response = await fixture.Client.GetAsync($"api/roomfacilities/{id}");
            var recivedRoomFacility = await response.Content.ReadFromJsonAsync<RoomFacilityWithIdDto>();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(recivedRoomFacility);
            return recivedRoomFacility;
        }
    }
}


