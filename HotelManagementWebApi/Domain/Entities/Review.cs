using HotelManagementWebApi.Domain.ValueObjects.Reviews;
using SharedKernel.Domain.Entities;

namespace HotelManagementWebApi.Domain.Entities
{
    [Obsolete]
    public class Review : IntBaseEntity
    {
        public Booking Booking { get; set; }
        public Comment Comment { get; set; }
        public ValueObjects.Reviews.Rating Rating { get; set; }
        public int BookingId { get; set; }

    }
}