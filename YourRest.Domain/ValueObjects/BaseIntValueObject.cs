namespace YourRest.Domain.ValueObjects
{
    public abstract class BaseIntValueObject
    {
        protected readonly int _value;

        protected BaseIntValueObject(int value)
        {
            _value = value;
        }

        public int Value => _value;
    }
}