using YourRest.Application.Interfaces.Photo;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;
using YourRest.Application.Exceptions;
using YourRest.Application.Services;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.UseCases.Photo
{
    public class UserPhotoUploadUseCase : IUserPhotoUploadUseCase
    {
        private readonly IUserPhotoRepository _photoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public UserPhotoUploadUseCase(
            IUserPhotoRepository photoRepository, 
            IUserRepository userRepository,
            IFileService fileService
            )
        {
            _photoRepository = photoRepository;
            _userRepository = userRepository;
            _fileService = fileService;
        }
        public async Task<PhotoPathResponseDto> ExecuteAsync(UserPhotoUploadModel request, string bucketName, string userKeyCloakId, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(a => a.KeyCloakId == userKeyCloakId, cancellationToken);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(userKeyCloakId);
            }
            
            var fileData = new FileData
            {
                Content = request.Photo.OpenReadStream(),
                FileName = request.Photo.FileName
            };

            var photoPath = await _fileService.AddPhotoAsync(fileData, bucketName, cancellationToken);
            
            var photo = new UserPhoto()
            {
                User = user,
                FilePath = photoPath
            };

            var savedPhoto = await _photoRepository.AddAsync(photo, false, cancellationToken);
            await _photoRepository.SaveChangesAsync(cancellationToken);

            return new PhotoPathResponseDto() { FilePath = savedPhoto.FilePath };
        }
    }
}
