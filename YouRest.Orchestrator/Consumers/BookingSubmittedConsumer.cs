using MassTransit;
using YouRest.Booking.Contracts;

namespace YouRest.Orchestrator.Consumers
{
    public class BookingSubmittedConsumer : IConsumer<BookingSubmitted>
    {

        public Task Consume(ConsumeContext<BookingSubmitted> context)
        {
            return Task.CompletedTask;
        }
    }
}
