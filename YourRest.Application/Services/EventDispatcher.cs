using Microsoft.Extensions.DependencyInjection;
using YourRest.Domain.Events;

namespace YourRest.Application.Services
{
    public class EventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.Handle(domainEvent);
            }
        }
    }
}