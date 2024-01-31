namespace YourRest.Application.Dto.Models
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsFavorite { get; set; }
    }
}
