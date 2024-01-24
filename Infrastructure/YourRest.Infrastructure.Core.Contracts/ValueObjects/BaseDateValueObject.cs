namespace YourRest.Infrastructure.Core.Contracts.ValueObjects
{
    public abstract class BaseDateValueObject
    {
        protected readonly DateTime _value;

        protected BaseDateValueObject(DateTime value)
        {
            _value = value.ToUniversalTime();
        }

        public DateTime Value => _value;
    }
}
