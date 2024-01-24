using YourRest.Application.Dto;
using YourRest.Application.Interfaces.Age;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.AgeRangeUseCases
{
    public class CreateAgeRangeUseCase : ICreateAgeRangeUseCase
    {
        private readonly IAgeRangeRepository _ageRangeRepository;
        public CreateAgeRangeUseCase(IAgeRangeRepository ageRangeRepository) 
        {
            this._ageRangeRepository = ageRangeRepository;
        }

        public async Task<AgeRangeWithIdDto> ExecuteAsync(AgeRangeDto ageRangeDto, CancellationToken token = default) 
        {
            var ageRange = new Infrastructure.Core.Contracts.Models.AgeRangeDto()
            { 
                AgeFrom = ageRangeDto.AgeFrom,  
                AgeTo = ageRangeDto.AgeTo 
            };
            var savedAgeRange = await _ageRangeRepository.AddAsync(ageRange , true,  token);

            // TODO: Заменить на Automapper
            var ageRangeWithId = new AgeRangeWithIdDto()
            {
                Id = savedAgeRange.Id,
                AgeFrom = savedAgeRange.AgeFrom,
                AgeTo = savedAgeRange.AgeTo
            };
            return ageRangeWithId;
        }
    }
}
