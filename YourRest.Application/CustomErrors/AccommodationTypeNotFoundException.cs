namespace YourRest.Application.CustomErrors
{
    public class AccommodationTypeNotFoundException : Exception
    {
        public AccommodationTypeNotFoundException(int accommodationTypeId)
         : base($"Accommodation with id {accommodationTypeId} not found")
        {
        }
    }
}