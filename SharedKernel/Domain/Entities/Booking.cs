namespace SharedKernel.Domain.Entities
{
    public class Booking : IntBaseEntity
    {
        public Customer Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public string Comment { get; set; }
        public int CustomerId { get; set; } 
    }
}
