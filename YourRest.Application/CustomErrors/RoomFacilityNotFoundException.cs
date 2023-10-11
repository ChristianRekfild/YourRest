﻿using YourRest.Application.Dto.Models;

namespace YourRest.Application.CustomErrors
{
    public class RoomFacilityNotFoundException : Exception
    {
        public RoomFacilityNotFoundException(string msg) : base(msg) { }
        public RoomFacilityNotFoundException(int facilityId) :
            base($"RoomFacility with id number {facilityId} not found")
        {
        }

    }
}
