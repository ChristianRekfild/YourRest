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
    public class AccommodationPhotoUploadUseCase : IAccommodationPhotoUploadUseCase
    {
        private readonly IAccommodationPhotoRepository _photoRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IFileService _fileService;

        public AccommodationPhotoUploadUseCase(
            IAccommodationPhotoRepository photoRepository, 
            IAccommodationRepository accommodationRepository,
            IFileService fileService
            )
        {
            _photoRepository = photoRepository;
            _accommodationRepository = accommodationRepository;
            _fileService = fileService;
        }
        public async Task<PhotoUploadResponseDto> ExecuteAsync(PhotoUploadModel photoUploadModel, string bucketName, CancellationToken cancellationToken)
        {
            var accommodation = await _accommodationRepository.GetAsync(photoUploadModel.AccommodationId, cancellationToken);

            if (accommodation == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {photoUploadModel.AccommodationId} not found");
            }
            
            var fileData = new FileData
            {
                Content = photoUploadModel.Photo.OpenReadStream(),
                FileName = photoUploadModel.Photo.FileName
            };

            var photoPath = await _fileService.AddPhotoAsync(fileData, bucketName, cancellationToken);
            
            var photo = new AccommodationPhoto()
            {
                Accommodation = accommodation,
                FilePath = photoPath
            };

            var savedPhoto = await _photoRepository.AddAsync(photo, false, cancellationToken);
            await _photoRepository.SaveChangesAsync(cancellationToken);

            return new PhotoUploadResponseDto() { FilePath = savedPhoto.FilePath };
        }
    }
}
