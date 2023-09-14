using System;
using System.Diagnostics;

namespace HotelManagementWebApi.Domain.ValueObjects.Booking
{
    public class BookingDate : BaseDateValueObject
    {
        public BookingDate(DateTime value) : base(value)
        {

        }
    }
}
