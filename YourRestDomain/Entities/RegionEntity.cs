namespace YourRestDomain.Entities
{
    //TODO: �������� domain model � entity
    public class RegionEntity : BaseEntity<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual CountryEntity Country { get; set; }
    }
}
