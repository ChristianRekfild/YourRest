namespace HotelManagementWebApi.Domain.ValueObjects
{
    public abstract class BaseDateValueObject
    {
        protected readonly DateTime _value;

        protected BaseDateValueObject(DateTime value)
        {
            _value = value;
        }

        public DateTime Value => _value;
    }
}