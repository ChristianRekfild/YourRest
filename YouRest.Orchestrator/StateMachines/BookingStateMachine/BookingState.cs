using MassTransit;

namespace YouRest.Orchestrator.StateMachines.BookingStateMachine
{
    public class BookingState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
