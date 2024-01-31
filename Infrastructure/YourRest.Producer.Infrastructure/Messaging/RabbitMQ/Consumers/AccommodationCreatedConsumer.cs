using MassTransit;
using YourRest.Domain.Events;
using YourRest.Domain.Repositories;
using YourRest.Domain.ValueObjects;

namespace YourRest.Producer.Infrastructure.Messaging.RabbitMQ.Consumers
{
    public class AccommodationCreatedConsumer : IConsumer<AccommodationCreatedEvent>
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationCreatedConsumer(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public async Task Consume(ConsumeContext<AccommodationCreatedEvent> context)
        {
            var message = context.Message;
            
            await _accommodationRepository.UpdateStateAsync(message.Id, AccommodationState.NeedVerify);
        }
    }
}