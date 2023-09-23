using HotelManagementWebApi.Domain.ValueObjects.Bookings;
using SharedKernel.Domain.Entities;

namespace HotelManagementWebApi.Domain.Entities
{
    public class Booking : IntBaseEntity
    {
        public BookingDate StartDate { get; set; }
        public BookingDate EndDate { get; set; }
        public ValueObjects.Bookings.BookingStatus Status { get; set; }
        public string Comment { get; set; }

    }
}
