namespace YourRest.Application.Dto.Models
{
    public class ResultDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CityId { get; set; }
    }
}