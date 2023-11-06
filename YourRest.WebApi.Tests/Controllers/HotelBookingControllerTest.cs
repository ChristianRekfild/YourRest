using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    public class HotelBookingControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public HotelBookingControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task AddHotelBooking_ReturnsStatusCodeCreated()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var addressEntity = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street",
                CityId = city.Id,
                ZipCode = "94105",
                Longitude = 120,
                Latitude = 75,
            });
            var accommodationType = new AccommodationType
            {
                Name = "Luxury"
            };
            var accomodation = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "GoldenHotel",
                AddressId = addressEntity.Id,
                AccommodationType = accommodationType
            });
            var hotelBooking = new HotelBookingDto {
                AccommodationId = accomodation.Id, 
                DateFrom = new DateTime(2023, 10, 5),  
                DateTo = new DateTime(2023, 10, 15),
                RoomId = 1,
                TotalAmount = 5000.0m,
                AdultNr = 2,
                ChildrenNr = 2 
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/hotelBooking", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<HotelBookingDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(hotelBooking.AccommodationId, hotelBookingResponse.AccommodationId);
            Assert.Equal(hotelBooking.DateTo, hotelBookingResponse.DateTo);
            Assert.Equal(hotelBooking.DateFrom, hotelBookingResponse.DateFrom);
            Assert.Equal(hotelBooking.RoomId, hotelBookingResponse.RoomId);
            Assert.Equal(hotelBooking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(hotelBooking.AdultNr, hotelBookingResponse.AdultNr);
            Assert.Equal(hotelBooking.ChildrenNr, hotelBookingResponse.ChildrenNr);
        }
    }
}
