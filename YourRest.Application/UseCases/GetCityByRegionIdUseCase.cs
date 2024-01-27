using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Photo;
using YourRest.Domain.Entities;

namespace YourRest.Application.UseCases
{
    public class GetCityByRegionIdUseCase : IGetCityByRegionIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByRegionIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        
        public async Task<IEnumerable<CityDTOWithLastPhoto>> Execute(int regionId, bool isOnlyFavorite, CancellationToken cancellationToken)
        {
            var cities = await _cityRepository.GetCitiesWithPhotosByRegionAsync(regionId, isOnlyFavorite, cancellationToken);

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
