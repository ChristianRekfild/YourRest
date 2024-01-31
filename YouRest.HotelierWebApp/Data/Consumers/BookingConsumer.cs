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
        public Task Consume(ConsumeContext<BookingModel> context)
        {

            logger.LogInformation($"[Received][{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] У Вас новое бронирование от: {context.Message.FirstName} {context.Message.LastName}");
            return Task.CompletedTask;
        }
    }
}