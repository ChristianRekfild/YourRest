namespace YourRest.Domain.Entities
{
    public class CityPhoto : IntBaseEntity
    {
        public string FilePath { get; set; }
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
