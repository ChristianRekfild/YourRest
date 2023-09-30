namespace YourRest.Application.CustomErrors
{
    public class AddressAlreadyExistsException : Exception
    {
        public AddressAlreadyExistsException(int accommodationId)
         : base($"Address for accommodation with id {accommodationId} already exists")
        {
        }
    }
}