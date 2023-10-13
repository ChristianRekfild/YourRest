//using Newtonsoft.Json;
//using System.Net;
//using System.Text;
//using YourRest.Application.Dto;
//using YourRest.Domain.Entities;
//using YourRest.WebApi.Tests.Fixtures;
//
//namespace YourRest.WebApi.Tests.Controllers
//{
//    public class ReviewControllerTest : IClassFixture<SingletonApiTest> //ApiTest
//    {
//        //public ReviewControllerTest(ApiFixture fixture) : base(fixture)
//        //{
//        //}
//        private readonly SingletonApiTest fixture;
//        public ReviewControllerTest(SingletonApiTest fixture)
//        {
//            this.fixture = fixture;
//        }
//
//        [Fact]
//        public async Task GivenBookingAndCorrectReviewData_WhenPostCalled_ReturnsCreatedAtAction()
//        {
//            var customer = new Customer
//                {
//                    FirstName = "Test",
//                    LastName = "Test",
//                    MiddleName = "Test",
//                    IsActive = true,
//                    Login = "ivanov@ivan.com",
//                    Password = "qwerty"
//                };
//            var customerId = (await fixture.InsertObjectIntoDatabase(customer)).Id;
//
//            var booking = new Booking {
//                StartDate = new DateTime(2023, 10, 1),
//                EndDate = new DateTime(2023, 10, 5),
//                Status = YourRest.Domain.Entities.BookingStatus.Pending,
//                Comment = "test",
//                CustomerId = customerId
//            };
//
//            var bookingId = (await fixture.InsertObjectIntoDatabase(booking)).Id;
//
//            var review = new ReviewDto {
//                BookingId = bookingId,
//                Comment = "test",
//                Rating = 1
//            };
//            var content = new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json");
//
//            var response = await fixture.Client.PostAsync("api/operator/review", content);
//
//            response.EnsureSuccessStatusCode();
//
//            var responseString = await response.Content.ReadAsStringAsync();
//
//            var createdReview = JsonConvert.DeserializeObject<SavedReviewDto>(responseString);
//
//            Assert.True(createdReview?.Id > 0);
//        }
//
//        [Fact]
//        public async Task GivenReviewDataWithoutExistBooking_WhenPostCalled_ReturnsNotFound()
//        {
//
//            var invalidReview = new ReviewDto {
//                BookingId = 3,
//                Comment = "test",
//                Rating = 1
//            };
//
//            var content = new StringContent(JsonConvert.SerializeObject(invalidReview), Encoding.UTF8, "application/json");
//
//            var response = await fixture.Client.PostAsync("api/operator/review", content);
//
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }
//    }
//}
