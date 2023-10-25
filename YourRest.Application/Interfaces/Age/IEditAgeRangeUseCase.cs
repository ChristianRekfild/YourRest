using YourRest.Application.Dto;
using YourRest.Domain.Entities;

namespace YourRest.Application.Interfaces.Age
{
    public interface IEditAgeRangeUseCase
    {
        Task ExecuteAsync(AgeRangeWithIdDto ageRangeWithIdDto, CancellationToken token);
    }
}
