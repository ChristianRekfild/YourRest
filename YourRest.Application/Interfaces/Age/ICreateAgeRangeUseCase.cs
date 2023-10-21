using YourRest.Application.Dto;
using YourRest.Domain.Entities;

namespace YourRest.Application.Interfaces.Age
{
    public interface ICreateAgeRangeUseCase
    {
        Task<AgeRangeWithIdDto> ExecuteAsync(AgeRangeDto ageRangeDto);
    }
}
