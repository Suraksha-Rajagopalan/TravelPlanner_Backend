using TravelPlannerAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITripShareService
    {
        Task<(bool Success, string ErrorMessage)> ShareTripAsync(int ownerId, TripShareRequestDto request);
        Task<IEnumerable<SharedTripDto>> GetTripsSharedWithUserAsync(int userId);
    }
}
