using YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Dto.Models
{
    public class CityDTOWithPhotos
    {
        public CityDTO City { get; set; }
        public List<PhotoPathResponseDto> Photos { get; set; }
    }
}
