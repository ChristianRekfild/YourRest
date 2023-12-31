﻿namespace YourRest.Domain.Entities
{
    public class Booking : IntBaseEntity
    {
        public Guid SystemId { get; set; } = Guid.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public int AdultNumber { get; set; }
        public int ChildrenNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Comment { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public Booking() => Rooms = new List<Room>();

    }
}
