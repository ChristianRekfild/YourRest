using YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Dto.Models
{
    public class CityDTOWithPhotos : CityDTO
    {
        public List<PhotoPathResponseDto> Photos { get; set; }
    }
}
