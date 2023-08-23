using YourRest.DAL.Contracts;

namespace YourRest.DAL.Postgre.Entities
{
    //TODO: разнести domain model и entity
    public class Country : IntBaseEntity
    {       
        public string Name { get; set; }
    }
}
