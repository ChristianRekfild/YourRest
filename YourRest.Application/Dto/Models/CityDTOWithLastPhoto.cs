using YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Dto.Models
{
    public class CityDTOWithLastPhoto
    {
        public CityDTO City { get; set; }
        public PhotoPathResponseDto? LastPhoto { get; set; }
    }
}