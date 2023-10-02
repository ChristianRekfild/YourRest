namespace YourRest.Application.CustomErrors
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException(int countryId)
         : base($"Country with id {countryId} not found")
        {
        }
    }
}