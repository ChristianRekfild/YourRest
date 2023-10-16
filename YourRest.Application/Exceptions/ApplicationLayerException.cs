namespace YourRest.Application.Exceptions
{
    public class ApplicationLayerException : Exception
    {
        public ApplicationLayerException(string message) : base(message) { }
    }
}