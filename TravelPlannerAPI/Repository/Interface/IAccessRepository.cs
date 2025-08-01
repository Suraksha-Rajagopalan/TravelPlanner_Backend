﻿using System.Threading.Tasks;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IAccessRepository
    {
        /// <summary>Returns true if the trip exists and is owned by the user.</summary>
        Task<bool> IsOwnerAsync(int tripId, int userId);

        /// <summary>Returns true if the trip is shared with the user.</summary>
        Task<bool> IsSharedAsync(int tripId, int userId);
    }
}
