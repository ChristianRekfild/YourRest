using YourRest.Application.Dto;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Age;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.AgeRangeUseCases
{
    public class EditAgeRangeUseCase : IEditAgeRangeUseCase
    {
        private readonly IAgeRangeRepository _ageRangeRepository;

        public EditAgeRangeUseCase(IAgeRangeRepository ageRangeRepository)
        {
            this._ageRangeRepository = ageRangeRepository;
        }
        public async Task ExecuteAsync(AgeRangeWithIdDto ageRangeWithIdDto, CancellationToken token = default)
        {
            var ageRange = await _ageRangeRepository.GetAsync(ageRangeWithIdDto.Id , token);

            if (ageRange == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {ageRangeWithIdDto.Id} not found");
            }

            var ageRangeUpdate = new Infrastructure.Core.Contracts.Models.AgeRangeDto()
            {
                Id = ageRangeWithIdDto.Id,
                AgeFrom = ageRangeWithIdDto.AgeFrom,
                AgeTo = ageRangeWithIdDto.AgeTo
            };

            await _ageRangeRepository.UpdateAsync(ageRangeUpdate, true, token);
        }
    }
}
