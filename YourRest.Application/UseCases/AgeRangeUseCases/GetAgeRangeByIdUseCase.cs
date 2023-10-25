using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Age;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.AgeRangeUseCases
{
    public class GetAgeRangeByIdUseCase: IGetAgeRangeByIdUseCase
    {
        private readonly IAgeRangeRepository ageRangeRepository;
        public GetAgeRangeByIdUseCase(IAgeRangeRepository ageRangeRepository)
        {
            this.ageRangeRepository = ageRangeRepository;
        }
        public async Task<AgeRangeWithIdDto> ExecuteAsync(int ageRangeId, CancellationToken token)
        {
            var ageRange = await ageRangeRepository.GetAsync(ageRangeId);

            if (ageRange == null)
            {
                throw new EntityNotFoundException($"AgeRange with id {ageRangeId} not found"); 
            }
            AgeRangeWithIdDto ageRangeWithId = new AgeRangeWithIdDto() 
            {
                Id = ageRange.Id,
                AgeFrom = ageRange.AgeFrom,
                AgeTo = ageRange.AgeTo
            };
            return ageRangeWithId;
        }
    }
}
