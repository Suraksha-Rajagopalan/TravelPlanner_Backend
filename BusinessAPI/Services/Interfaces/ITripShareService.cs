using BusinessAPI.Dtos;
using BusinessAPI.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface ITripShareService
    {
        Task<(bool Success, string ErrorMessage)> ShareTripAsync(int ownerId, TripShareRequestDto request);
        Task<List<SharedTripDto>> GetTripsSharedWithUserAsync(int userId);
    }
}
