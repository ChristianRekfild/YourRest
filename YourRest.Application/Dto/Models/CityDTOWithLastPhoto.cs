using YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Dto.Models
{
    public class CityDTOWithLastPhoto : CityDTO
    {
        public PhotoPathResponseDto? LastPhoto { get; set; }
    }
}
