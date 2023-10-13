namespace YourRest.Application.CustomErrors
{
    public class UserReviewCreationException : Exception
    {
        public UserReviewCreationException(int UserId, int BookingId)
         : base($"User with id {UserId} cannot create a review for booking {BookingId}.")
        {
        }
    }
}