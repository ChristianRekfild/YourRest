using System.Numerics;
using YourRestDomain.Abstractions;

namespace YourRestDomain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
    public abstract class BaseEntity<T> : IBaseEntity<T> where T : INumber<int>
    {
        public T Id { get; set; }
    }
}
