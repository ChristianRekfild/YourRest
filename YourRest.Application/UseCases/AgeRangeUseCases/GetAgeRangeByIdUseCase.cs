using YourRest.Application.Dto;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Age;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.AgeRangeUseCases
{
    public class GetAgeRangeByIdUseCase: IGetAgeRangeByIdUseCase
    {
        private readonly IAgeRangeRepository ageRangeRepository;
        public GetAgeRangeByIdUseCase(IAgeRangeRepository ageRangeRepository)
        {
            this.ageRangeRepository = ageRangeRepository;
        }
        public async Task<AgeRangeWithIdDto> ExecuteAsync(int ageRangeId, CancellationToken token = default)
        {
            var ageRange = await ageRangeRepository.GetAsync(ageRangeId, token);

            if (ageRange == null)
            {
                throw new EntityNotFoundException($"AgeRange with id {ageRangeId} not found"); 
            }

            // TODO: Заменить на использование Automapper
            var ageRangeWithId = new AgeRangeWithIdDto() 
            {
                Id = ageRange.Id,
                AgeFrom = ageRange.AgeFrom,
                AgeTo = ageRange.AgeTo
            };
            return ageRangeWithId;
        }
    }
}
