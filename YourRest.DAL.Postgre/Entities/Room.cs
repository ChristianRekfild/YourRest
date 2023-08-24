using YourRest.DAL.Contracts;

namespace YourRest.DAL.Postgre.Entities
{
    public class Room : IntBaseEntity
    {
        public string Name { get; set; }
    }
}
