namespace YourRest.Application.CustomErrors
{
    public class CountryNotFoundException : Exception
    {
        public int CountryId { get; }

        public CountryNotFoundException(string message)
        : base(message) { }

        public CountryNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public CountryNotFoundException(string message, int countryIdityId)
            : this(message)
        {
            CountryId = countryIdityId;
        }
    }
}