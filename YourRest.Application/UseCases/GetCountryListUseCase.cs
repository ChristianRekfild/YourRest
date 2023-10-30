using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCountryListUseCase : IGetCountryListUseCase
    {
        private readonly ICountryRepository _countryRepository;

        public GetCountryListUseCase(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<IEnumerable<CountryDto>> Execute()
        {
            var countries = await _countryRepository.GetAllAsync();

            return countries.Select(c => new CountryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}
