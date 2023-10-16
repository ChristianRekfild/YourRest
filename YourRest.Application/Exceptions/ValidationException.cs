namespace YourRest.Application.Exceptions
{
    public class ValidationException : ApplicationLayerException
    {
        public ValidationException(string message) : base(message) { }
    }
}