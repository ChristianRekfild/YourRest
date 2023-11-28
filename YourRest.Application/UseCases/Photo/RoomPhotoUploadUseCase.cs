using Amazon.S3;
using Amazon.S3.Model;
using YourRest.Application.Interfaces.Photo;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;
using YourRest.Application.Exceptions;
using YourRest.Application.Services;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.UseCases.Photo
{
    public class RoomPhotoUploadUseCase : IRoomPhotoUploadUseCase
    {
        private readonly IRoomPhotoRepository _photoRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IFileService _fileService;

        public RoomPhotoUploadUseCase(
            IRoomPhotoRepository photoRepository, 
            IRoomRepository roomRepository,
            IFileService fileService
            )
        {
            _photoRepository = photoRepository;
            _roomRepository = roomRepository;
            _fileService = fileService;
        }
        public async Task<PhotoUploadResponseDto> ExecuteAsync(RoomPhotoUploadModel request, string bucketName, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken);

            if (room == null)
            {
                throw new EntityNotFoundException($"Room with id {request.RoomId} not found");
            }
            
            var fileData = new FileData
            {
                Content = request.Photo.OpenReadStream(),
                FileName = request.Photo.FileName
            };

            var photoPath = await _fileService.AddPhotoAsync(fileData, bucketName, cancellationToken);
            
            var photo = new RoomPhoto()
            {
                Room = room,
                FilePath = photoPath
            };

            var savedPhoto = await _photoRepository.AddAsync(photo, false, cancellationToken);
            await _photoRepository.SaveChangesAsync(cancellationToken);

            return new PhotoUploadResponseDto() { FilePath = savedPhoto.FilePath };
        }
    }
}
