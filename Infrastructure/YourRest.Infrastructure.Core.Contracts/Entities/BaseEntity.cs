namespace YourRest.Infrastructure.Core.Contracts.Entities
{
    public abstract class BaseEntity<T> where T : notnull
    {
        public T Id { get; set; }
    }
}
