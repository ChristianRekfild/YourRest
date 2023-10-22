using YourRest.Application.CustomErrors;
using YourRest.Application.Dto;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    internal class GetAccommodationByIdUseCase : IGetAccommodationByIdUseCase
    {
        private readonly IAccommodationRepository _accommodationRepository;

        public GetAccommodationByIdUseCase(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
        }

        public async Task<AccommodationDto> Execute(int id)
        {
            var accommodation = await _accommodationRepository.GetAsync(id);

            if (accommodation == null)
                throw new AccommodationNotFoundException(id);

            return new AccommodationDto
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                BankAccount = accommodation.BankAccount,
                AddressId = accommodation.AddressId,
                Address = accommodation.Address
            };
        }
    }
}
