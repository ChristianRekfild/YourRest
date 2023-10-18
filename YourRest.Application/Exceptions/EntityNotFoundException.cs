namespace YourRest.Application.Exceptions
{
    public class EntityNotFoundException : ApplicationLayerException
    {
        public EntityNotFoundException(string message) : base(message) { }
    }
}