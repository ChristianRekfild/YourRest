namespace YourRest.Application.Exceptions
{
    public class InvalidParameterException : ApplicationLayerException
    {
        public InvalidParameterException(string message) : base(message) { }
    }
}