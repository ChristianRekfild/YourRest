namespace HotelManagementWebApi.Domain.ValueObjects.Booking
{
    public class BookingDate : BaseDateValueObject
    {
        public BookingDate(DateTime value) : base(value)
        {
            if (value < DateTime.Now) {
                throw new ArgumentException("Booking date cannot be in the past.");
            }
        }
    }
}
