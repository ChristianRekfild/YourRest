using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces.Age
{
    public interface IEditAgeRangeUseCase
    {
        Task ExecuteAsync(AgeRangeWithIdDto ageRangeWithIdDto, CancellationToken token = default);
    }
}
