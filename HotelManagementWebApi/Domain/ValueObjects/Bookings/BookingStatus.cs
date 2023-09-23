namespace HotelManagementWebApi.Domain.ValueObjects.Bookings
{
    public class BookingStatus : BaseIntValueObject
    {
        public static readonly int Confirmed = 1;
        public static readonly int Cancelled = 2;

        public BookingStatus(int value) : base(value)
        {
            if (value != Confirmed && value != Cancelled) {
                throw new ArgumentException("Status can only be Confirmed or Cancelled.");
            }
                
        }
    }
}
