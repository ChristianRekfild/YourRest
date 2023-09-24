namespace YourRestDomain.Entities
{
    public class BookingEntity : BaseEntity<int>
    {
        public CustomerEntity Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; } // Status of the booking (e.g., Pending(1), Confirmed(2), Cancelled(3))
        public string Comment { get; set; }
        public int CustomerId { get; set; }
    }
}
