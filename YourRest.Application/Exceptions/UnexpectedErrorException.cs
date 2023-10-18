namespace YourRest.Application.Exceptions
{
    public class UnexpectedErrorException : ApplicationLayerException
    {
        public UnexpectedErrorException(string message) : base(message) { }
    }
}