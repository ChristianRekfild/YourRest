namespace YourRest.Domain.ValueObjects.Reviews
{
    public class RatingVO
    {
        public int Value { get; private set; }

        private RatingVO(int value)
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentException("Rating value must be between 1 and 5.");
            }

            Value = value;
        }

        public static RatingVO Create(int value)
        {
            return new RatingVO(value);
        }
    }
}