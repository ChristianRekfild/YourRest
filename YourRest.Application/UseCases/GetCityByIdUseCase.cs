using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Photo;

namespace YourRest.Application.UseCases
{
    public class GetCityByIdUseCase : IGetCityByIdUseCase
    {
        private readonly ICityRepository _cityRepository;

        public GetCityByIdUseCase(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        
        public async Task<CityDTOWithPhotos> Execute(int id)
        {
            var city = await _cityRepository.GetCityWithPhotosAsync(id);

            if (city is null) throw new EntityNotFoundException($"City with id {id} not found");
    
            var photos = city.CityPhotos.Select(photo => new PhotoPathResponseDto
            {
                FilePath = photo.FilePath
            }).ToList();
    
            return new CityDTOWithPhotos()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description,
                IsFavorite = city.IsFavorite,
                Photos = photos
            };
        }
    }
}
