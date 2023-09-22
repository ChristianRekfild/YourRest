namespace YourRest.WebApi.BookingContext.Application.CustomErrors
{
    public class CityNotFountException : Exception
    {
        public int CityId { get; }

        public CityNotFountException(string message)
        : base(message) { }

        public CityNotFountException(string message, Exception inner)
            : base(message, inner) { }

        public CityNotFountException(string message, int cityId)
            : this(message)
        {
            CityId = cityId;
        }
    }
}
