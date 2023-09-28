namespace YourRest.Domain.ValueObjects.Addresses
{
    public class AddressTypeVO
    {
        public string Value { get; private set; }

        public static readonly AddressTypeVO Fact = new AddressTypeVO("Fact");
        public static readonly AddressTypeVO Legal = new AddressTypeVO("Legal");

        private AddressTypeVO(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Type cannot be null or empty.");
            }

            Value = value;
        }

        public static AddressTypeVO Create(string value)
        {
            switch(value)
            {
                case "Fact":
                    return Fact;
                case "Legal":
                    return Legal;
                default:
                    throw new ArgumentException($"Invalid type: {value}");
            }
        }
    }
}
