using System.Numerics;

namespace YourRestDomain.Abstractions
{
    public interface IBaseEntity
    {
        Guid Id { get; set; } 
    }
    public interface IBaseEntity<T> where T : INumber<int>
    {
        T Id { get; set; }
    }
}
