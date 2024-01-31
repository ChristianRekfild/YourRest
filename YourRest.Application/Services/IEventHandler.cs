using YourRest.Domain.Events;

namespace YourRest.Application.Services
{
    public interface IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent);
    }
}