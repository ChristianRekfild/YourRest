using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetCityListUseCase : IGetCityListUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityListUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<CityDTOWithLastPhoto>> Execute(bool isOnlyFavorite, CancellationToken cancellationToken)
        {

            var cities = await _cityRepository.GetCitiesWithPhotosAsync(isOnlyFavorite, cancellationToken);

            return cities.Select(c => new CityDTOWithLastPhoto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsFavorite = c.IsFavorite,
                LastPhoto = c.CityPhotos.OrderByDescending(photo => photo.Id)
                    .Select(photo => new PhotoPathResponseDto
                    {
                        FilePath = photo.FilePath
                    })
                    .FirstOrDefault()
            }).ToList();
        }
    }
}
