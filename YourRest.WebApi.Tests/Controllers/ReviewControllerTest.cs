using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using YourRest.Domain.Repositories;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class ReviewControllerTest
    {
        private readonly SingletonApiTest fixture;
        private readonly ITokenRepository _tokenRepository;
        public ReviewControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            var scope = fixture.Server.Host.Services.CreateScope();

            _tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
        }

        [Fact]
        public async Task GivenBookingAndCorrectReviewData_WhenPostCalled_ReturnsCreatedAtAction()
        {
            string userId = await CreateKeyCloakUser();

            var user = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.ru",
                KeyCloakId = userId
            };
            var customer = new Customer
                {
                    FirstName = "Test",
                    LastName = "Test",
                    MiddleName = "Test"
                };
            var customerId = (await fixture.InsertObjectIntoDatabase(customer)).Id;
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = new Accommodation
            {
                Name = "Test2",
                AccommodationType = accommodationType
            };
            
            user.UserAccommodations.Add(new UserAccommodation { User = user, Accommodation = accommodation });
            
            await fixture.InsertObjectIntoDatabase(accommodation);
            await fixture.InsertObjectIntoDatabase(user);

            var booking = new Booking {
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 10, 5),
                Status = BookingStatus.Pending,
                Comment = "test",
                Customer = customer
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
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = new Accommodation
            {
                Name = "Test2",
                AccommodationType = accommodationType
            };

            await fixture.InsertObjectIntoDatabase(accommodation);
            await CreateKeyCloakUser();
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
        }
        
        [Fact]
        public async Task WhenPostCalledWithoutAuthCredentials_Returns401Unauthorised()
        {
            fixture.Client.DefaultRequestHeaders.Authorization = null;
            
            var response = await fixture.Client.PostAsync("api/operator/review", null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        private async Task<string> CreateKeyCloakUser()
        {
            string? userId;
            
            string adminToken = (await _tokenRepository.GetAdminTokenAsync()).access_token;

            try
            {
                userId = await _tokenRepository.CreateUser(adminToken,  "test", "test", "test", "test@test.ru", "test");
                return userId;
            }
            catch { }

            return null;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return (await _tokenRepository.GetTokenAsync("test", "test")).access_token;
        }
    }
}
