using System.Threading.Tasks;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;

namespace TravelPlannerAPI.Services.Implementations
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _repo;

        public AccessService(IAccessRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> HasAccessToTripAsync(int tripId, int userId)
        {
            if (await _repo.IsOwnerAsync(tripId, userId))
                return true;

            return await _repo.IsSharedAsync(tripId, userId);
        }

        public async Task<AccessLevel?> GetAccessLevelAsync(int tripId, int userId)
        {
            if (await _repo.IsOwnerAsync(tripId, userId))
                return AccessLevel.Edit;

            if (await _repo.IsSharedAsync(tripId, userId))
                return AccessLevel.View;

            return null; // No access
        }

    }
}
