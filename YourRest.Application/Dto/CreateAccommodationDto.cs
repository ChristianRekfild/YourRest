namespace YourRest.Application.Dto
{
    public class CreateAccommodationDto
    {
        public string Name { get; set; }
        public int AccommodationTypeId { get; set; }
        public int? Stars { get; set; }
        public string Description { get; set; }
        public List<string>? FilesPath { get; set; }
    }
}