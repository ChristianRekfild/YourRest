using MassTransit;
using Newtonsoft.Json;
using System.Text;
using YouRest.Booking.Contracts;
using YourRest.Application.Dto.Models.HotelBooking;

namespace YouRest.Orchestrator.Consumers
{
    public class BookingSubmittedConsumer : IConsumer<BookingSubmitted>
    {
        private readonly IHttpClientFactory httpClientFactory;
        HttpClient httpClient;
        public BookingSubmittedConsumer(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task Consume(ConsumeContext<BookingSubmitted> context)
        {
            httpClient = httpClientFactory.CreateClient();
            var body = new HotelBookingDto()
            {
                AccommodationId = context.Message.AccommodationId,
                AdultNr = context.Message.AdultNr,
                ChildrenNr = context.Message.ChildrenNr,
                DateFrom = context.Message.StartDate,
                DateTo = context.Message.EndDate,
                TotalAmount = context.Message.TotalAmount
            };

            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("https://localhost:52892//api/hotelbooking", content);
        }
    }
}
