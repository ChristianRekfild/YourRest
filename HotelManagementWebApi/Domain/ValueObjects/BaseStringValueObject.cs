namespace HotelManagementWebApi.Domain.ValueObjects
{
    public abstract class BaseStringValueObject
    {
        protected readonly string _value;

        protected BaseStringValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be empty.");

            _value = value;
        }

        public string Value => _value;
    }
}