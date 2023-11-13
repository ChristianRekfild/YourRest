using YourRest.Application.Dto;
using  YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.Interfaces.Photo
{
    public interface IRoomPhotoUploadUseCase
    {
        Task<PhotoUploadResponseDto> Handle(RoomPhotoUploadModel request, string bucketName);
    }
}
