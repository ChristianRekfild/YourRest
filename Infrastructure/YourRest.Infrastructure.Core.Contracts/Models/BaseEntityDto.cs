namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public abstract class BaseEntityDto<T> where T : notnull
    {
        public T Id { get; set; }
    }
}
