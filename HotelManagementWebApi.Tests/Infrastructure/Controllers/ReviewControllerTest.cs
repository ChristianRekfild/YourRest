using HotelManagementWebApi.Application.UseCase.Review.Dto;
using HotelManagementWebApi.Domain.Entities.Booking;
using HotelManagementWebApi.Tests.Fixtures;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using BookingDateVO = HotelManagementWebApi.Domain.ValueObjects.Booking.BookingDate;
using BookingStatusVO = HotelManagementWebApi.Domain.ValueObjects.Booking.BookingStatus;

namespace HotelManagementWebApi.Tests.Infrastructure.Adapters.Controllers
{
    public class ReviewControllerTest : ApiTest
    {
        public ReviewControllerTest(ApiFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GivenBookingAndCorrectReviewData_WhenPostCalled_ReturnsCreatedAtAction()
        {
            var booking = new Booking {
                StartDate = new BookingDateVO(new DateTime(2023, 10, 1)),
                EndDate = new BookingDateVO(new DateTime(2023, 10, 5)),
                Status = new BookingStatusVO(1),
                Comment = "test"
            };

            var bookingId = await InsertObjectIntoDatabase(booking);

            var review = new ReviewDto {
                BookingId = bookingId,
                Comment = "test",
                Rating = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("api/operator/review", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();           
           
            var createdReview = JsonConvert.DeserializeObject<SavedReviewDto>(responseString);

            Assert.True(createdReview.Id > 0);
        }

        [Fact]
        public async Task GivenReviewDataWithoutExistBooking_WhenPostCalled_ReturnsNotFound()
        {
            
            var invalidReview = new ReviewDto {
                BookingId = 3,
                Comment = "test",
                Rating = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("api/operator/review", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
