﻿namespace YouRest.Booking.Contracts
{
    public interface BookingSubmitted
    {
        public Guid CorrelationId { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int AdultNr { get; set; }
        public int ChildrenNr { get; set; }
    }
}
