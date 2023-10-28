using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
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
            var hotelBooking = new HotelBookingDto {
                HotelId = 1, 
                DateFrom = (2008, 6, 1),  
                DateTo = (2008, 8, 1),
                RoomId = 1,
                TotalAmount = 5000.0m,
                customerId = 1,
                Adults = new List<GuestDto> 
                { 
                    new GuestDto { LastName = "Mixalova", Firstname = "Elena", Middlename = "Petrova", DoB = (1985, 7, 1) },
                    new GuestDto { LastName = "Mixalov", Firstname = "Evgenii", Middlename = "Ivanov", DoB = (1982, 27, 10) }
                },
                Children = new List<GuestDto>
                {
                    new GuestDto { LastName = "Mixalov", Firstname = "Semen", Middlename = "Evgenievich", DoB = (2010, 10, 1) }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(hotelBooking), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operator/AgeRange", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var hotelBookingResponse = JsonConvert.DeserializeObject<AgeRangeDto>(responseContent);

            Assert.NotNull(hotelBookingResponse);
            Assert.Equal(hotelBooking.HotelId, hotelBookingResponse.HotelId);
            Assert.Equal(hotelBooking.DateFrom, hotelBookingResponse.DateFrom);
            Assert.Equal(hotelBooking.RoomId, hotelBookingResponse.RoomId);
            Assert.Equal(hotelBooking.TotalAmount, hotelBookingResponse.TotalAmount);
            Assert.Equal(hotelBooking.customerId, hotelBookingResponse.customerId);
            Assert.Equal(hotelBooking.Adults, hotelBookingResponse.Adults);
            Assert.Equal(hotelBooking.Children, hotelBookingResponse.Children);
        }

        //[Fact]
        //public async Task AddAgeRange_ReturnBadRequest_WhenAgeFromGreaterThanAgeTo()
        //{
        //    var ageRange = new AgeRangeDto() { AgeFrom = 14, AgeTo = 6 };
        //    var content = new StringContent(JsonConvert.SerializeObject(ageRange), Encoding.UTF8, "application/json");
        //    var response = await fixture.Client.PostAsync($"api/operator/AgeRange", content);

        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //    var errorData = response.Content.ReadFromJsonAsync<ErrorViewModel>();
        //    var errorDescription = errorData?.Result?.ValidationErrors["AgeTo"].FirstOrDefault();

        //    Assert.Equal($"'Age To' должно быть больше или равно '{ageRange.AgeFrom}'.", errorDescription);
        //}

        //[Fact]
        //public async Task GetAgeRange_ReturnStatusOK_WhenDbHasAgeRange()
        //{
        //    var ageRangeEntity = new AgeRange() { AgeFrom = 6, AgeTo = 14 };

        //    AgeRange ageRange = await fixture.InsertObjectIntoDatabase(ageRangeEntity);

        //    var response = await fixture.Client.GetAsync($"api/operator/AgeRange/{ageRange.Id}");

        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    var ageRangeResponse = JsonConvert.DeserializeObject<AgeRangeDto>(responseContent);


        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    Assert.NotNull(ageRangeResponse);
        //    Assert.Equal(ageRange.AgeFrom, ageRangeResponse.AgeFrom);
        //    Assert.Equal(ageRange.AgeTo, ageRangeResponse.AgeTo);
        //}

        //[Fact]
        //public async Task PutAgeRange_ReturnsBadRequest_WhenDbEmpty()
        //{

        //    var ageRangePut = new AgeRangeWithIdDto() { Id = 1, AgeFrom = 4, AgeTo = 16 };
        //    var content = new StringContent(JsonConvert.SerializeObject(ageRangePut), Encoding.UTF8, "application/json");
        //    var response = await fixture.Client.PutAsync($"api/operator/AgeRange/", content);

        //    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        //}

        //[Fact]
        //public async Task PutAgeRange_ReturnsStatusCodeOK_WhenDbHasAgeRange()
        //{
        //    var ageRangeEntity = new AgeRange() { AgeFrom = 6, AgeTo = 14 };
        //    AgeRange ageRange = await fixture.InsertObjectIntoDatabase(ageRangeEntity);

        //    var ageRangePut = new AgeRange() { Id = ageRange.Id, AgeFrom = 4, AgeTo = 16 };
        //    var content = new StringContent(JsonConvert.SerializeObject(ageRangePut), Encoding.UTF8, "application/json");
        //    var response = await fixture.Client.PutAsync($"api/operator/AgeRange/", content);

        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    Assert.Equal("The AgeRange has been edited", responseContent);
        //}
    }
}
