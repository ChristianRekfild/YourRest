﻿using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class RemoveRoomFacilityUseCase : IRemoveRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;

        public RemoveRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
        }
        public async Task ExecuteAsync(RoomFacilityViewModel reviewDto)
        {
            if (await roomFacilityRepository.GetAsync(reviewDto.Id) is not RoomFacility roomFacility)
            {
                throw new RoomFacilityNotFoundException(reviewDto.Id);
            }
            await roomFacilityRepository.DeleteAsync(reviewDto.Id);
        }
    }
}