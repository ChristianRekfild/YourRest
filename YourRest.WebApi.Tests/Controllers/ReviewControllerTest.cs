using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            
            var token = await GetAccessTokenAsync();

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
            var token = await GetAccessTokenAsync();

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var invalidReview = new ReviewDto {
                BookingId = 3,
                Comment = "test",
                Rating = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/operator/review", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            fixture.CleanDatabase();
        }
        
        [Fact]
        public async Task WhenPostCalledWithoutAuthCredentials_Returns401Unauthorised()
        {
            var accommodation = new Accommodation
            {
                Name = "Test2"
            };

            await fixture.InsertObjectIntoDatabase(accommodation);
            
            var invalidReview = new ReviewDto {
                BookingId = 3,
                Comment = "test",
                Rating = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/operator/review", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            fixture.CleanDatabase();
        }
        
        private async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://keycloak:8080/auth/realms/YourRest/protocol/openid-connect/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = "your_rest_app",
                ["client_secret"] = "qBC5V3wc2AYKTcYN1CACo6REU9t1Inrf",
                ["username"] = "lilia.retsia",
                ["password"] = "123456"
            });

            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<JObject>(responseContent);
                return tokenResponse["access_token"].ToString();
            }
            return null;
        }

    }
}
