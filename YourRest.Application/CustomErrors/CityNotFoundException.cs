namespace YourRest.Application.CustomErrors
{
    public class CityNotFoundException : Exception
    {
        public int CityId { get; }

        public CityNotFoundException(string message)
        : base(message) { }

        public CityNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public CityNotFoundException(string message, int cityId)
            : this(message)
        {
            CityId = cityId;
        }
    }
}
