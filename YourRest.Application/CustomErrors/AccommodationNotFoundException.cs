namespace YourRest.Application.CustomErrors
{
    public class AccommodationNotFoundException : Exception
    {
        public AccommodationNotFoundException(int accommodationId)
         : base($"Accommodation with id {accommodationId} not found")
        {
        }
    }
}