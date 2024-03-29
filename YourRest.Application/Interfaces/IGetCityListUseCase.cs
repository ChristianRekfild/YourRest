﻿using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetCityListUseCase
    {
        Task<IEnumerable<CityDTOWithLastPhoto>> Execute(bool isOnlyFavorite, CancellationToken cancellationToken);
    }
}
