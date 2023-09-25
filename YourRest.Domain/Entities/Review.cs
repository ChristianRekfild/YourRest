namespace YourRest.Domain.Entities
{
    public class Review : IntBaseEntity
    {
        public Booking Booking { get; set; }
        public string Comment { get; set; }
        public Rating Rating { get; set; }
        public int BookingId { get; set; }
    }
}
