using HotelManagementWebApi.Domain.ValueObjects.Booking;

namespace HotelManagementWebApi.Domain.Entities.Booking
{
    public class Booking : IntBaseEntity
    {
        public BookingDate StartDate { get; set; }
        public BookingDate EndDate { get; set; }
        public BookingStatus Status { get; set; }
        public string Comment { get; set; }

    }
}
