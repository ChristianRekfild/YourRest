using HotelManagementWebApi.Domain.ValueObjects.Review;
using BookingEntity = HotelManagementWebApi.Domain.Entities.Booking.Booking;

namespace HotelManagementWebApi.Domain.Entities.Review
{
    public class Review : IntBaseEntity
    {
        public BookingEntity Booking { get; set; }
        public Comment Comment { get; set; }
        public Rating Rating { get; set; }
        public int BookingId { get; set; } 

    }
}