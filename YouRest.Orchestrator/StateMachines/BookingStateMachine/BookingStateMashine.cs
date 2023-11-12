using MassTransit;
using YouRest.Booking.Contracts;

namespace YouRest.Orchestrator.StateMachines.BookingStateMachine
{
    public class BookingStateMashine : MassTransitStateMachine<BookingState>
    {
        public State AwaitingManagerConfirmation { get; private set; }

        public Event<BookingSubmitted> BookingSubmitted { get; private set; }
        public Event<ConfirmBooking> BookingConfirmed { get; private set; }
        
        public BookingStateMashine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => BookingSubmitted, context => context.CorrelateById(x => x.Message.CorrelationId));
            Event(() => BookingConfirmed, context => context.CorrelateById(x => x.Message.CorrelationId));

            Initially(
                When(BookingSubmitted)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.BookingId = context.Message.BookingId;
                    context.Saga.CustomerId = context.Message.CustomerId;
                    context.Saga.AccommodationId = context.Message.AccommodationId;
                    context.Saga.StartDate = context.Message.StartDate;
                    context.Saga.EndDate = context.Message.EndDate;
                })
                //.PublishAsync(context => context.Init<ConfirmBooking>(new
                //{
                //    context.Saga.CorrelationId,
                //    context.Saga.BookingId,
                //    context.Saga.CustomerId,
                //    context.Saga.AccommodationId,
                //    context.Saga.StartDate,
                //    context.Saga.EndDate
                //}))
                .TransitionTo(AwaitingManagerConfirmation));

            During(AwaitingManagerConfirmation,
                When(BookingConfirmed)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                })
                .PublishAsync(context => context.Init<ConfirmBooking>(new { CorrelationId = context.Saga.CorrelationId }))
                .TransitionTo(AwaitingManagerConfirmation));
        }
    }
}
