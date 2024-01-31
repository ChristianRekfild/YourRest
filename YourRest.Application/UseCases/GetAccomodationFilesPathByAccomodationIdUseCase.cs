using YourRest.Application.Dto;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetAccomodationFilesPathByAccomodationIdUseCase : IGetAccomodationFilesPathByAccomodationIdUseCase
    {
        private readonly IAccommodationRepository accommodationRepository;

        public GetAccomodationFilesPathByAccomodationIdUseCase(IAccommodationRepository accommodationRepository)
        {
            this.accommodationRepository = accommodationRepository;
        }

        public async Task<AccommodationDto> ExecuteAsync(int accomodationId, CancellationToken cancellationToken)
        {
            var foundEntity = (await accommodationRepository.GetWithIncludeAsync(acc => acc.Id == accomodationId, cancellationToken, 
                include => include.AccommodationPhotos, 
                include => include.AccommodationType)).FirstOrDefault();
            if (foundEntity is null) throw new EntityNotFoundException($"accommodation with id {accomodationId} not found");

            return new AccommodationDto
            {
                Id = foundEntity.Id,
                AccommodationType = new() { Name = foundEntity.AccommodationType.Name, Id = foundEntity.AccommodationType.Id },
                Description = foundEntity.Description,
                Name = foundEntity.Name,
                Stars = foundEntity.StarRating?.Stars,
                FilesPath = foundEntity.AccommodationPhotos?.Select(accPhoto => accPhoto.FilePath).ToList(),
            };
        }
    }
}
