using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces.Age;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

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
            var ageRange = new AgeRange()
            { 
                AgeFrom = ageRangeDto.AgeFrom,  
                AgeTo = ageRangeDto.AgeTo 
            };
            var savedAgeRange = await _ageRangeRepository.AddAsync(ageRange , true,  token);

            AgeRangeWithIdDto ageRangeWithId = new AgeRangeWithIdDto()
            {
                Id = savedAgeRange.Id,
                AgeFrom = savedAgeRange.AgeFrom,
                AgeTo = savedAgeRange.AgeTo
            };
            return ageRangeWithId;
        }
    }
}
