using AutoMapper;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Photo;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetUserPhotosUseCase : IGetUserPhotosUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserPhotosUseCase(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PhotoPathResponseDto>> ExecuteAsync(string userKeyCloakId, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUserWithPhotosAsync(userKeyCloakId, cancellationToken);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException(userKeyCloakId);
            }
            
            var photoDtos = user.UserPhotos.Select(photo => new PhotoPathResponseDto
            {
                FilePath = photo.FilePath
            }).ToList();

            return photoDtos;
        }
    }
}
