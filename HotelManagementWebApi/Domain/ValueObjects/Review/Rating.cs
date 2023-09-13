namespace HotelManagementWebApi.Domain.ValueObjects.Review
{
    public class Rating
    {
        private readonly double _value;

        public Rating(double value)
        {
            if (value < 1 || value > 5) 
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }
               
            _value = value;
        }

        public double Value => _value;
    }
}