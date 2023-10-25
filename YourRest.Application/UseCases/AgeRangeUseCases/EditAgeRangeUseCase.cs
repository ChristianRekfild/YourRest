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
    public class EditAgeRangeUseCase : IEditAgeRangeUseCase
    {
        private readonly IAgeRangeRepository _ageRangeRepository;

        public EditAgeRangeUseCase(IAgeRangeRepository ageRangeRepository)
        {
            this._ageRangeRepository = ageRangeRepository;
        }
        public async Task ExecuteAsync(AgeRangeWithIdDto ageRangeWithIdDto)
        {
            var ageRange = await _ageRangeRepository.GetAsync(ageRangeWithIdDto.Id);

            if (ageRange == null)
            {
                throw new EntityNotFoundException($"Accommodation with id {ageRangeWithIdDto.Id} not found");
            }

            AgeRange ageRangeUpdate = new AgeRange()
            {
                Id = ageRangeWithIdDto.Id,
                AgeFrom = ageRangeWithIdDto.AgeFrom,
                AgeTo = ageRangeWithIdDto.AgeTo
            };

            await _ageRangeRepository.UpdateAsync(ageRangeUpdate);
        }
    }
}
