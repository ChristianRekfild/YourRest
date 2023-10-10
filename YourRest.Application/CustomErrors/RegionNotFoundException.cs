namespace YourRest.Application.CustomErrors
{
    public class RegionNotFoundException : Exception
    {
        public int RegionId { get; }

        public RegionNotFoundException(string message)
        : base(message) { }

        public RegionNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public RegionNotFoundException(string message, int regionId)
            : this(message)
        {
            RegionId = regionId;
        }
    }
}
