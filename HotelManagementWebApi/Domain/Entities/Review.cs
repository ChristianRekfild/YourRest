using HotelManagementWebApi.Domain.ValueObjects.Reviews;
using SharedKernel.Domain.Entities;

namespace HotelManagementWebApi.Domain.Entities
{
    public class Review : IntBaseEntity
    {
        public Booking Booking { get; set; }
        public Comment Comment { get; set; }
        public Rating Rating { get; set; }
        public int BookingId { get; set; }

    }
}