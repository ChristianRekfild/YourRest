using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces.Age
{
    public interface ICreateAgeRangeUseCase
    {
        Task<AgeRangeWithIdDto> ExecuteAsync(AgeRangeDto ageRangeDto, CancellationToken token = default);
    }
}
