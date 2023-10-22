namespace YourRest.Application.Exceptions
{
    public class MissingParameterException : ApplicationLayerException
    {
        public MissingParameterException(string message) : base(message) { }
    }
}