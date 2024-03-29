using YourRest.Application.Dto;
using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface IAccommodationPhotoUploadUseCase
    {
        Task<PhotoPathResponseDto> ExecuteAsync(PhotoUploadModel request, string bucketName, CancellationToken cancellationToken);
    }
}
