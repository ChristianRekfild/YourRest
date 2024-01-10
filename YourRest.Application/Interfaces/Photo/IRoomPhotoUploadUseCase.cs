using YourRest.Application.Dto;
using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface IRoomPhotoUploadUseCase
    {
        Task<PhotoPathResponseDto> ExecuteAsync(RoomPhotoUploadModel request, string bucketName, CancellationToken cancellationToken);
    }
}
