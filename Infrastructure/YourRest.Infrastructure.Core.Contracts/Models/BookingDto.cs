namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class BookingDto : IntBaseEntityDto
    {
        public Guid SystemId { get; set; } = Guid.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public BookingStatusDto Status { get; set; }
        public int AdultNumber { get; set; }
        public int ChildrenNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Comment { get; set; }

        public int CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public ICollection<RoomDto> Rooms { get; set; }
        public BookingDto() => Rooms = new List<RoomDto>();

    }
}
