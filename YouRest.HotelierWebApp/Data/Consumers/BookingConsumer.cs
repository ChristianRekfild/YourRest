using MassTransit;
using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Consumers
{
    public class BookingConsumer : IConsumer<BookingModel>
    {
        private readonly ILogger<BookingConsumer> logger;

        public BookingConsumer(ILogger<BookingConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BookingModel> context)
        {
            var response = new BookingModel();
            await context.RespondAsync(response);
            logger.LogInformation($"You have received a new reservation from: {response.FirstName} {response.LastName}");
        }
    }
}
