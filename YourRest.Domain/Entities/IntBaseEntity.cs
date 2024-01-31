using YourRest.Domain.Events;

namespace YourRest.Domain.Entities
{
    public abstract class IntBaseEntity : BaseEntity<int>, IDomainEvent
    {
    }
}
