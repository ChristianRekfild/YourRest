namespace YourRest.Domain.Events
{
    public class AccommodationCreatedEvent : IDomainEvent
    {
        public int Id { get; }

        public AccommodationCreatedEvent(int accommodationId)
        {
            Id = accommodationId;
        }
    }
}