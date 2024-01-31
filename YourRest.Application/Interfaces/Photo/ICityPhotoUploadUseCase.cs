using YourRest.Application.Dto;
using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface ICityPhotoUploadUseCase
    {
        Task<PhotoPathResponseDto> ExecuteAsync(CityPhotoUploadModel request, string bucketName, CancellationToken cancellationToken);
    }
}
