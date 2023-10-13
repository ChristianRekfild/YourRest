namespace YourRest.Application.CustomErrors
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string Sub)
         : base($"User not found with sub {Sub}")
        {
        }
    }
}