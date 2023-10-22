using Newtonsoft.Json;
using System.Net;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;
using System.Net.Http.Headers;

namespace YourRest.WebApi.Tests.Controllers
{
    public class ReviewControllerTest : IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public ReviewControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GivenBookingAndCorrectReviewData_WhenPostCalled_ReturnsCreatedAtAction()
        {
            var customer = new Customer
                {
                    FirstName = "Test",
                    LastName = "Test",
                    MiddleName = "Test",
                    IsActive = true,
                    Login = "ivanov@ivan.com",
                    Password = "qwerty"
                };
            var customerId = (await fixture.InsertObjectIntoDatabase(customer)).Id;

            var accommodation = new Accommodation
            {
                Name = "Test2"
            };

            var accommodationId = (await fixture.InsertObjectIntoDatabase(accommodation)).Id;

            var booking = new Booking {
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 10, 5),
                Status = YourRest.Domain.Entities.BookingStatus.Pending,
                Comment = "test",
                CustomerId = customerId,
                AccommodationId = accommodationId
            };

            var bookingId = (await fixture.InsertObjectIntoDatabase(booking)).Id;

            var review = new ReviewDto {
                BookingId = bookingId,
                Comment = "test",
                Rating = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");
            
            await fixture.CreateGroup(accommodationId);
            var token = await fixture.GetAccessTokenAsync();

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await fixture.Client.PostAsync("api/operator/review", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();           
           
            var createdReview = JsonConvert.DeserializeObject<SavedReviewDto>(responseString);

            Assert.True(createdReview?.Id > 0);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenReviewDataWithoutExistBooking_WhenPostCalled_ReturnsNotFound()
        {
            var accommodation = new Accommodation
            {
                Name = "Test2"
            };

            await fixture.InsertObjectIntoDatabase(accommodation);
            await fixture.CreateGroup(accommodation.Id);
            var token = await fixture.GetAccessTokenAsync();

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var invalidReview = new ReviewDto {
                BookingId = 3,
                Comment = "test",
                Rating = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/operator/review", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task WhenPostCalledWithoutAuthCredentials_Returns401Unauthorised()
        {
            fixture.Client.DefaultRequestHeaders.Authorization = null;
            
            var response = await fixture.Client.PostAsync("api/operator/review", null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
