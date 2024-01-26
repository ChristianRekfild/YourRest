using YourRest.Application.Interfaces.Photo;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;
using YourRest.Application.Exceptions;
using YourRest.Application.Services;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.UseCases.Photo
{
    public class CityPhotoUploadUseCase : ICityPhotoUploadUseCase
    {
        private readonly ICityPhotoRepository _photoRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IFileService _fileService;

        public CityPhotoUploadUseCase(
            ICityPhotoRepository photoRepository, 
            ICityRepository cityRepository,
            IFileService fileService
            )
        {
            _photoRepository = photoRepository;
            _cityRepository = cityRepository;
            _fileService = fileService;
        }
        public async Task<PhotoPathResponseDto> ExecuteAsync(CityPhotoUploadModel request, string bucketName, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.GetAsync(request.CityId, cancellationToken);

            if (city == null)
            {
                throw new EntityNotFoundException($"City with id {request.CityId} not found");
            }
            
            var fileData = new FileData
            {
                Content = request.Photo.OpenReadStream(),
                FileName = request.Photo.FileName
            };

            var photoPath = await _fileService.AddPhotoAsync(fileData, bucketName, cancellationToken);
            
            var photo = new CityPhoto()
            {
                City = city,
                FilePath = photoPath
            };

            var savedPhoto = await _photoRepository.AddAsync(photo, false, cancellationToken);
            await _photoRepository.SaveChangesAsync(cancellationToken);

            return new PhotoPathResponseDto() { FilePath = savedPhoto.FilePath };
        }
    }
}
