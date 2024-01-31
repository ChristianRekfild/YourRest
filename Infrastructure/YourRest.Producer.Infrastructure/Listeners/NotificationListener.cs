using YourRest.Domain.Events;
using MassTransit;
using System.Threading.Tasks;
using YourRest.Application.Services;

namespace YourRest.Producer.Infrastructure.Listeners
{
    public class NotificationListener : IEventHandler<AccommodationCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotificationListener(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(AccommodationCreatedEvent accommodationCreatedEvent)
        {
            await _publishEndpoint.Publish(accommodationCreatedEvent);
        }
    }
}