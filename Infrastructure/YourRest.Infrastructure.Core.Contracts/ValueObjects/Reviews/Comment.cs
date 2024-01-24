using YourRest.Infrastructure.Core.ContractsValueObjects;

namespace YourRest.Infrastructure.Core.Contracts.ValueObjects.Reviews
{
    public class Comment : BaseStringValueObject
    {
        public Comment(string value) : base(value)
        {
            if (value.Length > 500) {
                throw new ArgumentException("Comment cannot be longer than 500 characters.");
            }
                
        }
    }
}
