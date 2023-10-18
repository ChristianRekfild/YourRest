namespace YourRest.Application.Exceptions
{
    public class EntityConflictException : ApplicationLayerException
    {
        public EntityConflictException(string message) : base(message) { }
    }
}