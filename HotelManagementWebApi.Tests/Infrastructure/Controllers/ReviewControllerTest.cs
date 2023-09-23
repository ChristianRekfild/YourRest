using HotelManagementWebApi.Application.UseCase.Reviews.Dto;

using HotelManagementWebApi.Domain.ValueObjects.Bookings;
using HotelManagementWebApi.Tests.Fixtures;
using Newtonsoft.Json;
using SharedKernel.Domain.Entities;
using System.Net;
using System.Text;
using YourRest.Infrastructure.Repositories;

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
            var customer = new Customer
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    IsActive = true,
                    Login = "ivanov@ivan.com",
                    Password = "qwerty"
                };
            var customerId = await InsertObjectIntoDatabase(customer);

            var booking = new Booking {
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 10, 5),
                Status = SharedKernel.Domain.Entities.BookingStatus.Pending,
                Comment = "test",
                CustomerId = customerId
            };

            var bookingId = await InsertObjectIntoDatabase(booking);

            var review = new ReviewDto {
                BookingId = bookingId,
                Comment = "test",
                Rating = SharedKernel.Domain.Entities.Rating.One
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
                Rating = SharedKernel.Domain.Entities.Rating.One
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("api/operator/review", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
