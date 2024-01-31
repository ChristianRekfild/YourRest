using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetAccomodationFilesPathByAccomodationIdUseCase
    {
        Task<AccommodationDto> ExecuteAsync(int accomodationId,CancellationToken cancellationToken);
    }
}
